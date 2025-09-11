using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.RabbitMqPublisher
{
    public interface IRabbitMqPublisher
    {
        void Publish<T>(T message, string queueName);
    }
}
