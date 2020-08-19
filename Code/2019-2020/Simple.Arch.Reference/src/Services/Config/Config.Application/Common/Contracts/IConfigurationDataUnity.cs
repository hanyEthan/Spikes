using System.Threading;
using System.Threading.Tasks;

namespace Mcs.Invoicing.Services.Config.Application.Common.Contracts
{
    public interface IConfigurationDataUnity
    {
        bool? Initialized { get; }

        IModulesRepository Modules { get; }
        IConfigItemsRepository ConfigItems { get; }

        IModulesReadOnlyRepository ModulesReadOnly { get; }
        IConfigItemsReadOnlyRepository ConfigItemsReadOnly { get; }

        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
