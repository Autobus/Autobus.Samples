using Autobus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleServiceInterface;

namespace SampleClient
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutobus(builder =>
                {
                    builder
                        .UseBinaryRecordsSerialization()
                        .UseRabbitMQTransport(builder =>
                            builder.ConfigureConnectionFactory(factory =>
                            {
                                factory.HostName = "localhost";
                                factory.VirtualHost = "/";
                                factory.UserName = "guest";
                                factory.Password = "guest";
                            }))
                        .UseServicesFromAllAssemblies();
                })
                .ConfigureServices(services =>
                    services
                        // Let the DI know we want to use ISampleService as a service client
                        // This lets us communicate directly with a service through the interface
                        .AddServiceClient<ISampleService>()
                        .AddHostedService<SampleClient>());
    }
}