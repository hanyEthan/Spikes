using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.DataLayer.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(RoleSearchCriteria criteria);
        Task<SearchResults<Role>> GetAsync(RoleSearchCriteria criteria, string includeProperties = null);
        void DeletedAssociatedPrivilege(RolePrivilege model);
        void DeletedAssociatedActor(ActorRole model);

    }
}
