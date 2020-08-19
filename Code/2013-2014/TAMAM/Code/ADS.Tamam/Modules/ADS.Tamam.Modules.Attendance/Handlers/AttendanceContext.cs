using System;
using System.Linq;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using XCore.Framework.Utilities;

namespace ADS.Tamam.Modules.Attendance.Handlers
{
    internal class AttendanceContext
    {
        #region Nested

        public class ExcuseMetaData
        {
            public Excuse Excuse { get; set; }
            public int ShiftStartOffset { get; set; }
            public int ShiftEndOffset { get; set; }
            public int ShiftEffectiveOffset { get; set; }
            //public Guid AssociatedRawAttendanceId { get; set; }
        }

        #endregion

        #region props ...

        internal Shift Shift { get; set; }
        internal Shift ShiftPrevious { get; set; }
        internal List<Schedule> Schedules { get; private set; }   // all schedules
        internal Schedule Schedule { get; private set; }   // effective Schedule

        internal List<ExcuseMetaData> ExcuseMetaDataList { get; private set; }

        internal List<Policy> Policies { get; private set; }
        internal ShiftPolicy ShiftPolicy { get; private set; }
        internal OverTimePolicy OverTimePolicy { get; private set; }

        internal Guid PayCodeStatus { get; set; }
        internal ScheduleDay ScheduleDay { get; private set; }
        internal OverTimeSetting OverTime { get; private set; }

        internal AttendanceRawData RawAttendance { get; private set; }
        internal ScheduleEvent ScheduleEvent { get; set; }

        private readonly IAttendanceDataHandler dataHandler;
        private readonly RequestContext requestContext;
        private int? _AssociatedLeaveTypeId { get; set; }

        private string _AssociatedHolidayName { get; set; }
        private string _AssociatedHolidayNameCultureVariant { get; set; }

        internal int NightShiftDelta { get; set; }
        internal int BreaksTotalDuration { get; set; }
        internal int DuplicatePunchThreshold;

        private bool _AutoDetectedType;

        internal List<AttendanceRawData> SERawAttendances { get; private set; }

        #endregion
        #region cst ...

