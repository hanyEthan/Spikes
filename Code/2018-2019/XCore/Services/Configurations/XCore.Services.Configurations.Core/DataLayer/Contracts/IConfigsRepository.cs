using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Support;

namespace XCore.Services.Configurations.Core.DataLayer.Contracts
{
    public interface IConfigsRepository : IRepository<ConfigItem>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(ConfigSearchCriteria criteria);
        Task<SearchResults<ConfigItem>> GetAsync(ConfigSearchCriteria criteria, string includeProperties = null);
    }
}
