using System;

namespace XCore.Services.Notifications.SDK.Models.DTOs
{
    public class InternalNotificationDTO
    {
        #region prop.
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }

        public virtual bool IsRead { get; set; }
        public virtual bool IsDismissed { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime DateRead { get; set; }
        public virtual DateTime DateDismissed { get; set; }
        public virtual string ActorId { get; set; }
        public virtual string ActionId { get; set; }
        public virtual string TargetId { get; set; }
        public virtual string Content { get; set; }
        #endregion
    }
}
