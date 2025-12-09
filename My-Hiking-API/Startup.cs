using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyHikingAPI.Services;

[assembly: FunctionsStartup(typeof(MyHikingAPI.Startup))]

namespace MyHikingAPI;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
         
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IMountainService, MountainService>();
        

        //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
    }
}
