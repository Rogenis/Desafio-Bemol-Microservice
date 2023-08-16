using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;
using Microservice.Services;
using Microservice.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microservice.Filters;

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
            // Configurar o filtro de autorização da chave de API
            services.AddScoped<ApiKeyAuthorizationFilter>();

            // Registra o filtro de autorização globalmente
            services.AddMvc(options =>
            {
                options.Filters.Add<ApiKeyAuthorizationFilter>();
            });

            // Registra o filtro ApiExceptionFilter como um filtro global
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
            });

            services.AddScoped<LogActionFilter>(); // Registra o filtro de log de ação

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