        public AttendanceContext(AttendanceRawData raw, RequestContext requestContext)
        {
            try
            {
                // detect attendance status mode ...
                if (!bool.TryParse(Broker.ConfigurationHandler.GetValue(ADS.Common.Constants.TamamCaptureConfig.Section, ADS.Common.Constants.TamamCaptureConfig.TypeAutoDetectionMode), out _AutoDetectedType)) _AutoDetectedType = false;

                this.RawAttendance = raw;
                if (!int.TryParse(Broker.ConfigurationHandler.GetValue(ADS.Common.Constants.TamamEngineConfig.Section, ADS.Common.Constants.TamamEngineConfig.DuplicatePunchThreshold), out DuplicatePunchThreshold)) DuplicatePunchThreshold = 0;
                this.requestContext = requestContext;
                this.dataHandler = TamamDataBroker.GetRegisteredDataLayer<IAttendanceDataHandler>(TamamConstants.AttendanceDataHandlerName);

                this.Policies = TamamServiceBroker.OrganizationHandler.GetPolicies(raw.PersonId, requestContext).Result;
                this.OverTimePolicy = new OverTimePolicy(Policies.FirstOrDefault(p => p.PolicyTypeId == Guid.Parse(PolicyTypes.OvertimePolicyType)));

                #region check yesterday's night shift ( if exists )

                bool associatedToYesterdayShift = false;
                Schedule yesterdaysSchedule;
                ScheduleDay yesterdaysScheduleDay;
                ScheduleDay todaysScheduleDay;
                Schedule todaysSchedule;
                var todaysShift = ShiftGetByProximity(GetDayShifts(RawAttendance.AttendanceDateTime, out todaysScheduleDay, out todaysSchedule), raw);
                var todaysShiftPolicy = GetShiftPolicy(this.Policies, todaysShift);
                var yesterdaysShifts = GetDayShifts(RawAttendance.AttendanceDateTime.AddDays(-1), out yesterdaysScheduleDay, out yesterdaysSchedule);
                var yesterdaysNightShift = yesterdaysShifts.Where(x => x.IsNightShift).FirstOrDefault();
                var nightShiftPolicy = yesterdaysNightShift == null ? null : GetShiftPolicy(this.Policies, yesterdaysNightShift);
                var nightShiftLateMargin = new TimeSpan(0, nightShiftPolicy != null ? nightShiftPolicy.ShiftEndMargin : 0, 0);

                #region Auto Detected Type Mode
                if (_AutoDetectedType)
                {
                    TimeSpan todayStartDiff, yesterdayEndDiff;

                    if (yesterdaysNightShift != null)
                    {
                        if (RawAttendance.AttendanceDateTime.TimeOfDay < yesterdaysNightShift.StartTime.Value.TimeOfDay && RawAttendance.AttendanceDateTime.TimeOfDay <= yesterdaysNightShift.EndTime.Value.TimeOfDay)
                        {
                            associatedToYesterdayShift = true;
                        }
                        else if (RawAttendance.AttendanceDateTime.TimeOfDay > yesterdaysNightShift.EndTime.Value.TimeOfDay && RawAttendance.AttendanceDateTime.TimeOfDay < yesterdaysNightShift.EndTime.Value.TimeOfDay.Add(nightShiftLateMargin))
                        {
                            if (todaysShift != null)
                            {
                                todayStartDiff = todaysShift.StartTime.Value.TimeOfDay - RawAttendance.AttendanceDateTime.TimeOfDay;
                                yesterdayEndDiff = RawAttendance.AttendanceDateTime.TimeOfDay - yesterdaysNightShift.EndTime.Value.TimeOfDay;
                                if (yesterdayEndDiff <= todayStartDiff)
                                {
                                    associatedToYesterdayShift = true;
                                }
                            }
                            else
                            {
                                associatedToYesterdayShift = true;
                            }
                        }
                        if (associatedToYesterdayShift)
                        {
                            this.Schedule = yesterdaysSchedule;
                            this.ScheduleDay = yesterdaysScheduleDay;
                            this.Shift = yesterdaysNightShift;
                            this.ShiftPolicy = nightShiftPolicy ?? new ShiftPolicy(null);
                        }
                    }

                    if ((todaysShift != null && !todaysShift.IsNightShift && IsTimeInsideShift(RawAttendance.AttendanceDateTime, todaysShift, todaysShiftPolicy, true)) || !associatedToYesterdayShift)
                    {
                        this.Schedule = todaysSchedule;
                        this.ScheduleDay = todaysScheduleDay;
                        this.Shift = ScheduleDay != null ? todaysShift : null;
                        this.ShiftPolicy = todaysShiftPolicy ?? new ShiftPolicy(null);
                    }
                }
                #endregion
                #region Normal Mode
                else 
                {
                    // yesterday's night shift ?
                    if (yesterdaysNightShift != null && RawAttendance.AttendanceDateTime.TimeOfDay < yesterdaysNightShift.StartTime.Value.TimeOfDay && RawAttendance.AttendanceDateTime.TimeOfDay < yesterdaysNightShift.EndTime.Value.TimeOfDay.Add(RawAttendance.Type == AttendanceEventType.Out ? nightShiftLateMargin : new TimeSpan(0, 0, 0)))
                    {
                        this.Schedule = yesterdaysSchedule;
                        this.ScheduleDay = yesterdaysScheduleDay;
                        this.Shift = yesterdaysNightShift;
                        this.ShiftPolicy = nightShiftPolicy ?? new ShiftPolicy(null);
                        associatedToYesterdayShift = true;
                    }

                    // today's other shifts ?
                    if ((todaysShift != null && !todaysShift.IsNightShift && IsTimeInsideShift(RawAttendance.AttendanceDateTime, todaysShift, todaysShiftPolicy, true)) || !associatedToYesterdayShift)
                    {
                        this.Schedule = todaysSchedule;
                        this.ScheduleDay = todaysScheduleDay;
                        this.Shift = ScheduleDay != null ? todaysShift : null;
                        this.ShiftPolicy = todaysShiftPolicy ?? new ShiftPolicy(null);
                    }
                }
                #endregion

                #endregion

                this.PayCodeStatus = PayCodeGet();
                this.OverTime = GetOverTimeSetting(OverTimePolicy, PayCodeStatus);
                this.PayCodeStatus = PayCodeValidate();

                if (Shift != null)
                {
                    #region Night Shift delta

                    if (Shift.IsNightShift)
                    {
                        //delta is the number of hour that need to be added on all times to make it in the same day 
                        //so it will be the different between 23 -the last hour in the day- and the lager number i have in this handle
                        //if raw date time is in shift range so the shift end is large than raw date time so the delta will be 23-shift end
                        //else if raw date time is IN and out shift range -that mean it before shift start-  so the shift end will be large then raw date time so the delta will be 23-shift end
                        //else the raw date time will be the large time so the delta will be 23 - raw date time
                        NightShiftDelta = 23 - ((RawAttendance.AttendanceDateTime.Hour >= Shift.StartTime.Value.Hour || RawAttendance.AttendanceDateTime.Hour <= Shift.EndTime.Value.Hour)
                                        ? Shift.EndTime.Value.Hour
                                        : (RawAttendance.Type == AttendanceEventType.In ? Shift.EndTime.Value.Hour : RawAttendance.AttendanceDateTime.Hour));
                    }

                    #endregion

                    if (PayCodeStatus != AttendanceCodes.PayCodeStatus.NormalDay && PayCodeStatus != AttendanceCodes.PayCodeStatus.WorkOnUnScheduled)
                    {
                        var scheduleEvents = dataHandler.GetScheduleEvent(raw.PersonId, this.ScheduleDay.Day).Result;
                        this.ScheduleEvent = scheduleEvents != null ? scheduleEvents.Where(x => x.ShiftId == Shift.Id).FirstOrDefault() : null;
                        this.ScheduleEvent = this.ScheduleEvent == null ? (scheduleEvents != null ? scheduleEvents.Where(x => x.ShiftId == Guid.Empty).FirstOrDefault() : null) : this.ScheduleEvent;
                    }
                    else
                    {
                        this.ScheduleEvent = dataHandler.GetScheduleEvent(raw.PersonId, Shift.Id, this.ScheduleDay.Day).Result;
                    }

                    #region collect excuses and breaks ( Neo in Matrix )

                    this.ExcuseMetaDataList = new List<ExcuseMetaData>();
                    var excuses = ExcusesLookFor(this.RawAttendance.AttendanceDateTime.Date, this.Shift.IsNightShift ? this.Shift.StartTime.Value.AddHours(this.NightShiftDelta) : (this.Shift.IsFlexible ? new DateTime(1800, 1, 1) : this.Shift.StartTime.Value), this.Shift.IsNightShift ? this.Shift.EndTime.Value.AddHours(this.NightShiftDelta) : (this.Shift.IsFlexible ? new DateTime(1800, 1, 1).AddDays(1).AddMinutes(-1) : this.Shift.EndTime.Value), this.Shift.IsNightShift, this.NightShiftDelta);
                    foreach (var shiftExcuse in excuses)
                    {
                        var excuseMetaDataModel = new ExcuseMetaData();
                        excuseMetaDataModel.Excuse = shiftExcuse;
                        if (!(Shift.IsFlexible || Shift.IsNightShift))
                        {
                            excuseMetaDataModel.ShiftStartOffset = XIntervals.IsOverlapped(new XIntervals.TimeRange(this.Shift.StartTime.Value.TimeOfDay
                                , this.Shift.StartTime.Value.AddMinutes(this.ShiftPolicy.LateComeGrace).TimeOfDay),
                                new XIntervals.TimeRange(shiftExcuse.StartTime.TimeOfDay, shiftExcuse.EndTime.TimeOfDay))
                                                                 ? (int)(shiftExcuse.StartTime.TimeOfDay - this.Shift.StartTime.Value.TimeOfDay).TotalMinutes : 0;
                            excuseMetaDataModel.ShiftEndOffset = this.Shift.EndTime.Value.TimeOfDay <= shiftExcuse.EndTime.TimeOfDay ?
                               (int)(this.Shift.EndTime.Value.TimeOfDay - shiftExcuse.EndTime.TimeOfDay).TotalMinutes : 0;
                        }
                        else if (Shift.IsNightShift)
                        {
                            var adjustmentExcuseEnd = shiftExcuse.EndTime.AddHours(this.NightShiftDelta).Hour == 0 ? shiftExcuse.EndTime.AddHours(this.NightShiftDelta).TimeOfDay.Add(new TimeSpan(24, 0, 0)) : shiftExcuse.EndTime.AddHours(this.NightShiftDelta).TimeOfDay;
                            excuseMetaDataModel.ShiftStartOffset = XIntervals.IsOverlapped(new XIntervals.TimeRange(this.Shift.StartTime.Value.AddHours(this.NightShiftDelta).TimeOfDay
                               , this.Shift.StartTime.Value.AddHours(this.NightShiftDelta).AddMinutes(this.ShiftPolicy.LateComeGrace).TimeOfDay),
                               new XIntervals.TimeRange(shiftExcuse.StartTime.AddHours(this.NightShiftDelta).TimeOfDay, adjustmentExcuseEnd))
                                                                ? (int)(shiftExcuse.StartTime.AddHours(this.NightShiftDelta) - this.Shift.StartTime.Value.AddHours(this.NightShiftDelta)).TotalMinutes : 0;

                            excuseMetaDataModel.ShiftEndOffset = this.Shift.EndTime.Value.AddHours(this.NightShiftDelta).TimeOfDay <= adjustmentExcuseEnd ?
                               (int)(this.Shift.EndTime.Value.AddHours(this.NightShiftDelta).TimeOfDay - adjustmentExcuseEnd).TotalMinutes : 0;
                        }
                        else
                        {
                            excuseMetaDataModel.ShiftStartOffset = 0;
                            excuseMetaDataModel.ShiftEndOffset = 0;
                            excuseMetaDataModel.ShiftEffectiveOffset = 0;
                        }
                        if (excuseMetaDataModel.ShiftStartOffset == 0 && excuseMetaDataModel.ShiftEndOffset == 0)
                            excuseMetaDataModel.ShiftEffectiveOffset = 0;
                        else
                            excuseMetaDataModel.ShiftEffectiveOffset = Math.Min(excuseMetaDataModel.ShiftStartOffset == 0 ? 1440 : excuseMetaDataModel.ShiftStartOffset,
                                                                                excuseMetaDataModel.ShiftEndOffset == 0 ? 1440 : excuseMetaDataModel.ShiftEndOffset);
                        this.ExcuseMetaDataList.Add(excuseMetaDataModel);
                    }

                    #endregion
                }
                else
                {
                    var scheduleEvents = dataHandler.GetScheduleEvent(raw.PersonId, ScheduleDay != null ? this.ScheduleDay.Day : raw.AttendanceDateTime.Date).Result;
                    this.ScheduleEvent = scheduleEvents != null ? scheduleEvents.FirstOrDefault() : null;
                    this.ShiftPrevious = ScheduleDay != null ? ScheduleDay.DayShifts.Select(sh => sh.Shift).Where(sh => this.ScheduleEvent != null && sh.Id == this.ScheduleEvent.ShiftId).FirstOrDefault() : null;
                }

                // SE raw attendances ...
                SERawAttendances = new List<AttendanceRawData>();
                if (ScheduleEvent != null)
                {
                    var response = TamamServiceBroker.AttendanceHandler.GetAttendanceRaw(new List<Guid> { ScheduleEvent.Id }, true, SystemRequestContext.Instance);
                    if (response.Type == ResponseState.Success)
                    {
                        if (response.Result.Count() > 0)
                        {
                            SERawAttendances = response.Result.OrderBy(x => x.AttendanceDateTime).ToList();
                        }
                    }
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
            }
        }

        /// <summary>
        ///  Schedule Event can be dirty if the old detected Type  for Raw Attendance changed or 
        ///  if the current raw attendance was not in the proper order (expected to be the last one)
        ///  the function will also change / detect  the raw attendance types if needed
        /// </summary>
        /// <returns></returns>
        public bool EvaluateSEDirtyState()
        {
            var isdirty = false;
            var AllRawAttendance = SERawAttendances;
            AllRawAttendance.Add(this.RawAttendance);
            AllRawAttendance = AllRawAttendance.OrderBy(x => x.AttendanceDateTime).ToList();
            //If the last on in order not the new attendance so the SE is dirty.
            if (this.RawAttendance.Id != AllRawAttendance[AllRawAttendance.Count() - 1].Id) isdirty = true;

            // skip re-evaluating the raw att. status in case of auto switch mode is turned off, or if the attendance is manually edit or added
            if (_AutoDetectedType) //&& !this.RawAttendance.IsManual
            {
                if (ScheduleEvent != null)
                {
                    if (this.ScheduleEvent.IsDirty) return true;
                    if (SERawAttendances.Count() > 0)
                    {
                        for (int i = 0; i < AllRawAttendance.Count(); i++)
                        {
                            var rawAttendanve = AllRawAttendance[i];
                            var OrgType = rawAttendanve.Type;
                            AttendanceEventType NewType = AttendanceEventType.NotSet;
                            if (i == 0)
                            {
                                NewType = AttendanceEventType.In;
                            }
                            else
                            {
                                var previousRawattendance = AllRawAttendance[i - 1];
                                var timeDiff = rawAttendanve.AttendanceDateTime - previousRawattendance.AttendanceDateTime;
                                if (timeDiff.TotalSeconds <= DuplicatePunchThreshold)
                                {
                                    NewType = AttendanceEventType.NotSet;
                                }
                                else
                                {
                                    if (previousRawattendance.Type == AttendanceEventType.In)
                                    {
                                        NewType = AttendanceEventType.Out;
                                    }
                                    else
                                    {
                                        NewType = AttendanceEventType.In;
                                    }
                                }
                            }
                            if (OrgType != NewType)
                            {
                                if (this.RawAttendance.Id != rawAttendanve.Id && !rawAttendanve.IsManual)
                                {
                                    rawAttendanve.HandleForShiftEnd = false;
                                    rawAttendanve.Type = NewType;
                                    isdirty = true;
                                    dataHandler.UpdateAttendanceRawData(rawAttendanve);
                                }
                                else if (this.RawAttendance.Id == rawAttendanve.Id && !this.RawAttendance.HandleForShiftEnd && !this.RawAttendance.IsManual)
                                {
                                    rawAttendanve.Type = NewType;
                                    dataHandler.UpdateAttendanceRawData(rawAttendanve);
                                }


                            }
                        }
                    }
                    else
                    {
                        if (!this.RawAttendance.HandleForShiftEnd && !this.RawAttendance.IsManual)
                        {
                            this.RawAttendance.Type = AttendanceEventType.In;
                            dataHandler.UpdateAttendanceRawData(this.RawAttendance);
                        }
                    }
                }
                else
                {
                    if (!this.RawAttendance.HandleForShiftEnd && !this.RawAttendance.IsManual)
                    {
                        this.RawAttendance.Type = AttendanceEventType.In;
                        dataHandler.UpdateAttendanceRawData(this.RawAttendance);
                    }
                }

            }

            return isdirty;
        }

        #endregion
        #region models ...

        internal struct OverTimeSetting
        {
            public int Maximum;
            public float Rate;
        }

        #endregion

        #region internals

        internal ScheduleEvent ScheduleEventGet(Guid personId, DateTime date)
        {
            #region Cache

            var cacheKey = "AttendanceContext_ScheduleEventGet" + personId + date.ToShortDateString();
            var cached = Broker.Cache.Get<ScheduleEvent>(TamamCacheClusters.Attendance, cacheKey);
            if (cached != null) return cached;

            #endregion

            var dataHandlerResponse = dataHandler.GetScheduleEvent(personId, date);
            if (dataHandlerResponse.Type != ResponseState.Success) return null;
            var SE = dataHandlerResponse.Result.FirstOrDefault();

            #region Cache

            Broker.Cache.Add<ScheduleEvent>(TamamCacheClusters.Attendance, cacheKey, SE);

            #endregion

            return SE;
        }
        internal List<ScheduleEvent> ScheduleEventsGet(Guid personId, DateTime date)
        {
            #region Cache

            var cacheKey = "AttendanceContext_ScheduleEventsGet" + personId + date.ToShortDateString();
            var cached = Broker.Cache.Get<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey);
            if (cached != null) return cached;

            #endregion

            var dataHandlerResponse = dataHandler.GetScheduleEvent(personId, date);
            if (dataHandlerResponse.Type != ResponseState.Success) return null;
            var SEs = dataHandlerResponse.Result;

            #region Cache

            Broker.Cache.Add<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey, SEs);

