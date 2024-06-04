using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Text.Json;
using Repository;
using Common;

namespace Listener
{
    public class Consumer
    {
        private ClientService _clientService { get; set; } = new();
        private ItemRepository _itemRepository { get; set; } = new();

        /// <summary>
        /// RabbitMQ tarafında kuyruktan çıkan verileri yakalar ve işlem yapar
        /// </summary>
        public void Listen()
        {
            var channel = _clientService.Connect();

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(_clientService.QueueName, false, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Item item = JsonSerializer.Deserialize<Item>(message);

                if (_itemRepository.FindItemsWithContent(item.Content).Count != 0)
                {
                    Console.WriteLine($"Duplicate item received with content {item.Content}.");
                }
                else
                {
                    _itemRepository.SaveItem(item);

                    Console.WriteLine($"Item with content {item.Content} saved with id {item.Id}");
                }

                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine();
        }
    }
}
