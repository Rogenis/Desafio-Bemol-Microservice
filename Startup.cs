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
            services.AddScoped<CosmosDbService>();
            // Configuração do Service Bus
            services.AddScoped<ServiceBusQueue>();

            // Configuração do Cosmos DB
            services.AddSingleton<CosmosClient>(InitializeCosmosClient);
            services.AddScoped(typeof(CosmosDbRepository<>));

            services.AddControllers();
        }

        private CosmosClient InitializeCosmosClient(IServiceProvider serviceProvider)
        {
            string connectionString = Configuration.GetConnectionString("CosmosDBConnection");
            return new CosmosClient(connectionString);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

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
