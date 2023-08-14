using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Microservice.Services
{
    public class CosmosDbService
    {
        private readonly string _cosmosDbConnectionString;
        private CosmosClient _cosmosClient;

        public CosmosDbService(IConfiguration configuration)
        {
            _cosmosDbConnectionString = configuration.GetConnectionString("CosmosDBConnection");
            _cosmosClient = new CosmosClient(_cosmosDbConnectionString);
        }

        public Container GetContainer(string databaseName, string containerName)
        {
            var database = _cosmosClient.GetDatabase(databaseName);
            return database.GetContainer(containerName);
        }
    }
}
