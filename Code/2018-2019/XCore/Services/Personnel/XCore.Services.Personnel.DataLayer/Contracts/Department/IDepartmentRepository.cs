using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Departments;

namespace XCore.Services.Personnel.DataLayer.Contracts.Departments
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(DepartmentSearchCriteria criteria);
        Task<SearchResults<Department>> GetAsync(DepartmentSearchCriteria criteria, string includeProperties = null);
    }
}
