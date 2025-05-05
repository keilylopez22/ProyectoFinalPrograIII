using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.DTOs;
using ApiECommerce.Data;
using ApiECommerce.Modelo;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;


namespace ApiECommerce.Servicio
{
    public class KafkaPedidoConsumidor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public KafkaPedidoConsumidor(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = "grupo-pedidos",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("crear-pedido");

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);
            var pedidoDto = JsonSerializer.Deserialize<PedidoKafkaDTO>(result.Message.Value);

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // procesamiento similar al servicio original
            decimal total = 0;
            var detalles = new List<DetallePedido>();

            foreach (var item in pedidoDto.DetallesPedido)
            {
                var producto = await db.productos.FindAsync(item.IdProductos);
                if (producto == null || producto.Existencias < item.CantidadProductos)
                    continue;

                var subtotal = producto.Precio * item.CantidadProductos;
                producto.Existencias -= item.CantidadProductos;

                detalles.Add(new DetallePedido
                {
                    IdProductos = item.IdProductos,
                    CantidadProductos = item.CantidadProductos,
                    PrecioUnitario = producto.Precio,
                    SubTotal = subtotal
                });

                total += subtotal;
            }

            var pedido = new Pedido
            {
                Fecha = pedidoDto.Fecha,
                IdCliente = pedidoDto.IdCliente,
                Total = total,
                Estado = "Pendiente",
                DetallesPedido = detalles
            };

            await db.pedidos.AddAsync(pedido);
            await db.SaveChangesAsync();
        }
    }
}

}