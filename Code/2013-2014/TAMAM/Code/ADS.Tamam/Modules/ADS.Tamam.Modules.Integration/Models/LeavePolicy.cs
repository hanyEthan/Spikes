using ADS.Tamam.Modules.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Models
{
    public class LeavePolicy : ILoggable
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string LeaveTypeCode { get; set; }
        public int AllowedAmount { get; set; }
        public bool AllowRequests { get; set; }
        public int DaysBeforeRequest { get; set; }
        public bool CarryOver { get; set; }
        public int MaxCarryOverDays { get; set; }
        public bool AllowHalfDays { get; set; }
        public bool RequireAttachements { get; set; }
        public bool IncludeWeekEndsandHolidays { get; set; }
        public int DaysLimitForOldLeaves { get; set; }
        public int MaxDaysPerRequest { get; set; }
        public bool EssentialCredit { get; set; }
        public bool DisablePlannedLeaves { get; set; }
        public bool UnlimitedCredit { get; set; }
        public bool ExceedsProgressiveCredit { get; set; }       
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
            return string.Format("Code [{0}] Name [{1}] NameCultureVarient [{2}] LeaveTypeCode [{3}] AllowedAmount [{4}] AllowRequests [{5}] DaysBeforeRequest [{6}]" +
            "CarryOver [{7}] MaxCarryOverDays [{8}] AllowHalfDays [{9}] RequireAttachements [{10}] IncludeWeekEndsandHolidays [{11}] DaysLimitForOldLeaves [{12}] MaxDaysPerRequest [{13}]" +
            "EssentialCredit [{14}] DisablePlannedLeaves [{15}] UnlimitedCredit [{16}] ExceedsProgressiveCredit [{17}]",
                 Code,
                 Name,
                 NameCultureVarient,
                 LeaveTypeCode,
                 AllowedAmount,
                 AllowRequests,
                 DaysBeforeRequest,
                 CarryOver,
                 MaxCarryOverDays,
                 AllowHalfDays,
                 RequireAttachements,
                 IncludeWeekEndsandHolidays,
                 DaysLimitForOldLeaves,
                 MaxDaysPerRequest,
                 EssentialCredit,
                 DisablePlannedLeaves,
                 UnlimitedCredit,
                 ExceedsProgressiveCredit);
        }
    }
}
