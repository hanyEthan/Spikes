using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Domain.Events;

namespace Mcs.Invoicing.Services.Config.Application.Common.Contracts
{
    public interface IConfigEventsPublisher
    {
        bool Initialized { get; }
        Task PublishEvent(ConfigCreatedEvent @event);
    }
}
