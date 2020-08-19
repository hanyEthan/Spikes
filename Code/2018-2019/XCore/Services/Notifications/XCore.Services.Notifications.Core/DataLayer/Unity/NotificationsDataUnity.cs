using System;
using System.Threading.Tasks;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.DataLayer.Context;
using XCore.Services.Notifications.Core.DataLayer.Contracts;

namespace XCore.Services.Notifications.Core.DataLayer.Unity
{
    class NotificationsDataUnity: INotificationsDataUnity
    {
        #region cst.

        public NotificationsDataUnity(NotificationsDataContext dataContext, IMessageTemplateRepository MessageTemplate, IInternalNotificationRepository InternalNotification)
        {
            this._DataContext = dataContext;
            this.MessageTemplate = MessageTemplate;
            this.InternalNotification = InternalNotification;
            this.Initialized = Initialize();
        }

        #endregion
        #region props

        public bool? Initialized { get; protected set; }

        private readonly NotificationsDataContext _DataContext;
        public IMessageTemplateRepository MessageTemplate { get; protected set; }
        public IInternalNotificationRepository InternalNotification { get; protected set; }

        #endregion
        #region publics

        public async Task SaveAsync()
        {
            try
            {
                await _DataContext.SaveChangesAsync();
            }
            catch (Exception x)
            {
                throw new Exception(null,x);
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.MessageTemplate?.Initialized.GetValueOrDefault() ?? false);
            isValid = isValid && (this.InternalNotification?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
