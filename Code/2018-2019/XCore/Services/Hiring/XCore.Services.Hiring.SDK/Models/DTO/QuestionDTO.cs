
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.SDK.Models.DTO
{
    public class QuestionDTO : Entity<int>
    {
        #region props
        public  int AdvertisementId { get; set; }
        public AdvertisementDTO Advertisement { get; set; }

        #endregion       
    }
}
