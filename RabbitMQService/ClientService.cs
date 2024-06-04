using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQService
{
    public class ClientService
    {
        public string ExchangeName = "AdCreativeExchange";
        public string QueueName = "AdCreativeQueue";
        public string RouteKey = "AdCreativeRouteKey";

        private IConnection _connection;
        private IModel _channel;

        private static ConnectionFactory _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };

        /// <summary>
        /// RabbitMQ tarafında bağlantı açar.
        /// </summary>
        /// <returns></returns>
        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, type: ExchangeType.Direct, true, false);

            _channel.QueueDeclare(QueueName, true, false, false);

            _channel.QueueBind(QueueName, ExchangeName, RouteKey);

            return _channel;
        }
    }
}
