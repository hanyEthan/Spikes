using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Support;

namespace XCore.Services.Configurations.Core.DataLayer.Contracts
{
    public interface IModulesRepository : IRepository<Module>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(ModuleSearchCriteria criteria);
        Task<SearchResults<Module>> GetAsync(ModuleSearchCriteria criteria, string includeProperties = null);
    }
}
