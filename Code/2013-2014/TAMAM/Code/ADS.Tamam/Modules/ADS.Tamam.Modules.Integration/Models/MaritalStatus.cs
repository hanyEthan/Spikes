using System;
using ADS.Tamam.Modules.Integration.Helpers;

namespace ADS.Tamam.Modules.Integration.Models
{
    public class MaritalStatus : IDetailCodeSimilar
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameVariant { get; set; }
        public bool Activated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool isSynced { get; set; }

        public string Reference
        {
            get { return this.Code; }
        }

        public string GetLoggingData()
        {
            var logs = string.Format( "Code [{0}] Name [{1}] NameVariant [{2}] Activated [{3}] DateCreated [{4}] DateUpdated [{5}]", this.Code, this.Name, this.NameVariant, this.Activated, this.DateCreated.ToString( IntegrationConstants.DateFormat ), this.DateUpdated.ToString( IntegrationConstants.DateFormat ) );
            return logs;
        }
    }
}