using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.DataLayer.Contracts
{
    public interface IPrivilegeRepository : IRepository<Privilege>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(PrivilegeSearchCriteria criteria);
        Task<SearchResults<Privilege>> GetAsync(PrivilegeSearchCriteria criteria, string includeProperties = null);
    }
}
