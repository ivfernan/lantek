using lantek.client;
using lantek.model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace lantek
{
    class Program
    {
                
        static async Task Main(string[] args)
        {
            
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json", false)
            .Build();
           
            var services = new ServiceCollection();

            services.AddHttpClient<CuttingMachineClient>(); 
            
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<ICuttingMachineClient,CuttingMachineClient>();
            
            var serviceProvider = services.BuildServiceProvider();
            var machineServiceClient = serviceProvider.GetRequiredService<ICuttingMachineClient>();

            await machineServiceClient.getCuttingMachines();

        }
        
    }
}

