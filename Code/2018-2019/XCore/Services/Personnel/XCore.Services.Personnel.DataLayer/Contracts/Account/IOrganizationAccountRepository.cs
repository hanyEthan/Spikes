using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.DataLayer.Contracts.Accounts
{
    public interface IOrganizationAccountRepository : IRepository<OrganizationAccount>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(OrganizationAccountSearchCriteria criteria);
        Task<SearchResults<OrganizationAccount>> GetAsync(OrganizationAccountSearchCriteria criteria, string includeProperties = null);
    }
}
