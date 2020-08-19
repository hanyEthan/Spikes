using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async.Models;

namespace Mcs.Invoicing.Services.Config.Client.Sdk.Contracts
{
    public interface IConfigServiceClient
    {
        bool Initialized { get; }

        Task CreateAsync(ConfigCreateCommandMessage request);
        Task<CreateConfigItemResponseProto> CreateProto(ConfigCreateCommandMessage request, string jwtToken = null);
        Task<bool?> IsHealthy(string jwtToken = null);
    }
}
