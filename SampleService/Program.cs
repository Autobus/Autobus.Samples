using System;
using Autobus;
using Microsoft.Extensions.Hosting;
using SampleServiceInterface;

namespace SampleService
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
                    // Let the DI know we want to run the SampleService.
                    // Each request and command gets its own scope
                    services.AddServiceKernel<ISampleService, SampleService>());
    }
}