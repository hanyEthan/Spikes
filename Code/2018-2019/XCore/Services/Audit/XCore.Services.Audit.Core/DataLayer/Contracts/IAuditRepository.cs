using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.Core.DataLayer.Contracts
{
    public interface IAuditRepository : IRepository<AuditTrail>
    {
        bool? Initialized { get; }
        Task<SearchResults<AuditTrail>> GetAsync(AuditSearchCriteria criteria);
    }
}
