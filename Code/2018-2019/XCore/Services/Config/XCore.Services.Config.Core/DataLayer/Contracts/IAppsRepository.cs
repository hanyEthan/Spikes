using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.Core.DataLayer.Contracts
{
    public interface IAppsRepository : IRepository<App>
    {
        bool Any(AppSearchCriteria criteria);
        SearchResults<App> Get(AppSearchCriteria criteria, string includeProperties = null);
    }
}
