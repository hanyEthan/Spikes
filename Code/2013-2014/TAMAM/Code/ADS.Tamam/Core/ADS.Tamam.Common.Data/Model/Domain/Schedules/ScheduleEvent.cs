using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using ADS.Common.Contracts;
using ADS.Common.Workflow.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleEvent : IWorkflowTarget, IXSerializable
    {
        #region props ...

        public Guid Id { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime? ExpectedIn { get; set; }
        public DateTime? ActualIn { get; set; }
        public Guid InTimeRawId { get; set; }
        public bool InTimeIsOriginal { get; set; }
        public ManualAttendanceStatus InManualAttendanceStatus { get; set; }
        public string InAttendanceSource { get; set; }

        public DateTime? ExpectedOut { get; set; }
        public DateTime? ActualOut { get; set; }
        public Guid OutTimeRawId { get; set; }
        public bool OutTimeIsOriginal { get; set; }
        public ManualAttendanceStatus OutManualAttendanceStatus { get; set; }
        public string OutAttendanceSource { get; set; }

        public int Hours { get; set; }
        public int CalculatedHours { get; set; }
        public int? OffHours { get; set; }
        public int OffHoursCalculated { get; set; }
        public int Overtime { get; set; }
        public int CalculatedOvertime { get; set; }
        public int WorkingHoursExpected { get; set; }
        public int WorkingHoursTotal { get; set; }
        public bool IsDirty { get; set; }

        public Guid? TotalStatusId { get; set; }
        public AttendanceCode TotalStatus { get; set; }

        public Guid? PayCodeStatusId { get; set; }
        public AttendanceCode PayCodeStatus { get; set; }

        public Guid? HoursStatusId { get; set; }
        public AttendanceCode HoursStatus { get; set; }

        public Guid? StatusInId { get; set; }
        public AttendanceCode StatusIn { get; set; }

        public Guid? StatusOutId { get; set; }
        public AttendanceCode StatusOut { get; set; }

        public JustificationStatus JustificationStatus { get; set; }
        public string StaffComments { get; set; }
        public string ManagerComments { get; set; }

        public Guid PersonId { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid ShiftId { get; set; }
        public int? LeaveTypeId { get; set; }

        public string HolidayName { get; set; }
        public string HolidayNameCultureVariant { get; set; }

        public string Comments { get; set; }
        public string TerminalId { get; set; }
        public string Location { get; set; }
        public Schedule Schedule { get; set; }
        public Person Person { get; set; }
        public Shift Shift { get; set; }

        public IList<AttendanceEvent> AttendanceEvents { get; set; }

        #endregion
        #region cst ...

        public ScheduleEvent()
        {
            AttendanceEvents = new List<AttendanceEvent>();
        }
        public ScheduleEvent(Guid id, Person person, Schedule schedule, Shift shift, DateTime date, DateTime timeIn, DateTime timeOut, DateTime expectedIn, DateTime expectedOut, int hours, int hoursCalculated, int overtime, int overtimeCalculated, AttendanceCode payCode, AttendanceCode Status, AttendanceCode hoursStatus)
        {
            Id = id;
            Person = person;
            Shift = shift;
            Schedule = schedule;

            PersonId = person == null ? Guid.Empty : person.Id;
            ShiftId = shift == null ? Guid.Empty : shift.Id;
            ScheduleId = schedule == null ? Guid.Empty : schedule.Id;

            EventDate = date;
            ActualIn = timeIn;
            ActualOut = timeOut;
            ExpectedIn = expectedIn;
            ExpectedOut = expectedOut;

            Hours = hours;
            CalculatedHours = hoursCalculated;
            Overtime = overtime;
            CalculatedOvertime = overtimeCalculated;

            PayCodeStatus = payCode;
            PayCodeStatusId = payCode == null ? Guid.Empty : payCode.Id;

            TotalStatus = Status;
            TotalStatusId = Status == null ? Guid.Empty : Status.Id;

            HoursStatus = hoursStatus;
            HoursStatusId = hoursStatus == null ? Guid.Empty : hoursStatus.Id;

            AttendanceEvents = new List<AttendanceEvent>();
        }

        #endregion
        #region classes

        [Serializable]
        public class ScheduleEventStatuses
        {
            public Guid? InStatus { get; set; }
            public Guid? OutStatus { get; set; }
            public Guid? TotalStatus { get; set; }
            public Guid? Paycode { get; set; }
            public Guid? HoursStatus { get; set; }
        }

        #endregion
        #region IWorkflowTarget

        [XDontSerialize]
        public double EffectiveAmount
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
        #region Helpers

        public IList<AttendanceEvent> GetBreaks()
        {
            if (this.AttendanceEvents == null || this.AttendanceEvents.Count == 0) return new List<AttendanceEvent>();
            return this.AttendanceEvents.Where(x => x.InTime != null && x.OutTime != null).ToList();
        }
        public List<AttendanceEvent> GetWorkingDurations()
        {
            var periods = new List<AttendanceEvent>();
            if (this.AttendanceEvents == null || this.AttendanceEvents.Count <= 1) return periods;

            var AEs = this.AttendanceEvents != null ? this.AttendanceEvents.OrderBy(x => x.InTime).ToList() : null;
            if (AEs == null || AEs.Count <= 1) return periods;

            var AEIn = AEs.Where(x => x.OutTime == null).ToList();
            var AEOut = AEs.Where(x => x.InTime == null).ToList();
            if (AEIn == null || AEIn.Count != 1) return periods;
            if (AEOut.Count != 1) return periods;

            AttendanceEvent AELastIn = AEIn[0];
            AttendanceEvent AELastOut = null;

            #region loop on breaks ...

            foreach (var AE in AEs)
            {
                if (AE.InTime == null || AE.OutTime == null) continue;

                AttendanceEvent period;
                AELastOut = AE;

                if (AELastIn.OutTime == null)
                {
                    period = new AttendanceEvent()
                    {
                        InTime = AELastIn.InTime,
                        InTimeIsOriginal = AELastIn.InTimeIsOriginal,
                        InTimeRawId = AELastIn.InTimeRawId,
                        StatusInId = AELastIn.StatusInId,
                        OutTime = AELastOut.InTime,
                        OutTimeIsOriginal = AELastOut.InTimeIsOriginal,
                        OutTimeRawId = AELastOut.InTimeRawId,
                        StatusOutId = AELastOut.StatusInId,

                        Duration = (int)(AELastOut.InTime.Value - AELastIn.InTime.Value).TotalMinutes,
                        CalculatedDuration = (int)(AELastOut.InTime.Value - AELastIn.InTime.Value).TotalMinutes,
                    };


                }
                else
                {
                    period = new AttendanceEvent()
                    {
                        InTime = AELastIn.OutTime,
                        InTimeIsOriginal = AELastIn.OutTimeIsOriginal,
                        InTimeRawId = AELastIn.OutTimeRawId,
                        StatusInId = AELastIn.StatusOutId,
                        OutTime = AELastOut.InTime,
                        OutTimeIsOriginal = AELastOut.InTimeIsOriginal,
                        OutTimeRawId = AELastOut.InTimeRawId,
                        StatusOutId = AELastOut.StatusInId,

                        Duration = (int)(AELastOut.InTime.Value - AELastIn.OutTime.Value).TotalMinutes,
                        CalculatedDuration = (int)(AELastOut.InTime.Value - AELastIn.OutTime.Value).TotalMinutes,
                    };
                }

                // for night shifts ...
                period.Duration += period.Duration < 0 ? (24 * 60) : 0;
                period.CalculatedDuration += period.CalculatedDuration < 0 ? (24 * 60) : 0;

                AELastIn = AE;
                periods.Add(period);
            }

            #endregion
            #region last period ...

            if (AEOut != null)
            {
                AttendanceEvent period;
                AELastOut = AEOut[0];

                if (AELastIn.OutTime == null)
                {
                    period = new AttendanceEvent()
                    {
                        InTime = AELastIn.InTime,
                        InTimeIsOriginal = AELastIn.InTimeIsOriginal,
                        InTimeRawId = AELastIn.InTimeRawId,
                        StatusInId = AELastIn.StatusInId,
                        OutTime = AELastOut.OutTime,
                        OutTimeIsOriginal = AELastOut.OutTimeIsOriginal,
                        OutTimeRawId = AELastOut.OutTimeRawId,
                        StatusOutId = AELastOut.StatusOutId,

                        Duration = (int)(AELastOut.OutTime.Value - AELastIn.InTime.Value).TotalMinutes,
                        CalculatedDuration = (int)(AELastOut.OutTime.Value - AELastIn.InTime.Value).TotalMinutes,
                    };
                }
                else
                {
                    period = new AttendanceEvent()
                    {
                        InTime = AELastIn.OutTime,
                        InTimeIsOriginal = AELastIn.OutTimeIsOriginal,
                        InTimeRawId = AELastIn.OutTimeRawId,
                        StatusInId = AELastIn.StatusOutId,
                        OutTime = AELastOut.OutTime,
                        OutTimeIsOriginal = AELastOut.OutTimeIsOriginal,
                        OutTimeRawId = AELastOut.OutTimeRawId,
                        StatusOutId = AELastOut.StatusOutId,

                        Duration = (int)(AELastOut.OutTime.Value - AELastIn.OutTime.Value).TotalMinutes,
                        CalculatedDuration = (int)(AELastOut.OutTime.Value - AELastIn.OutTime.Value).TotalMinutes,
                    };
                }

                // for night shifts ...
                period.Duration += period.Duration < 0 ? (24 * 60) : 0;
                period.CalculatedDuration += period.CalculatedDuration < 0 ? (24 * 60) : 0;

                periods.Add(period);
            }

            #endregion

            return periods;
        }

        public string GetDetailedStatus()
        {
            // in status ...
            string ACIN = this.StatusIn != null ? this.StatusIn.Importance == AttendanceCodesImportance.Major ? this.StatusIn.Code : "" : "";

            // out status ...
            string ACOT = this.StatusOut != null ? this.StatusOut.Importance == AttendanceCodesImportance.Major ? this.StatusOut.Code : "" : "";

            // total status ...
            var TS = this.TotalStatus != null ? this.TotalStatus.Importance == AttendanceCodesImportance.Major ? this.TotalStatus.Name : "" : "";

            if ((string.IsNullOrEmpty(TS) || this.TotalStatus.Id == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatus != null && this.PayCodeStatus.Id != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                TS = this.PayCodeStatus.Code;
            }

            var HS = this.HoursStatus != null ? this.HoursStatus.Importance == AttendanceCodesImportance.Major ? this.HoursStatus.Code : "" : "";

            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");

            DS = !string.IsNullOrEmpty(HS) ? (!string.IsNullOrEmpty(DS) ? DS.Replace(" )", string.Format(" / {0})", HS)) : string.Format("( {0} )", HS)) : DS;

            // ...
            return !string.IsNullOrEmpty(TS) && !string.IsNullOrEmpty(DS) ? string.Format("{0} {1}", TS, DS) : TS + DS;
        }
        public ScheduleEventStatuses GetDetailedStatusModel()
        {
            var status = new ScheduleEventStatuses();

            if (this.AttendanceEvents != null)
            {
                // in status ...
                var AEIN = this.AttendanceEvents.Where(x => x.InTime == this.ActualIn && x.InTime != null).FirstOrDefault();
                if (AEIN != null) status.InStatus = AEIN.StatusInId;

                // out status ...
                var AEOT = this.AttendanceEvents.Where(x => x.OutTime == this.ActualOut && x.OutTime != null).FirstOrDefault();
                if (AEOT != null) status.OutStatus = AEOT.StatusOutId;
            }

            // total status ...
            if (this.TotalStatusId.HasValue) status.TotalStatus = this.TotalStatusId.Value;

            // paycode ...
            if (this.PayCodeStatusId.HasValue) status.Paycode = this.PayCodeStatusId.Value;

            //Hour Status
            if (this.HoursStatusId.HasValue) status.HoursStatus = this.HoursStatusId.Value;
            // ...
            return status;
        }
        public string GetLocalizedDetailedStatus()
        {
            // in status ...
            string ACIN = this.StatusIn != null ? this.StatusIn.Importance == AttendanceCodesImportance.Major ? this.StatusIn.Code : "" : "";

            // out status ...
            string ACOT = this.StatusOut != null ? this.StatusOut.Importance == AttendanceCodesImportance.Major ? this.StatusOut.Code : "" : "";

            // total status ...
            var TS = this.TotalStatus != null ? this.TotalStatus.Importance == AttendanceCodesImportance.Major ? GetStatusNameCultureAware(this.TotalStatus) : "" : "";

            if ((string.IsNullOrEmpty(TS) || this.TotalStatus.Id == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatus != null && this.PayCodeStatus.Id != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                //TS = this.PayCodeStatus.Code;
                TS = GetStatusNameCultureAware(this.PayCodeStatus);
            }

            var HS = this.HoursStatus != null ? this.HoursStatus.Importance == AttendanceCodesImportance.Major ? this.HoursStatus.Code : "" : "";

            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");

            DS = !string.IsNullOrEmpty(HS) ? (!string.IsNullOrEmpty(DS) ? DS.Replace(" )", string.Format(" / {0})", HS)) : string.Format("( {0} )", HS)) : DS;

            // ...
            return !string.IsNullOrEmpty(TS) && !string.IsNullOrEmpty(DS) ? string.Format("{0} {1}", TS, DS) : TS + DS;
        }
        public string GetLocalizedDetailedStatus_Full()
        {
            // in status ...
            string ACIN = this.StatusIn != null ? this.StatusIn.Code : "";

            // out status ...
            string ACOT = this.StatusOut != null ? this.StatusOut.Code : "";

            // total status ...
            var TS = this.TotalStatus != null ? GetStatusNameCultureAware(this.TotalStatus) : "";

            if ((string.IsNullOrEmpty(TS) || this.TotalStatus.Id == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatus != null && this.PayCodeStatus.Id != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                //TS = this.PayCodeStatus.Code;
                TS = GetStatusNameCultureAware(this.PayCodeStatus);
            }

            var HS = this.HoursStatus != null ? this.HoursStatus.Importance == AttendanceCodesImportance.Major ? this.HoursStatus.Code : "" : "";
            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");

            DS = !string.IsNullOrEmpty(HS) ? (!string.IsNullOrEmpty(DS) ? DS.Replace(" )", string.Format(" / {0})", HS)) : string.Format("( {0} )", HS)) : DS;

            // ...
            return !string.IsNullOrEmpty(TS) && !string.IsNullOrEmpty(DS) ? string.Format("{0} {1}", TS, DS) : TS + DS;
        }
        public string GetLocalizedDetailedStatus_Full(string culture)
        {
            var isLocalized = culture.ToLower().Contains("ar");

            // in status ...
            string ACIN = this.StatusIn != null ? GetStatusName(this.StatusIn, culture) : "";

            // out status ...
            string ACOT = this.StatusOut != null ? GetStatusName(this.StatusOut, culture) : "";

            // total status ...
            var TS = GetStatusName(this.TotalStatus, culture);

            if ((string.IsNullOrEmpty(TS) || this.TotalStatus.Id == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatus != null && this.PayCodeStatus.Id != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                //TS = GetStatusName( this.PayCodeStatus , culture );
                TS = GetStatusName(this.PayCodeStatus, culture);
            }

            var HS = this.HoursStatus != null ? this.HoursStatus.Importance == AttendanceCodesImportance.Major ? this.HoursStatus.Code : "" : "";
            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");
            DS = !string.IsNullOrEmpty(HS) ? (!string.IsNullOrEmpty(DS) ? DS.Replace(" )", string.Format(" / {0})", HS)) : string.Format("( {0} )", HS)) : DS;

            // working hours ...
            var HRS = CalculatedHours != 0 ? isLocalized ? "أوقات العمل : " : " Working Hours : " + XConvert.FromMinutesToHours(CalculatedHours) : "";

            // ...
            var STS = !string.IsNullOrEmpty(TS) && !string.IsNullOrEmpty(DS) ? string.Format("{0} {1}", TS, DS) : TS + DS;
            return STS + HRS;
        }


        public string GetLocalizedDetailedInOutStatus(string culture)
        {
            var isLocalized = culture.ToLower().Contains("ar");

            // in status ...
            string ACIN =  this.StatusIn != null ? GetStatusName(this.StatusIn, culture) : "";
            if (string.IsNullOrEmpty(ACIN) && this.TotalStatusId == AttendanceCodes.ScheduleEventStatus.MissedInPunch)
            {
                ACIN = GetStatusName(this.TotalStatus, culture);
            }
            // out status ...
            string ACOT = this.StatusOut != null ? GetStatusName(this.StatusOut, culture) : "";
            if (string.IsNullOrEmpty(ACOT) && this.TotalStatusId == AttendanceCodes.ScheduleEventStatus.MissedOutPunch)
            {
                ACOT = GetStatusName(this.TotalStatus, culture);
            }
            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");

            // total status ...
            var TS = GetStatusName(this.TotalStatus, culture);

            if ((string.IsNullOrEmpty(TS) || this.TotalStatus.Id == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatus != null && this.PayCodeStatus.Id != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                //TS = GetStatusName( this.PayCodeStatus , culture );
                TS = GetStatusName(this.PayCodeStatus, culture);
            }

            return string.IsNullOrWhiteSpace(DS) ? TS : DS;
        }

        public string GetLocalizedDetailedStatus_Detailed()
        {
            var isLocalized = Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar");

            // in status ...
            string ACIN =  this.StatusIn != null ? GetStatusNameCultureAware(this.StatusIn) : "";

            // out status ...
            string ACOT = this.StatusOut != null ? GetStatusNameCultureAware(this.StatusOut) : "";

            // total status ...
            var TS = this.TotalStatus != null ? GetStatusNameCultureAware(this.TotalStatus) : "";

            if ((string.IsNullOrEmpty(TS) || this.TotalStatus.Id == AttendanceCodes.ScheduleEventStatus.Attended) && (this.PayCodeStatus != null && this.PayCodeStatus.Id != AttendanceCodes.PayCodeStatus.NormalDay))
            {
                //TS = this.PayCodeStatus.Code;
                TS = GetStatusNameCultureAware(this.PayCodeStatus);
            }

            var HS = this.HoursStatus != null ? this.HoursStatus.Importance == AttendanceCodesImportance.Major ? this.HoursStatus.Code : "" : "";
            var DS = !string.IsNullOrEmpty(ACIN) && !string.IsNullOrEmpty(ACOT) ? string.Format("( {0} / {1} )", ACIN, ACOT) : string.Format("( {0} )", ACIN + ACOT).Replace("(  )", "");
            DS = !string.IsNullOrEmpty(HS) ? (!string.IsNullOrEmpty(DS) ? DS.Replace(" )", string.Format(" / {0})", HS)) : string.Format("( {0} )", HS)) : DS;
            // working hours ...
            var HRS = CalculatedHours != 0 ? isLocalized ? "أوقات العمل : " : " Working Hours : " + XConvert.FromMinutesToHours(CalculatedHours) : "";

            // ...
            var STS = !string.IsNullOrEmpty(TS) && !string.IsNullOrEmpty(DS) ? string.Format("{0} {1}", TS, DS) : TS + DS;
            return STS + HRS;
        }

        private string GetStatusName(AttendanceCode AC, string culture)
        {
            return AC != null ? (!string.IsNullOrEmpty(culture) && culture.ToLower().Contains("ar")) ? AC.NameCultureVarient : AC.Name : "";
        }
        private string GetStatusNameCultureAware(AttendanceCode AC)
        {
            return AC != null ? Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar") ? AC.NameCultureVarient : AC.Name : "";
        }

        #endregion
        # region ToString

        public override string ToString()
        {
            var isLocalized = Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("ar");

            string meta_command = isLocalized ? "التمام" : "Attendance";
            string meta_person = Person != null ? isLocalized ? Person.FullNameCultureVarient : Person.FullName : PersonId.ToString();
            string meta_date = EventDate.ToString("dd/MM/yyyy");
            string meta_status = GetLocalizedDetailedStatus_Detailed();

            string meta_format = isLocalized ? "{0} للموظف ({1}) في التاريخ ({2}) بالحالة [{3}] بالنسبة لأوقات عمله"
                                             : "{0} for ({1}) on ({2}), with status [{3}].";

            return string.Format(meta_format, meta_command, meta_person, meta_date, meta_status);
        }

        # endregion
    }
}
