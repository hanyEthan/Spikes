using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Configurations.SDK.Models.DTOs
{
    public class ModuleDTO : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public int AppId { get; set; }
        public AppDTO App { get; set; }

        #endregion
    }
}
