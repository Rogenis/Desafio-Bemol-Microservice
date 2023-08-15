using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;
using Microservice.Services;
using Microservice.Repositories;
using System;

namespace Microservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configurações de conexão para o Cosmos DB e Service Bus
            services.Configure<ServiceBusQueueSettings>(Configuration.GetSection("ServiceBusQueueSettings"));

            // Serviços
            services.AddScoped<CosmosDbService>();
            services.AddScoped<ServiceBusQueue>();

            // Registro do CosmosClient
            services.AddSingleton(sp => InitializeCosmosClient(sp));

            // Repositório genérico para o Cosmos DB
            services.AddScoped<CosmosDbRepository>(provider =>
            {
                var cosmosDbService = provider.GetRequiredService<CosmosDbService>();
                var configuration = provider.GetRequiredService<IConfiguration>();

                string databaseName = "MicroserviceDB"; // Nome do seu banco de dados
                string containerName = "Items"; // Nome do seu container

                return new CosmosDbRepository(cosmosDbService, databaseName, containerName);
            });

            // Controllers
            services.AddControllers();
        }

        private CosmosClient InitializeCosmosClient(IServiceProvider serviceProvider)
        {
            string connectionString = Configuration.GetConnectionString("CosmosDBConnection");
            return new CosmosClient(connectionString);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configurações de ambiente e middleware

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
