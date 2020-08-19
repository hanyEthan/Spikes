using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface IOrganizationDelegationRepository : IRepository<OrganizationDelegation>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(OrganizationDelegationSearchCriteria criteria);
        Task<SearchResults<OrganizationDelegation>> GetAsync(OrganizationDelegationSearchCriteria criteria, string includeProperties = null);
    }
}
