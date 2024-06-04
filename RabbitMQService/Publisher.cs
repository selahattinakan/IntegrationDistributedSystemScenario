using System.Collections.Concurrent;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQService
{
    public class Publisher
    {
        private ClientService _clientService { get; set; } = new();

        /// <summary>
        /// Gelen mesajı kuyruğa ekler
        /// </summary>
        /// <param name="message"></param>
        public void Publish(string message)
        {
            var channel = _clientService.Connect();

            var bodyByte = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(_clientService.ExchangeName, _clientService.RouteKey, properties, bodyByte);
        }
    }
}
