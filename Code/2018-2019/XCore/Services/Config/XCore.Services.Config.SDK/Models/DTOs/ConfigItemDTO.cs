using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.SDK.Models.DTOs
{
    public class ConfigItemDTO
    {
        #region props.

        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string MetaData { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool ReadOnly { get; set; }
        public string Version { get; set; }

        public int ModuleId { get; set; }
        public ModuleDTO Module { get; set; }

        public int AppId { get; set; }
        public AppDTO App { get; set; }

        #endregion
    }
}

