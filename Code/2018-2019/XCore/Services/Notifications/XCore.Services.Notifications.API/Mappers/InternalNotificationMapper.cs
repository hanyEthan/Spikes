using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.API.Mappers
{
    public class InternalNotificationMapper : IModelMapper<InternalNotificationDTO, InternalNotification>,
                                         IModelMapper<InternalNotification, InternalNotificationDTO>
    {

        public static InternalNotificationMapper Instance { get; } = new InternalNotificationMapper();

        public InternalNotificationDTO Map(InternalNotification from, object metadata = null)
        {
            if (from == null) return null;

            var to = new InternalNotificationDTO();
            to.Id = from.Id;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);
            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);

            to.IsDeleted = from.IsDeleted;
            to.IsDismissed = from.IsDismissed;
            to.IsRead = from.IsRead;
            to.DateRead = from.DateRead;
            to.DateDismissed = from.DateDismissed;
            to.ActorId = from.ActorId;
            to.ActionId = from.ActionId;
            to.TargetId = from.TargetId;
            to.Content = from.Content;

            return to;



        }

        public InternalNotification Map(InternalNotificationDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new InternalNotification();
            to.Id = from.Id;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);
            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);

            to.IsDeleted = from.IsDeleted;
            to.IsDismissed = from.IsDismissed;
            to.IsRead = from.IsRead;
            to.DateRead = from.DateRead;
            to.DateDismissed = from.DateDismissed;
            to.ActorId = from.ActorId;
            to.ActionId = from.ActionId;
            to.TargetId = from.TargetId;
            to.Content = from.Content;

            return to;
        }
    }
}
