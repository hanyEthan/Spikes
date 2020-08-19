using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(OrganizationSearchCriteria criteria);
        Task<SearchResults<Organization>> GetAsync(OrganizationSearchCriteria criteria, string includeProperties = null);
    }
}
