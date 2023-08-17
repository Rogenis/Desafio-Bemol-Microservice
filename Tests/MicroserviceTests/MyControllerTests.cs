using Microservice.Controllers;
using Microservice.DTOs;
using Microservice.Models;
using Microservice.Repositories;
using Microservice.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Microservice.Tests
{
    [TestFixture]
    public class MyControllerTests
    {
        private MyController _myController;
        private Mock<CosmosDbRepository> _cosmosRepositoryMock;
        private Mock<ServiceBusQueue> _serviceBusQueueMock;

        [SetUp]
        public void Setup()
        {
            _cosmosRepositoryMock = new Mock<CosmosDbRepository>();
            _serviceBusQueueMock = new Mock<ServiceBusQueue>();
            _myController = new MyController(_cosmosRepositoryMock.Object, _serviceBusQueueMock.Object);
        }

        [Test]
        public async Task CreateItem_DtoValido_RetornaResultadoOk()
        {
            // Arrange
            var meuDto = new MyDto
            {
                id = Guid.NewGuid().ToString(),
                Key = "ChaveTeste",
                AnotherKey = "OutraChaveTeste"
            };

            _cosmosRepositoryMock.Setup(repo => repo.CreateAsync(meuDto)).ReturnsAsync(meuDto);
            _serviceBusQueueMock.Setup(queue => queue.SendMessageAsync(meuDto)).Verifiable();

            // Act
            var resultado = await _myController.CreateItem(meuDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(200, resultado.StatusCode);
            Assert.AreEqual(meuDto, resultado.Value);
            _cosmosRepositoryMock.Verify(repo => repo.CreateAsync(meuDto), Times.Once);
            _serviceBusQueueMock.Verify(queue => queue.SendMessageAsync(meuDto), Times.Once);
        }

        [Test]
        public async Task CreateItem_DtoInvalido_RetornaInternalServerError()
        {
            // Arrange
            var dtoInvalido = new MeuDto(); // DTO inválido

            // Act
            var resultado = await _myController.CreateItem(dtoInvalido) as ObjectResult;

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(500, resultado.StatusCode);
            _cosmosRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<MeuDto>()), Times.Never);
            _serviceBusQueueMock.Verify(queue => queue.SendMessageAsync(It.IsAny<MeuDto>()), Times.Never);
        }

        [Test]
        public async Task CreateItem_ExcecaoLancada_RetornaInternalServerError()
        {
            // Arrange
            var meuDto = new MeuDto
            {
                id = Guid.NewGuid().ToString(),
                Key = "ChaveTeste",
                AnotherKey = "OutraChaveTeste"
            };

            _cosmosRepositoryMock.Setup(repo => repo.CreateAsync(meuDto)).Throws<Exception>();

            // Act
            var resultado = await _myController.CreateItem(meuDto) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(500, resultado.StatusCode);
            _cosmosRepositoryMock.Verify(repo => repo.CreateAsync(meuDto), Times.Once);
            _serviceBusQueueMock.Verify(queue => queue.SendMessageAsync(meuDto), Times.Never);
        }
    }
}
