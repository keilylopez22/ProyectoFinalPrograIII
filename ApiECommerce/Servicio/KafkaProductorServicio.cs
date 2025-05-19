using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.DTOs;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace ApiECommerce.Servicio
{

    public interface IKafkaProductorServicio
    {
        Task EnviarPedidoAsync(PedidoKafkaDTO pedido);
        Task EnviarEventoAsync(string topic, PedidoEventoDTO evento);
    }
    public class KafkaProductorServicio : IKafkaProductorServicio
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProductorServicio(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task EnviarPedidoAsync(PedidoKafkaDTO pedido)
        {
            var mensajeJson = JsonSerializer.Serialize(pedido);
            //Topic de Kafka donde se enviará el mensaje
            await _producer.ProduceAsync("crear-pedido", new Message<Null, string> { Value = mensajeJson });
        }

        public async Task EnviarEventoAsync(string topic, PedidoEventoDTO evento)
        {
            var mensajeJson = JsonSerializer.Serialize(evento);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = mensajeJson });
        }
    }

}