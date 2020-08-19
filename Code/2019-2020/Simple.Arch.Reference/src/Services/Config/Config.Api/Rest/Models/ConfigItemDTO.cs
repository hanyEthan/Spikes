using System;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Models
{
    public class ConfigItemDTO
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreationDateTimeUtc { get; set; }
        public DateTime? LastModificationDateTimeUtc { get; set; }
        public string CreatedByUserId { get; set; }
        public string LastModifiedByUserId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }
}
