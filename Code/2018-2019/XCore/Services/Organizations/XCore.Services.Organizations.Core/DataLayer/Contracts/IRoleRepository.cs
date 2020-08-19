using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        bool? Initialized { get; }
        Task<SearchResults<Role>> GetAsync(RoleSearchCriteria criteria, string includeProperties = null);
        Task<bool> AnyAsync(RoleSearchCriteria criteria, string includeProperties = null);
    }
}
