using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(DepartmentSearchCriteria criteria);
        Task<SearchResults<Department>> GetAsync(DepartmentSearchCriteria criteria, string includeProperties = null);
    }
}
