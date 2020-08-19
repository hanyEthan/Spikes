using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.DataLayer.Contracts
{
    public interface IAppsRepository : IRepository<App>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(AppSearchCriteria criteria);
        Task<SearchResults<App>> GetAsync(AppSearchCriteria criteria, string includeProperties = null);
    }
}
