using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Api.Models;

namespace Api.Services
{
    public class ProcessOrdersService : BackgroundService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _inputQueueName;
        private readonly string _outputQueueName;

        public ProcessOrdersService(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
            _inputQueueName = "orderrequests";
            _outputQueueName = "readytoship";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("OrderProcessing Service Started");

            var processor = _serviceBusClient.CreateProcessor(_inputQueueName);

            processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) =>
            {
                string orderRequest = args.Message.Body.ToString();

                //Deserialize
                OrderRequest order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);

                //TODO:: Process Order
                Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                order.status = OrderStatus.COMPLETED;

                //Write to ReadyToShip Queue
                var sender = _serviceBusClient.CreateSender(_outputQueueName);
                var message = new ServiceBusMessage(JsonConvert.SerializeObject(order));
                await sender.SendMessageAsync(message);

                await args.CompleteMessageAsync(args.Message);
            };

            processor.ProcessErrorAsync += (ProcessErrorEventArgs args) =>
            {
                Console.WriteLine($"Error: {args.Exception.Message}");
                return Task.CompletedTask;
            };

            await processor.StartProcessingAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await processor.StopProcessingAsync(stoppingToken);
        }
    }
}