using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Position : Entity<int>
    {
        #region props.
        public virtual int AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }

        #endregion      
    }
}
