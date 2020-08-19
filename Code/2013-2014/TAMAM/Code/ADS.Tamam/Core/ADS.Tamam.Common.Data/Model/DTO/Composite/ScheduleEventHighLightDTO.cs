using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.DTO.Composite
{
    [Serializable]
    public class ScheduleEventHighLightDTO : IXSerializable
    {
        public Guid SEId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? ActualIn { get; set; }
        public DateTime? ActualOut { get; set; }
        public Guid TotalStatusId { get; set; }
        public Guid PayCodeStatusId { get; set; }
        public Guid HoursStatusId { get; set; }
        public Guid PersonId { get; set; }
        public string PersonName { get; set; }
        public string PersonNameCultureVariant { get; set; }
        public string ShiftName { get; set; }
        public string ShiftNameCultureVarient { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameCultureVariant { get; set; }
        public Guid StatusInId { get; set; }
        public Guid StatusOutId { get; set; }
        public string TotalStatusName { get; set; }
        public string TotalStatusNameCultureVarient { get; set; }
        public string PayCodeStatusName { get; set; }
        public string PayCodeStatusNameCultureVarient { get; set; }
        public string HoursStatusName { get; set; }
        public string HoursStatusNameCultureVarient { get; set; }
        public string StatusInName { get; set; }
        public string StatusInNameCultureVarient { get; set; }
        public string StatusOutName { get; set; }
        public string StatusOutNameCultureVarient { get; set; }
        public int TSHighLight { get; set; }
        public int PCSHighLight { get; set; }
        public int HSHighLight { get; set; }
        public int SINHighLight { get; set; }
        public int SOUTHighLight { get; set; }

        public string GetLocalizedDetailedStatus()
        {
            // in status ...         
            string ACIN = SINHighLight == (int)AttendanceCodesImportance.Major ? Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar") ? StatusInNameCultureVarient : StatusInName : "";

            // out status ...           
            string ACOT = SOUTHighLight == (int)AttendanceCodesImportance.Major ? Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar") ? StatusOutNameCultureVarient : StatusOutName : "";

            // total status ...
            var TS = TSHighLight == (int)AttendanceCodesImportance.Major ? Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar") ? TotalStatusNameCultureVarient : TotalStatusName : "";

            if ((string.IsNullOrEmpty(TS) || this.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatusId != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                //TS = this.PayCodeStatus.Code;
                TS = Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar") ? PayCodeStatusNameCultureVarient : PayCodeStatusName;
            }

            var HS = PCSHighLight == (int)AttendanceCodesImportance.Major ? Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar") ? HoursStatusNameCultureVarient : HoursStatusName : "";

            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");

            DS = !string.IsNullOrEmpty(HS) ? (!string.IsNullOrEmpty(DS) ? DS.Replace(" )", string.Format(" / {0})", HS)) : string.Format("( {0} )", HS)) : DS;

            // ...
            return !string.IsNullOrEmpty(TS) && !string.IsNullOrEmpty(DS) ? string.Format("{0} {1}", TS, DS) : TS + DS;
        }

         [XDontSerialize]
        public string EventDate_Formatted
        {
            get { return EventDate.ToString("dd/MM/yyyy"); }
        }
         [XDontSerialize]
        public string ActualIn_Formatted
        {
            get
            {
                return ActualIn.HasValue ? ActualIn.Value.ToString("hh:mm tt") : string.Empty;
            }
        }
         [XDontSerialize]
        public string ActualOut_Formatted
        {
            get
            {
                return ActualOut.HasValue ? ActualOut.Value.ToString("hh:mm tt") : string.Empty;
            }
        }
         [XDontSerialize]
        public string Status
        {
            get { return GetLocalizedDetailedStatus(); }
        }
    }
}
