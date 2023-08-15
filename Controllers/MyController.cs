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
        [TypeFilter(typeof(ApiKeyAuthorizationFilter))] // Aplicar filtro de autorização da chave de API
        public async Task<IActionResult> CreateItem(MyDto myDto)
        {
            try
            {
                // Persistir no Azure Cosmos DB
                var createdItem = await _cosmosRepository.CreateAsync(myDto);
                Console.WriteLine($"Created item: {createdItem}");

                // Enviar mensagem para a fila do Azure Service Bus
                await _serviceBusQueue.SendMessageAsync(myDto);

                return Ok(createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // ... Implementar outros métodos CRUD e integração com Cosmos DB e Service Bus ...
    }
}
