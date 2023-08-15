using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microservice.DTOs;

namespace Microservice.Services
{
    public class ServiceBusQueue
    {
        private readonly QueueClient _queueClient;

        public ServiceBusQueue(IOptions<ServiceBusQueueSettings> settings)
        {
            _queueClient = new QueueClient(settings.Value.ConnectionString, settings.Value.QueueName);
        }

        public async Task SendMessageAsync(MyDto dto)
        {
            var messageBody = JsonConvert.SerializeObject(dto);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await _queueClient.SendAsync(message);
        }
    }
}
