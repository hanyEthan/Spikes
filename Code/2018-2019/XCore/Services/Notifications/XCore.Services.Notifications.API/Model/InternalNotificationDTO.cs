using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCore.Services.Notifications.API.Model
{
    public class InternalNotificationDTO
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
       // public virtual string Name { get; set; }
       // public virtual string NameCultured { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }

        public virtual bool IsRead { get; set; } = false;
        public virtual bool IsDismissed { get; set; } = false;
        public virtual bool IsDeleted { get; set; } = false;
        public  virtual DateTime DateRead { get; set; }
        public  virtual DateTime DateDismissed { get; set; }
        public  virtual string ActorId { get; set; }
        public  virtual string ActionId { get; set; }
        public  virtual string TargetId { get; set; }
        public virtual string Content { get; set; }
    }          
}