            #endregion

            return SEs;
        }

        internal Guid ScheduleEventCreate()
        {
            ScheduleEvent.PayCodeStatusId = PayCodeStatus;
            ScheduleEvent.LeaveTypeId = PayCodeStatus == AttendanceCodes.PayCodeStatus.WorkOnLeave ? _AssociatedLeaveTypeId : null;
            ScheduleEvent.HolidayName = PayCodeStatus == AttendanceCodes.PayCodeStatus.WorkOnHoliday ? _AssociatedHolidayName : null;
            ScheduleEvent.HolidayNameCultureVariant = PayCodeStatus == AttendanceCodes.PayCodeStatus.WorkOnHoliday ? _AssociatedHolidayNameCultureVariant : null;

            ScheduleEvent.WorkingHoursExpected = Shift == null ? 0 : XMath.Round(Shift.Duration * 60);
            ScheduleEvent.WorkingHoursTotal = CalculateWorkingTime();

            #region DL

            var response = dataHandler.CreateScheduleEvent(ScheduleEvent);
            if (response.Type != ResponseState.Success) return Guid.Empty;

            #endregion

            AttendanceHandler.StartViolationWorkflow(ScheduleEvent);

            #region cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion

            return response.Result;
        }
        internal Guid ScheduleEventUpdate()
        {
            ScheduleEvent.PayCodeStatusId = PayCodeStatus;
            ScheduleEvent.LeaveTypeId = PayCodeStatus == AttendanceCodes.PayCodeStatus.WorkOnLeave ? _AssociatedLeaveTypeId : null;

            ScheduleEvent.HolidayName = PayCodeStatus == AttendanceCodes.PayCodeStatus.WorkOnHoliday ? _AssociatedHolidayName : null;
            ScheduleEvent.HolidayNameCultureVariant = PayCodeStatus == AttendanceCodes.PayCodeStatus.WorkOnHoliday ? _AssociatedHolidayNameCultureVariant : null;

            ScheduleEvent.WorkingHoursTotal = CalculateWorkingTime();

            var id = dataHandler.UpdateScheduleEvent(ScheduleEvent).Result;
            AttendanceHandler.StartViolationWorkflow(ScheduleEvent);

            #region Cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion

            return id;
        }
        internal bool ScheduleEventDelete()
        {
            AttendanceHandler.CancelViolationWorkflow(ScheduleEvent);

            var response = dataHandler.DeleteScheduleEvent(ScheduleEvent);
            if (response.Type != ResponseState.Success) return false;

            #region Cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion

            ScheduleEvent = null;
            return true;
        }

