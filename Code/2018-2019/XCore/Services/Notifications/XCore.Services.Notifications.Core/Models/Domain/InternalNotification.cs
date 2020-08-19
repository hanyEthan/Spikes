using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Notifications.Core.Models.Domain
{
   public  class InternalNotification : Entity<int>
    {

        public bool IsRead { get; set; }
        public bool IsDismissed { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime DateRead { get; set; }
        public DateTime DateDismissed { get; set; }
        public string ActorId { get; set; }
        public string ActionId { get; set; }
        public string TargetId { get; set; }
        public string Content { get; set; }


    }
}
