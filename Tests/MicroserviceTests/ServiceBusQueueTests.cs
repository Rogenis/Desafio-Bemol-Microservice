using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Microservice.Services;
using Microservice.DTOs;

namespace Microservice.Tests.Services
{
    [TestFixture]
    public class ServiceBusQueueTests
    {
        private Mock<IOptions<ServiceBusQueueSettings>> _optionsMock;
        private Mock<QueueClient> _queueClientMock;
        private ServiceBusQueue _serviceBusQueue;

        [SetUp]
        public void Setup()
        {
            _optionsMock = new Mock<IOptions<ServiceBusQueueSettings>>();
            _queueClientMock = new Mock<QueueClient>();
            _optionsMock.Setup(options => options.Value)
                        .Returns(new ServiceBusQueueSettings
                        {
                            ConnectionString = "your-connection-string-here",
                            QueueName = "your-queue-name-here"
                        });

            _serviceBusQueue = new ServiceBusQueue(_optionsMock.Object);
        }

        [Test]
        public async Task SendMessageAsync_ValidDto_SendsMessageToQueue()
        {
            // Arrange
            var dto = new MyDto
            {
                id = "123",
                Key = "TestKey",
                AnotherKey = "AnotherTestKey"
            };

            // Act
            await _serviceBusQueue.SendMessageAsync(dto);

            // Assert
            _queueClientMock.Verify(client =>
                client.SendAsync(It.IsAny<Message>()),
                Times.Once);
        }

        // Add more test methods as needed
    }
}
