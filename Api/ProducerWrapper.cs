using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Api
{
    public class ProducerWrapper
    {
        private string _topicName;
        private ServiceBusSender _sender;
        private ServiceBusClient _client;
        private static readonly Random rand = new Random();

        public ProducerWrapper(string connectionString, string topicName)
        {
            this._topicName = topicName;
            this._client = new ServiceBusClient(connectionString);
            this._sender = _client.CreateSender(topicName);
        }

        public async Task writeMessage(string message)
        {
            var serviceBusMessage = new ServiceBusMessage(message)
            {
                MessageId = rand.Next(5).ToString()
            };

            try
            {
                await _sender.SendMessageAsync(serviceBusMessage);
                Console.WriteLine($"SERVICE BUS => Delivered '{message}' to '{_topicName}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _sender.DisposeAsync();
            await _client.DisposeAsync();
        }
    }
}