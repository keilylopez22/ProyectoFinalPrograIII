using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiECommerce.DTOs;
using ApiECommerce.Data;
using ApiECommerce.Modelo;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace ApiECommerce.Servicio
{
    public class KafkaPedidoConsumidor : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private CancellationTokenSource _cts;
        private Task _backgroundTask;

        public KafkaPedidoConsumidor(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Iniciar la tarea en segundo plano
            _backgroundTask = Task.Run(() => EjecutarConsumidorKafkaAsync(_cts.Token), _cts.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return _backgroundTask ?? Task.CompletedTask;
        }

        private async Task EjecutarConsumidorKafkaAsync(CancellationToken stoppingToken)
        {
            // Esperar unos segundos para asegurar que la API está totalmente iniciada
            await Task.Delay(5000, stoppingToken);

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
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result?.Message?.Value == null) continue;

                    var pedidoDto = JsonSerializer.Deserialize<PedidoKafkaDTO>(result.Message.Value);

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error de consumo Kafka: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error general: {ex.Message}");
                }
            }
        }
    }
}
