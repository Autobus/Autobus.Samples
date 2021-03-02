using System;
using System.Threading;
using System.Threading.Tasks;
using Autobus;
using Microsoft.Extensions.Hosting;
using SampleServiceInterface;

namespace SampleClient
{
    public class SampleClient : IHostedService
    {
        private readonly IAutobus _autobus;
        private readonly ISampleService _sampleService;

        // You can easily access your services through DI
        public SampleClient(IAutobus autobus, ISampleService sampleService)
        {
            _autobus = autobus;
            _sampleService = sampleService;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // You can subscribe to events as follows
            _autobus.Subscribe<PingEvent>(OnPingEvent);
            
            // Print on our sample service and send out a ping
            await _sampleService.Print(new("Print from our sample client!"));
            var pingResponse = await _sampleService.Ping(new(12));
            Console.WriteLine($"Got our ping response: {pingResponse}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _autobus.Unsubscribe<PingEvent>(OnPingEvent);
            return Task.CompletedTask;
        }

        // Events can be marked as async, they run in their own task when they are received 
        private Task OnPingEvent(PingEvent @event)
        {
            Console.WriteLine($"Got ping event: {@event.Value}");
            return Task.CompletedTask;
        }
    }
}