        internal bool AttendanceEventAdd(AttendanceEvent attEvent)
        {
            if (this.ScheduleEvent.AttendanceEvents == null) this.ScheduleEvent.AttendanceEvents = new List<AttendanceEvent>();
            this.ScheduleEvent.AttendanceEvents.Add(attEvent);

            return true;
        }
        internal bool AttendanceEventDelete(AttendanceEvent attEvent)
        {
            if (!dataHandler.DeleteAttendanceEvent(attEvent).Result) return false;

            #region Cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion

            return true;
        }

        internal List<Excuse> ExcusesLookFor(DateTime date, DateTime start, DateTime end, bool isNightShift, int nightShiftDelta)
        {
            var excuses = TamamServiceBroker.LeavesHandler.SearchExcuses(new ExcuseSearchCriteria
            {
                StartDate = date,
                EndDate = date,
                Personnel = new List<Guid> { RawAttendance.PersonId },
                ExcuseStatuses = new List<int> { (int)ExcuseStatus.Approved, (int)ExcuseStatus.Taken },
                ActivePersonnelStatus = true,
            }, requestContext).Result.Excuses;

            var excusesFiltered = excuses.Where(x => (isNightShift ? x.StartTime.AddHours(nightShiftDelta).TimeOfDay : x.StartTime.TimeOfDay) <= end.TimeOfDay
                                                                    && start.TimeOfDay <= (isNightShift ? (x.EndTime.AddHours(nightShiftDelta).Hour == 0 ? x.EndTime.AddHours(nightShiftDelta).TimeOfDay.Add(new TimeSpan(24, 0, 0)) : x.EndTime.AddHours(nightShiftDelta).TimeOfDay)
                                                                    : x.EndTime.TimeOfDay)).ToList();

            return excusesFiltered;
        }
        internal List<ExcuseMetaData> ExcusesPreCacheLookFor(DateTime date, DateTime start, DateTime end, bool isNightShift, int nightShiftDelta)
        {
            var excusesFiltered = this.ExcuseMetaDataList.Where(x => (isNightShift ? x.Excuse.StartTime.AddHours(nightShiftDelta).TimeOfDay : x.Excuse.StartTime.TimeOfDay) <= end.TimeOfDay
                                                       && start.TimeOfDay <= (isNightShift ? (x.Excuse.EndTime.AddHours(nightShiftDelta).Hour == 0 ? x.Excuse.EndTime.AddHours(nightShiftDelta).TimeOfDay.Add(new TimeSpan(24, 0, 0)) : x.Excuse.EndTime.AddHours(nightShiftDelta).TimeOfDay)
                                                       : x.Excuse.EndTime.TimeOfDay)).ToList();

            return excusesFiltered;
        }
        internal Leave LeavesLookForHalfLeaves(DateTime date, DateTime start, DateTime end)
        {
            // shift limits ...
            LeaveModePeriod? requiredPeriod = null;
            if (!this.Shift.IsFlexible)
            {
                var shiftStart = this.Shift.IsNightShift ? this.Shift.StartTime.Value.AddHours(this.NightShiftDelta) : this.Shift.StartTime.Value;
                var shiftEnd = this.Shift.IsNightShift ? this.Shift.EndTime.Value.AddHours(this.NightShiftDelta) : this.Shift.EndTime.Value;
                var dayStart = new DateTime(date.Year, date.Month, date.Day, shiftStart.Hour, shiftStart.Minute, shiftStart.Second);
                var dayEnd = new DateTime(date.Year, date.Month, date.Day, shiftEnd.Hour, shiftEnd.Minute, shiftEnd.Second);

                var dayMid = dayStart.AddTicks((dayEnd - dayStart).Ticks / 2);

                // required types of leaves ...
                if (end < start) XIntervals.Swap(ref start, ref end);
                if (end.TimeOfDay < dayStart.TimeOfDay || start.TimeOfDay > dayEnd.TimeOfDay) return null;
                requiredPeriod = start.TimeOfDay <= dayMid.TimeOfDay && end.TimeOfDay <= dayMid.TimeOfDay ? (LeaveModePeriod?)LeaveModePeriod.FirstHalf
                                  : start.TimeOfDay >= dayMid.TimeOfDay && end.TimeOfDay >= dayMid.TimeOfDay ? (LeaveModePeriod?)LeaveModePeriod.SecondHalf
                                  : null;   // null => any
            }

            // leaves ...
            var leaves = TamamServiceBroker.LeavesHandler.SearchLeaves(new LeaveSearchCriteria
            {
                StartDate = date,
                EndDate = date,
                Personnel = new List<Guid> { RawAttendance.PersonId },
                LeaveStatuses = new List<int> { (int)LeaveStatus.Approved, (int)LeaveStatus.Taken },

            }, true, requestContext).Result.Leaves;

            // filter ...
            var targetLeave = requiredPeriod.HasValue ? leaves.FirstOrDefault(x => x.LeaveMode == LeaveMode.HalfDay && x.LeaveModePeriod == requiredPeriod && x.EffectiveDaysCount != 0)
                                                      : leaves.FirstOrDefault(x => x.LeaveMode == LeaveMode.HalfDay && x.EffectiveDaysCount != 0);
            return targetLeave;
        }


