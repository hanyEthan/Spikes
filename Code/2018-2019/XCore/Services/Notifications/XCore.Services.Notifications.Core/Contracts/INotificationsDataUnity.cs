using System.Threading.Tasks;
using XCore.Services.Notifications.Core.DataLayer.Contracts;

namespace XCore.Services.Notifications.Core.Contracts
{
    public interface INotificationsDataUnity
    {
        bool? Initialized { get; }
        IMessageTemplateRepository MessageTemplate { get; }
        IInternalNotificationRepository InternalNotification { get; }
        Task SaveAsync();
    }
}
