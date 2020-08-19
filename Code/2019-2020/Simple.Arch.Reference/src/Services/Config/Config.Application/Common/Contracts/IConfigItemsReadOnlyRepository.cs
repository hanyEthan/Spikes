using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;

namespace Mcs.Invoicing.Services.Config.Application.Common.Contracts
{
    public interface IConfigItemsReadOnlyRepository : IRepositoryRead<ConfigItem>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(ConfigItemsSearchCriteria criteria);
        Task<SearchResults<ConfigItem>> GetAsync(ConfigItemsSearchCriteria criteria, string includeProperties = null);
        Task<ConfigItem> GetAsync(int moduleId, string key, string includeProperties = null);
    }
}
