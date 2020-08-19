using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.Core.DataLayer.Contracts
{
    public interface IConfigsRepository : IRepository<ConfigItem>
    {
        bool Any(ConfigSearchCriteria criteria);
        SearchResults<ConfigItem> Get(ConfigSearchCriteria criteria, string includeProperties = null);
    }
}
