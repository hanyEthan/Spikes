using ADS.Common.Models.Domain.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsEmailSenderDataHandler : IBaseHandler
    {
        bool Save( EmailMessage message );
        bool Delete( Guid id );
        List<EmailMessage> GetAll();
    }
}