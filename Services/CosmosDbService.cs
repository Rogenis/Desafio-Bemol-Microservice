using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;

namespace Microservice.Services
{
    public class CosmosDbService
    {
        private readonly CosmosClient _cosmosClient;

        public CosmosDbService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CosmosDBConnection");
            _cosmosClient = new CosmosClient(connectionString);
        }

        public Container GetContainer(string databaseName, string containerName)
        {
            var database = _cosmosClient.GetDatabase(databaseName);
            return database.GetContainer(containerName);
        }
    }
}