        internal void ExcuseChangeToTaken(Excuse excuse)
        {
            excuse.ExcuseStatusId = (int)ExcuseStatus.Taken;
            TamamServiceBroker.LeavesHandler.EditExcuse(excuse, requestContext);
        }

        internal static void ExcuseChangeToTaken(Excuse excuse, RequestContext context)
        {
            excuse.ExcuseStatusId = (int)ExcuseStatus.Taken;
            TamamServiceBroker.LeavesHandler.EditExcuse(excuse, context);
        }
        internal static void LeaveChangeToTaken(Guid leaveId, RequestContext context)
        {
            TamamServiceBroker.LeavesHandler.ChangeLeaveStatus(leaveId, LeaveStatus.Taken);
        }

        private Guid PayCodeGet()
        {
            if (ScheduleDay == null) return AttendanceCodes.PayCodeStatus.WorkOnUnScheduled;
            if (ScheduleDay.IsDayOff) return AttendanceCodes.PayCodeStatus.WorkOnWeekend;

            // leave ...
            //var leave = SystemBroker.LeavesHandler.GetLeave(RawAttendance.PersonId, RawAttendance.AttendanceDateTime.Date).Result;
            var leave = SystemBroker.LeavesHandler.GetLeave(RawAttendance.PersonId, ScheduleDay.Day).Result;
            if (leave != null)
            {
                AttendanceContext.LeaveChangeToTaken(leave.Id, requestContext);
                if (leave.LeaveMode == LeaveMode.FullDay)
                {
                    _AssociatedLeaveTypeId = leave.LeaveTypeId;
                    return AttendanceCodes.PayCodeStatus.WorkOnLeave;
                }
            };

            var excuses = TamamServiceBroker.LeavesHandler.SearchExcuses(new ExcuseSearchCriteria
            {
                StartDate = ScheduleDay.Day /*RawAttendance.AttendanceDateTime.Date*/,
                EndDate = ScheduleDay.Day/*RawAttendance.AttendanceDateTime.Date*/,
                Personnel = new List<Guid> { RawAttendance.PersonId },
                ExcuseStatuses = new List<int> { (int)ExcuseStatus.Approved, (int)ExcuseStatus.Taken },
                ActivePersonnelStatus = true,
                Duration = Shift != null ? Convert.ToDouble(Shift.Duration) : 0,
            }, requestContext).Result.Excuses;
            if (excuses.Count > 0)
            {
                foreach (var excuse in excuses)
                {
                    var duration = SystemBroker.SchedulesHandler.GetExcuseEffectiveTime(excuse, Shift).Result;
                    if (duration > 0)
                    {
                        if (excuse.ExcuseStatusId == (int)ExcuseStatus.Approved) AttendanceContext.ExcuseChangeToTaken(excuse, requestContext);   // mark that excuse as taken
                        return AttendanceCodes.PayCodeStatus.OnAway;
                    }
                }

            }

            // holiday ...
            var holidays = HolidaysGet();
            if (holidays != null && holidays.Count > 0)
            {
                _AssociatedHolidayName = holidays[0].Name;
                _AssociatedHolidayNameCultureVariant = holidays[0].NameCultureVariant;
                return AttendanceCodes.PayCodeStatus.WorkOnHoliday;
            }

            return AttendanceCodes.PayCodeStatus.NormalDay;
        }
        private Guid PayCodeValidate()
        {
            return Shift == null && PayCodeStatus == AttendanceCodes.PayCodeStatus.NormalDay ? AttendanceCodes.PayCodeStatus.WorkOnUnScheduled : PayCodeStatus;
        }

