using System.Threading.Tasks;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages;

namespace Mcs.Invoicing.Services.Audit.Client.Sdk.Contracts
{
    public interface IAuditClient
    {
        bool Initialized { get; }
        Task CreateAsync(IAuditMessage request);
    }
}
