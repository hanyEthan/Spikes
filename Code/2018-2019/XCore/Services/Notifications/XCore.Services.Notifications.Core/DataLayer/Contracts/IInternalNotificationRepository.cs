using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.DataLayer.Contracts
{
    public interface IInternalNotificationRepository : IRepository<InternalNotification>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(InternalNotificationSearchCriteria criteria);
        Task<SearchResults<InternalNotification>> GetAsync(InternalNotificationSearchCriteria criteria, string includeProperties = null);
        Task SetIsDismissedAsync(InternalNotification item, bool isActive);
        Task SetIsDeletedAsync(InternalNotification item, bool isActive);
        Task SetIsReadAsync(List<InternalNotification> item, bool isActive);
        

    }
}
