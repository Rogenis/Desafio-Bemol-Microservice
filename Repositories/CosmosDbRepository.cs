using Microsoft.Azure.Cosmos;
using Microservice.Models;
using Microservice.Services;
using Microservice.DTOs;
using System;
using System.Threading.Tasks;

namespace Microservice.Repositories
{
    public class CosmosDbRepository
    {
        private readonly Container _container;

        public CosmosDbRepository(CosmosDbService cosmosDbService, string databaseName, string containerName)
        {
            _container = cosmosDbService.GetContainer(databaseName, containerName);
        }

    public async Task<MyDto> CreateAsync(MyDto dto)
    {
        // Mapear o DTO para a entidade
        var entity = MapDtoToEntity(dto);

        try
        {
            // Criar o item no Cosmos DB
            var response = await _container.CreateItemAsync(entity);

            // Mapear a resposta para um DTO
            var createdDto = MapEntityToDto(response.Resource);

            return createdDto;
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error creating item: {ex}");
            throw; // Re-throw the exception to be handled at the controller level
        }
    }

    private MyEntity MapDtoToEntity(MyDto dto)
    {
        return new MyEntity
        {
            id = dto.id,
            Key = dto.Key,
            AnotherKey = dto.AnotherKey
        };
    }

    private MyDto MapEntityToDto(MyEntity entity)
    {
        return new MyDto
        {
            id = entity.id,
            Key = entity.Key,
            AnotherKey = entity.AnotherKey
        };
    }

    }
}
