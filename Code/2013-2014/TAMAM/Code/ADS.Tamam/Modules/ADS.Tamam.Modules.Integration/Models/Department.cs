using ADS.Tamam.Modules.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Models
{
    public class Department : ILoggable
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameVariant { get; set; }
        public string ParentCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool isSynced { get; set; }

        public string GetLoggingData()
        {
            return string.Format( "Code [{0}] Name [{1}] NameVariant [{2}] ParentCode [{3}] DateCreated [{4}] DateUpdated [{5}]", this.Code, this.Name, this.NameVariant, this.ParentCode, this.DateCreated.ToString(IntegrationConstants.DateFormat), this.DateUpdated.ToString(IntegrationConstants.DateFormat));
        }

        public string Reference
        {
            get
            {
                return this.Code.ToString();
            }
        }

    }
}
