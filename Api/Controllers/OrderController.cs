using System;
using System.Threading.Tasks;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName = "orderrequests";

        public OrderController(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Received a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            await using var sender = _serviceBusClient.CreateSender(_queueName);
            var message = new ServiceBusMessage(serializedOrder);
            await sender.SendMessageAsync(message);

            return Created("TransactionId", "Your order is in progress");
        }
    }
}