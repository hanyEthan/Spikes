using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class ApplicationHistory : Entity<int>
    {
        #region props

        public int ActionId { get; set; }
        public int ModelId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int StatusOld { get; set; }
        public int StatusNew { get; set; }
        public ApplicationHistoryMode Mode { get; set; }
       

        #endregion
       
    }

    
}
