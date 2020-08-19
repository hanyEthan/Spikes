using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts;

namespace Mcs.Invoicing.Services.Config.Application.Common.Contracts
{
    public interface IConfigItemsRepository : IRepository<ConfigItem>
    {
        bool? Initialized { get; }
    }
}
