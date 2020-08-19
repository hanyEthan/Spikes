using ADS.Tamam.Modules.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Models
{
    public class Excuse : ILoggable
    {
        public string Code { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PersonCode { get; set; }
        public string StatusCode { get; set; }
        public string TypeCode { get; set; }
        public string Notes { get; set; }
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
            return string.Format("Code [{0}] StartTime [{1}] EndTime [{2}] PersonCode [{3}] StatusCode [{4}] TypeCode [{5}] Notes [{6}] DateCreated [{7}] DateUpdated [{8}]",
                    Code,
                    StartTime,
                    EndTime,
                    PersonCode,
                    StatusCode,
                    TypeCode,
                    Notes,
                    DateCreated,
                    DateUpdated
                    );
        }


    }
}
