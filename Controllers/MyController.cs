using Microsoft.AspNetCore.Mvc;
using Microservice.DTOs;
using Microservice.Filters;
using Microservice.Repositories;
using Microservice.Services;
using System;
using System.Threading.Tasks;

namespace Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyController : ControllerBase
    {
        private readonly CosmosDbRepository _cosmosRepository;
        private readonly ServiceBusQueue _serviceBusQueue;

        public MyController(CosmosDbRepository cosmosRepository, ServiceBusQueue serviceBusQueue)
        {
            _cosmosRepository = cosmosRepository;
            _serviceBusQueue = serviceBusQueue;
        }

        [HttpPost]
        [TypeFilter(typeof(ApiKeyAuthorizationFilter))] // Aplica filtro de autorização da chave de API
        [TypeFilter(typeof(LogActionFilter))] // Aplica filtro de log de ação a todas as ações
        public async Task<IActionResult> CreateItem(MyDto myDto)
        {
            try
            {
                // Persistir no Azure Cosmos DB
                var createdItem = await _cosmosRepository.CreateAsync(myDto);
                Console.WriteLine($"Created item: {createdItem}");

                // Envia mensagem para a fila do Azure Service Bus
                await _serviceBusQueue.SendMessageAsync(myDto);

                return Ok(createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public Task GetAllItems()
        {
            throw new NotImplementedException();
        }

        public Task GetItemById(string id)
        {
            throw new NotImplementedException();
        }
         public Task UpdateItem(string id)
        {
            throw new NotImplementedException();
        }
         public Task DeleteItem(string id)
        {
            throw new NotImplementedException();
        }
        
        // Testa o filtro de exceção
        // [HttpGet("test-exception")]
        // public IActionResult TestException()
        //{
           // throw new Exception("Simulating an exception");
        //}
    }
}
