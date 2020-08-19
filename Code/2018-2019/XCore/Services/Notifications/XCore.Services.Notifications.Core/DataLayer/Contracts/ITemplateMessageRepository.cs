using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.DataLayer.Contracts
{
    public interface IMessageTemplateRepository : IRepository<MessageTemplate>
    {
            bool? Initialized { get; }
            Task<bool> AnyAsync(MessageTemplateSearchCriteria criteria);
            Task<SearchResults<MessageTemplate>> GetAsync(MessageTemplateSearchCriteria criteria, string includeProperties = null);
    }
}
