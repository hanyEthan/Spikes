using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Personnels;

namespace XCore.Services.Personnel.DataLayer.Contracts.Personnels
{
    public interface IPersonnelRepository : IRepository<Person>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(PersonSearchCriteria criteria);
        Task<SearchResults<Person>> GetAsync(PersonSearchCriteria criteria, string includeProperties = null);
    }
}