        internal static int? GetShiftDuration(Shift shift)
        {
            if (shift == null) return null;
            return (int)(shift.Duration * 60);

            //return Round( ( shift.EndTime.Value.AddDays( shift.IsNightShift ? 1 : 0 ) - Shift.StartTime.Value ).TotalMinutes );
        }
        internal int? GetShiftDuration()
        {
            return GetShiftDuration(this.Shift);
        }

        private Shift ShiftGetByProximity(List<Shift> list, AttendanceRawData raw)
        {
            //var earlyMargin = new TimeSpan(0, ShiftPolicy != null ? ShiftPolicy.ShiftStartMargin : 0, 0);
            //var lateMargin = new TimeSpan(0, ShiftPolicy != null ? ShiftPolicy.ShiftEndMargin : 0, 0);

            var margins = new Dictionary<Guid, XIntervals.TimeRange>();
            foreach (var shift in list)
            {
                var policyNative = SystemBroker.SchedulesHandler.GetShiftPolicy(this.Policies, shift).Result;
                var policy = new ShiftPolicy(policyNative);
                var x = new TimeSpan(0, policy.ShiftStartMargin, 0);
                var y = new TimeSpan(0, policy.ShiftEndMargin, 0);

                margins.Add(shift.Id, new XIntervals.TimeRange(x, y));
            }


            if (_AutoDetectedType)
                return XIntervals.Nearest(RawAttendance.AttendanceDateTime, Map(list), margins) as Shift;
            else
                return XIntervals.Nearest(RawAttendance.AttendanceDateTime, Map(list), margins, raw.Type == AttendanceEventType.Out) as Shift;
        }

