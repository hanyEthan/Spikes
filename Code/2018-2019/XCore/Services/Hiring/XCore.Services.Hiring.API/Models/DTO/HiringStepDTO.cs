using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.API.Models.DTO
{
    public class HiringStepDTO : Entity<int>
    {
        #region props.
        public int HiringProcessId { get; set; } 
        public HiringProcessDTO HiringProcess { get; set; } 
        #endregion       
    }
}
