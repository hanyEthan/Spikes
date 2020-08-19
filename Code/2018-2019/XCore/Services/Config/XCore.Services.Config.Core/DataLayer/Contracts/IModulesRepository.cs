using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.Core.DataLayer.Contracts
{
    public interface IModulesRepository : IRepository<Module>
    {
        bool Any(ModuleSearchCriteria criteria);
        SearchResults<Module> Get(ModuleSearchCriteria criteria, string includeProperties = null);
    }
}
