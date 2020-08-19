using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface IEventRepository : IRepository<Event>
    {
        bool? Initialized { get; }
        Task<SearchResults<Event>> GetAsync(EventSearchCriteria criteria, string includeProperties = null);
        Task<bool> AnyAsync(EventSearchCriteria criteria, string includeProperties = null);
    }
}
