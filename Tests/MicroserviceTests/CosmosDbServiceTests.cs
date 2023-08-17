using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microservice.Services;
using Moq;
using NUnit.Framework;

namespace Microservice.Tests.Services
{
    [TestFixture]
    public class CosmosDbServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private CosmosDbService _cosmosDbService;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(config => config.GetConnectionString("CosmosDBConnection"))
                             .Returns("your-connection-string-here");

            _cosmosDbService = new CosmosDbService(_configurationMock.Object);
        }

        [Test]
        public void GetContainer_ValidDatabaseAndContainer_ReturnsContainer()
        {
            // Arrange
            var databaseName = "YourDatabaseName";
            var containerName = "YourContainerName";

            // Act
            var container = _cosmosDbService.GetContainer(databaseName, containerName);

            // Assert
            Assert.IsNotNull(container);
            Assert.AreEqual(databaseName, container.Database.Id);
            Assert.AreEqual(containerName, container.Id);
        }

        // Add more test methods as needed
    }
}
