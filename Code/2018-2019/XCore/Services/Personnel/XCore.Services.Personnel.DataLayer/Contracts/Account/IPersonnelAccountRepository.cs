using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.DataLayer.Contracts.Accounts
{
    public interface IPersonnelAccountRepository : IRepository<PersonnelAccount>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(PersonnelAccountSearchCriteria criteria);
        Task<SearchResults<PersonnelAccount>> GetAsync(PersonnelAccountSearchCriteria criteria, string includeProperties = null);
    }
}
