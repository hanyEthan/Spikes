using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.DataLayer.Contracts
{
    public interface IApplicationRepository : IRepository<Application>
    {
        bool? Initialized { get; }
        Task<SearchResults<Application>> Get(ApplicationsSearchCriteria criteria, string includeProperties = null);
        Task<bool> AnyAsync(ApplicationsSearchCriteria criteria);

    }
}