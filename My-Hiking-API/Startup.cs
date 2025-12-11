using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyHikingAPI.Services;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(MyHikingAPI.Startup))] // Registers this class as the functions startup for DI config

namespace MyHikingAPI;

// Startup class where services for DI are registered 
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
         
        //builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IMountainService, MountainService>(); // Registers domain service so it can be injected 
        

        //builder.Services.AddSingleton<ILoggerProvider, MyHikingAPI(); // Registers a custom logger 
    }
}
