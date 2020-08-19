using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.SDK.Models.DTOs
{
    public class AppDTO : Entity<int>
    {
        #region props.

        public string Description { get; set; }

        #endregion
    }
}
