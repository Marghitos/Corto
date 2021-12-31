using Corto.API.Controllers.Adapters;
using Corto.BL.Adapters;
using Corto.BL.Models;
using Corto.BL.Services;
using Corto.Common.DataAccess;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Corto.API
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
            services.AddControllers();

            //START - SqlRepository 
            string sqlServerName = Configuration.GetSection("SqlServer:ServerName").Value;
            string sqlDatabaseName = Configuration.GetSection("SqlServer:DatabaseName").Value;
            string sqlUserName = Configuration.GetSection("SqlServer:Username").Value;
            string sqlPassword = Configuration.GetSection("SqlServer:Password").Value;
            string sqlPort = Configuration.GetSection("SqlServer:Port").Value;

            services.AddSingleton<ISqlRepository, SqlRepository>(s => new SqlRepository(sqlServerName, sqlDatabaseName, sqlUserName, sqlPassword, sqlPort));
            //END - SqlRepository

            //START - CosmosDbRepository
            string cosmosDbDatabaseName = Configuration.GetSection("CosmosDb:DatabaseName").Value;
            string cosmosDbContainerName = Configuration.GetSection("CosmosDb:ContainerName").Value;
            string cosmosDbAccount = Configuration.GetSection("CosmosDb:Account").Value;
            string cosmosDbKey = Configuration.GetSection("CosmosDb:Key").Value;

            services.AddSingleton<ICosmosDbRepository<UrlItem>, CosmosDbRepository<UrlItem>>(c => new CosmosDbRepository<UrlItem>(cosmosDbDatabaseName, cosmosDbContainerName, cosmosDbAccount, cosmosDbKey));
            //END - CosmosDbRepository

            //START - UrlManagerService
            int expiryDaysCount = int.TryParse(Configuration.GetSection("Settings:ExpiryDaysCount").Value, out int result) ? result : 60;

            services.AddNamedSingleton<IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse>, CosmosDbResponseGetToUrlManagerServiceResponseAdapter>(
                "Get",
                c =>
                    new CosmosDbResponseGetToUrlManagerServiceResponseAdapter(expiryDaysCount)
                );
            services.AddNamedSingleton<IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse>, CosmosDbResponseInsertToUrlManagerServiceResponseAdapter>("Insert");

            services.AddSingleton<IUrlManagerService, UrlManagerService>(
                c =>
                    new UrlManagerService(c.GetService<ICosmosDbRepository<UrlItem>>(),
                                          c.GetNamedService<IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse>>("Get"),
                                          c.GetNamedService<IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse>>("Insert"))
                );
            //END - UrlManagerService

            //START - KeyRangeService
            services.AddSingleton<IAdapter<DataRow, KeyRangeServiceResponse>, DataRowToKeyRangeServiceResponseAdapter>();
            services.AddSingleton<IKeyRangeService, KeyRangeService>(
                k =>
                    new KeyRangeService(k.GetService<ISqlRepository>(), k.GetService<IAdapter<DataRow, KeyRangeServiceResponse>>())
                );
            //END - KeyRangeService

            //START - AlgorithmService
            services.AddSingleton<IAlgorithmService, AlgorithmService>();
            //END - AlgorithmService

            //START UrlShortenerController
            services.AddSingleton<IAdapter<UrlMangerServiceResponse, ApiResponse>, UrlManagerServiceResponseToApiResponseAdapter>();
            //END UrlShortenerController
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
