using System;
using Telerik.OpenAccess.Metadata.Fluent;

namespace ADS.Common.Models.Domain
{
    /// <summary>
    /// a key-value pair, representing a single configuration entry
    /// </summary>
    public class ConfigurationItem
    {
        public Guid Id { get; set; }
        public string ApplicationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }

        #region ctr.

        public ConfigurationItem()
        {
        }
        public ConfigurationItem( Guid id , string applicationId , string key , string value , bool active ) : this()
        {
            Id = id;
            ApplicationId = applicationId;
            Key = key;
            Value = value;
            Active = active;
        }
        
        #endregion
    }
}
