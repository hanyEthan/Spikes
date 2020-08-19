using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Configurations.SDK.Models.DTOs
{
    public class AppDTO : Entity<int>
    {
        #region props.

        public string Description { get; set; }

        #endregion
    }
}
