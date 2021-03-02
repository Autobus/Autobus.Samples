using System;
using System.Threading.Tasks;
using Autobus;
using SampleServiceInterface;

namespace SampleService
{
    public class SampleService : ISampleService
    {
        private readonly IAutobus _autobus;

        public SampleService(IAutobus autobus)
        {
            _autobus = autobus;
        }
        
        // These can be async, each request runs in its own task
        public Task<PingResponse> Ping(PingRequest request)
        {
            // Fire off the ping event letting everyone know we got pinged.
            // Not sure why you'd ever do this, just an example
            _autobus.Publish(new PingEvent(request.Value));
            return Task.FromResult(new PingResponse(request.Value));
        }

        // Commands can be async too
        public Task Print(PrintCommand command)
        {
            Console.WriteLine($"Service got print: {command.Message}");
            return Task.CompletedTask;
        }
    }
}