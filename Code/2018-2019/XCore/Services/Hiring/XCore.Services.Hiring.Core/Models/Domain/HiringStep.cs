using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class HiringStep : Entity<int>
    {
        #region props.
        public virtual int HiringProcessId { get; set; } 
        public virtual HiringProcess HiringProcess { get; set; } 
        #endregion       
    }
}
