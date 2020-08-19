using ADS.Tamam.Modules.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Models
{
    public class Delegate :ILoggable
    {
        public string Code { get; set; }
        public string PersonCode{ get; set; }
        public string DelegateCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool isSynced { get; set; }

        public string Reference
        {
            get 
            { 
                return Code; 
            }
        }

        public string GetLoggingData()
        {
            return string.Format("Code [{0}] PersonCode [{1}] DelegateCode [{2}] StartDate [{3}] EndDate [{4}] DateCreated [{5}] DateUpdated [{6}]",
                 Code,
                 PersonCode,
                 DelegateCode,
                 StartDate,
                 EndDate,
                 DateCreated,
                 DateUpdated );
        }
    }
}
