using Microsoft.Azure.Cosmos;
using Microservice.Models;
using Microservice.Services;
using Microservice.DTOs;
using System;

namespace Microservice.Repositories
{
    public class CosmosDbRepository<TDto> where TDto : IDto, new()
    {
        private readonly Container _container;

        public CosmosDbRepository(CosmosDbService cosmosDbService, string databaseName, string containerName)
        {
            _container = cosmosDbService.GetContainer(databaseName, containerName);
        }

        public TDto Create(TDto dto)
        {
            var entity = MapDtoToEntity(dto);
            var response = _container.CreateItemAsync(entity).Result;
            var createdDto = MapEntityToDto(response.Resource);
            return createdDto;
        }

        private MyEntity MapDtoToEntity(TDto dto)
        {
            return new MyEntity
            {
                Id = Guid.NewGuid().ToString(),
                Key = dto.Key,
                AnotherKey = dto.AnotherKey
            };
        }

        private TDto MapEntityToDto(MyEntity entity)
        {
            return new TDto
            {
                Key = entity.Key,
                AnotherKey = entity.AnotherKey
            };
        }

        // Implementar outros métodos do repositório conforme necessário...
    }
}
