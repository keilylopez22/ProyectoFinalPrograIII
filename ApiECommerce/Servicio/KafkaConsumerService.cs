using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resend;
using System.Text.Json;
using Confluent.Kafka;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ApiECommerce.DTOs;
using ApiECommerce.Data;
using ApiECommerce.Modelo;
using ApiECommerce.DTOs;

namespace ApiECommerce.Servicio
{
    public class PedidoConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;

        public PedidoConsumerService(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "pedido-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("pedidos-eventos"); // el nombre del topic
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(stoppingToken);

                    // Aquí deserializas y procesas el evento
                    var evento = JsonSerializer.Deserialize<PedidoEventoDTO>(cr.Message.Value);

                    // Aquí podrías llamar a Resend para notificar
                    await EnviarNotificacionConResend(evento);
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine($"Error al consumir evento: {ex.Message}");
                }
            }

            _consumer.Close();
        }

        private async Task EnviarNotificacionConResend(PedidoEventoDTO evento)
        {
            // Aquí iría la lógica para notificar con Resend
            Console.WriteLine($"Notificación enviada para el pedido #{evento.PedidoId}");
        }
    }
}
