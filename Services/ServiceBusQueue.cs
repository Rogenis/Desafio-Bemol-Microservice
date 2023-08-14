using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Microservice.DTOs;

namespace Microservice.Services
{
    public class ServiceBusQueue : IAsyncDisposable
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _queueName;
        private QueueClient _queueClient;

        public ServiceBusQueue(IConfiguration configuration)
        {
            _serviceBusConnectionString = configuration.GetConnectionString("ServiceBusConnection");
            _queueName = "MyQueue";
            _queueClient = new QueueClient(_serviceBusConnectionString, _queueName);
        }

        public async Task SendMessageAsync(MyDto myDto)
        {
            string serializedDto = JsonConvert.SerializeObject(myDto);
            byte[] messageBody = Encoding.UTF8.GetBytes(serializedDto);

            var message = new Message(messageBody);

            await _queueClient.SendAsync(message);
        }

        public async ValueTask DisposeAsync()
        {
            if (_queueClient != null)
            {
                await _queueClient.CloseAsync();
                _queueClient = null;
            }
        }
    }
}
