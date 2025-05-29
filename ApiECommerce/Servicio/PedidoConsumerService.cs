using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ApiECommerce.DTOs;

namespace ApiECommerce.Servicio
{
    public class PedidoConsumerService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private IConsumer<string, string> _consumer;
        private Task _consumerTask;
        private CancellationTokenSource _cts;

        public PedidoConsumerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "pedido-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("pedidos-eventos");

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _consumerTask = Task.Run(() => EjecutarConsumidorAsync(_cts.Token), _cts.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();

            try
            {
                await _consumerTask;
            }
            catch (OperationCanceledException)
            {
                // Cancelación esperada
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task EjecutarConsumidorAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(stoppingToken);
                    var evento = JsonSerializer.Deserialize<PedidoEventoDTO>(cr.Message.Value);

                    await EnviarNotificacionConResend(evento);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consumir evento: {ex.Message}");
                }
            }
        }

        /* private async Task EnviarNotificacionConResend(PedidoEventoDTO evento)
         {
             Console.WriteLine($"Notificación enviada para el pedido #{evento.PedidoId}");
             //enviar notificacion con resend 


             await Task.CompletedTask; // Simula una llamada async real
         }*/
        private async Task EnviarNotificacionConResend(PedidoEventoDTO evento)
        {
            var apiKey = _configuration["Resend:ApiKey"]; // Guarda tu API Key en appsettings.json
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var emailData = new
            {
                from = "onboarding@resend.dev",
                to = "klopezh16@miumg.edu.gt", 
                subject = $"Pedido #{evento.PedidoId} recibido",
                html = $"<p>¡Gracias por tu pedido #{evento.PedidoId}!</p>"
            };

            var response = await httpClient.PostAsJsonAsync(
                "https://api.resend.com/emails",
                emailData
            );

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Notificación enviada para el pedido #{evento.PedidoId}");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al enviar notificación: {error}");
            }
        }
    }
}
