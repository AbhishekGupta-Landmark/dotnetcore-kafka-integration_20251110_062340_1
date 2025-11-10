using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace Api
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;

        public ConsumerWrapper(string connectionString, string topicName)
        {
            this._topicName = topicName;
            this._client = new ServiceBusClient(connectionString);
            this._processor = _client.CreateProcessor(topicName);
        }

        public async Task<string> ReadMessageAsync()
        {
            string message = null;
            _processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) => 
            {
                message = args.Message.Body.ToString();
                await args.CompleteMessageAsync(args.Message);
            };

            await _processor.StartProcessingAsync();

            // Wait for message
            while (message == null)
            {
                await Task.Delay(100);
            }

            await _processor.StopProcessingAsync();

            return message;
        }
    }
}