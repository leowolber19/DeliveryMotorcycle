using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.RabbitMqPublisher
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly IModel _channel;

        public RabbitMqPublisher(IModel channel)
        {
            _channel = channel;
        }

        public void Publish<T>(T message, string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
