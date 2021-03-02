using System.Threading.Tasks;
using Autobus;

namespace SampleServiceInterface
{
    public record PrintCommand(string Message);

    public record PingRequest(int Value);

    public record PingResponse(int Value);

    public record PingEvent(int Value);
    
    // Interfaces are used to define what requests and commands the service can handle
    public interface ISampleService
    {
        Task<PingResponse> Ping(PingRequest request);
        Task Print(PrintCommand command);
    }

    // This contract should get picked up when calling UseServicesFromAllAssemblies on the AutobusBuilder
    // However, there is a way you can manually add them too
    public class SampleServiceContract : BaseServiceContract
    {
        // Here we actually need to build a contract for the service. Events have to be manually included here
        public override void Build(IServiceContractBuilder builder) =>
            builder
                .UseName("SampleService")
                .AddInterface<ISampleService>()
                .AddEvent<PingEvent>();
    }
}