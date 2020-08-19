
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Question : Entity<int>
    {
        #region props
        public virtual int? AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }

        #endregion       
    }
}
