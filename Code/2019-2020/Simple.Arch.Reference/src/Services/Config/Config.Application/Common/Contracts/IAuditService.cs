using System.Threading.Tasks;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages;

namespace Mcs.Invoicing.Services.Config.Application.Common.Contracts
{
    public interface IAuditService
    {
        bool? Initialized { get; }
        Task CreateAsync(IAuditMessage request);
    }
}