        private List<Holiday> HolidaysGet()
        {
            return TamamServiceBroker.OrganizationHandler.GetHolidays(RawAttendance.PersonId, this.ScheduleDay.Day, this.ScheduleDay.Day, SystemRequestContext.Instance).Result;
        }

        private OverTimeSetting GetOverTimeSetting(OverTimePolicy policy, Guid payCode)
        {
            if (payCode == AttendanceCodes.PayCodeStatus.NormalDay || payCode == AttendanceCodes.PayCodeStatus.OnAway)
            {
                var setting = new OverTimeSetting { Maximum = policy.MaxOvertime, Rate = policy.OvertimeRate };
                return setting;
            }
            if (payCode == AttendanceCodes.PayCodeStatus.WorkOnWeekend)
            {
                var setting = new OverTimeSetting
                {
                    Maximum = policy.MaxWeekendOvertime,
                    Rate = policy.WeekendOvertimeRate
                };
                return setting;
            }
            if (payCode == AttendanceCodes.PayCodeStatus.WorkOnHoliday)
            {
                var setting = new OverTimeSetting
                {
                    Maximum = policy.MaxHolidayOvertime,
                    Rate = policy.HolidayOvertimeRate
                };
                return setting;
            }
            if (payCode == AttendanceCodes.PayCodeStatus.WorkOnLeave)
            {
                var setting = new OverTimeSetting { Maximum = policy.MaxLeaveOvertime, Rate = policy.LeaveOvertimeRate };
                return setting;
            }
            return new OverTimeSetting { Maximum = 0, Rate = 0 };
        }

