using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.DataLayer.Contracts.Organizations
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(OrganizationSearchCriteria criteria);
        Task<SearchResults<Organization>> GetAsync(OrganizationSearchCriteria criteria, string includeProperties = null);
    }
}