        private int CalculateWorkingTime()
        {
            if (!this.ScheduleEvent.ActualIn.HasValue || !this.ScheduleEvent.ActualOut.HasValue) return 0;

            var IN = this.ScheduleEvent.ActualIn.Value;
            var OUT = this.ScheduleEvent.ActualOut.Value;
            var breaks = BreaksTotalDuration;

            #region breaks

            if (this.ScheduleEvent.AttendanceEvents != null && this.ScheduleEvent.AttendanceEvents.Count >= 2)
            {
                var AEBRs = this.ScheduleEvent.AttendanceEvents.Where(x => x.InTime != null && x.OutTime != null).ToList();
                foreach (var AEBR in AEBRs)
                {
                    var INBR = AEBR.InTime.Value;
                    var OUTBR = AEBR.OutTime.Value;

                    breaks += OUTBR > INBR ? (int)(OUTBR - INBR).TotalMinutes
                                           : (int)((new TimeSpan(24, 0, 0) - INBR.TimeOfDay).TotalMinutes + OUTBR.TimeOfDay.TotalMinutes);
                }
            }

            #endregion

            var duration = OUT > IN ? (OUT - IN).TotalMinutes : (new TimeSpan(24, 0, 0) - IN.TimeOfDay).TotalMinutes + OUT.TimeOfDay.TotalMinutes;
            var durationTotal = (int)duration - breaks;

            return durationTotal;
        }

        #endregion
        #region Helpers

        private List<XIntervals.ITimeRange> Map(List<Shift> from)
        {
            if (from == null) return null;

            var to = new List<XIntervals.ITimeRange>();
            foreach (var node in from)
            {
                to.Add((XIntervals.ITimeRange)node);
            }

            return to;
        }
        private ShiftPolicy GetShiftPolicy(List<Policy> policies, Shift shift)
        {
            var policy = SystemBroker.SchedulesHandler.GetShiftPolicy(policies, shift).Result;
            return policy == null ? null : new ShiftPolicy(policy);
        }

        private List<Shift> GetDayShifts(DateTime shiftDate, out ScheduleDay scheduleDay, out Schedule schedule)
        {
            schedule = null;
            scheduleDay = null;
            var schedules = TamamServiceBroker.SchedulesHandler.GetSchedules(shiftDate, shiftDate, new List<Guid> { this.RawAttendance.PersonId }, null, this.requestContext).Result;
            if (schedules == null) return new List<Shift>();
            // loop on all schedules and check the effective one for this person -effective schedule the one that has detail-
            //as the person my have two schedule in the same day -one inherited from department and one assign to the person-                          
            foreach (var scheduleItem in schedules)
            {
                var scheduleDetail = TamamServiceBroker.SchedulesHandler.GetScheduleDetails(scheduleItem, shiftDate, shiftDate, null, new List<Guid> { this.RawAttendance.PersonId }, this.requestContext).Result;
                if (scheduleDetail.Count > 0)
                {
                    schedule = scheduleItem;
                    scheduleDay = scheduleDetail.FirstOrDefault();
                    break;
                }
            }

            return scheduleDay != null ? scheduleDay.DayShifts.Select(sh => sh.Shift).ToList() : new List<Shift>();
        }

        private bool IsTimeInsideShift(DateTime time, XIntervals.ITimeRange shift, ShiftPolicy shiftPolicy, bool isOut)
        {
            var marginBefore = shiftPolicy != null ? new TimeSpan(0, shiftPolicy.ShiftEndMargin, 0) : new TimeSpan(0, 0, 0);
            var marginAfter = shiftPolicy != null ? new TimeSpan(0, shiftPolicy.ShiftStartMargin, 0) : new TimeSpan(0, 0, 0);

            return XIntervals.Nearest(time, new List<XIntervals.ITimeRange>() { shift }, marginBefore, marginAfter, isOut) != null;
        }
        #endregion
    }
}