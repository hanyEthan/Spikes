using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Definitions;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using XCore.Framework.Utilities;

namespace ADS.Tamam.Modules.Attendance.Handlers
{
    public partial class AttendanceHandler : IAttendanceHandler, ISystemAttendanceHandler
    {
        #region Properties

        private static readonly AttendanceApprovalWorkflowDefinition violationsWorkflowDefinition = new AttendanceApprovalWorkflowDefinition();
        private static readonly AttendanceManualEditApprovalWorkflowDefinition manualEditDefinition = new AttendanceManualEditApprovalWorkflowDefinition();

        #endregion
        #region models ...

        internal class TimeOff
        {
            public TimeSpan TimeStart { get; set; }
            public TimeSpan TimeEnd { get; set; }

            public enum TimeOffType { Excuse, Leave, Break, }

            /// <summary>
            /// offset related to expected timeoff start ( +ve for early, -ve for late )
            /// </summary>
            public int DurationCalculated { get; set; }

            /// <summary>
            /// total duration for the time window we are searching in
            /// </summary>
            public int Duration { get; set; }

            /// <summary>
            /// actual time taken for time off ( leave / excuse )
            /// </summary>
            public int Break { get; set; }

            /// <summary>
            /// expected total time for time off ( leave / excuse )
            /// </summary>
            public int BreakExpected { get; set; }

            /// <summary>
            /// the time someone had to work during an expected break time ...
            /// </summary>
            public int BreakEffective { get; set; }

            /// <summary>
            /// the time someone had to work during an expected break time ...
            /// </summary>
            public int WorkedDuringBreak { get; set; }

            ///// <summary>
            ///// in case the calculated duration includes the break duration ...
            ///// </summary>
            //public bool BreakIncludedWithDuration { get; set; }

            public TimeOffType? BreakType { get; set; }


            /// <summary>
            /// GUID for time offs semicolon separated  ( leave / excuse ) 
            /// </summary>
            public string BreakId { get; set; }

            public string BreakDetail { get; set; }
        }

        #endregion
        #region IAttendanceHandler

        public ExecutionResponse<bool> HandleAttendanceEvent(AttendanceRawData rawData, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                var aContext = new AttendanceContext(rawData, requestContext);
                if (aContext.RawAttendance.Type == AttendanceEventType.In)
                {
                    HandleInEvnet(aContext);
                }
                else if (aContext.RawAttendance.Type == AttendanceEventType.Out)
                {
                    HandleOutEvent(aContext);
                }
                context.Response.Set(ResponseState.Success, true);
            }, requestContext);

            return context.Response;
        }

        #endregion

        #region internals ...

        #region IN

        private void HandleInEvnet(AttendanceContext aContext)
        {
            if (aContext.ScheduleEvent == null)
            {
                HandleFirstIn(aContext);
            }
            else
            {
                HandleRepeatedIn(aContext);
            }
        }
        private void HandleFirstIn(AttendanceContext aContext)
        {
            if (aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.NormalDay && aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.WorkOnUnScheduled)
            {
                HandlePayCodeIn(aContext);
                return;
            }

            if (aContext.Shift == null)
            {
                #region SE

                var scheduleEvent = new ScheduleEvent
                {
                    EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                    PersonId = aContext.RawAttendance.PersonId,
                    ActualIn = aContext.RawAttendance.AttendanceDateTime,
                    InTimeRawId = aContext.RawAttendance.Id,
                    InTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    InManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                    InAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    AttendanceEvents = new List<AttendanceEvent>(),
                    TerminalId = aContext.RawAttendance.TerminalId,
                    //Location = aContext.RawAttendance.Location,
                };

                #endregion

                aContext.ScheduleEvent = scheduleEvent;
                aContext.ScheduleEventCreate();
            }
            else
            {
                #region SE
                var scheduleEvent = new ScheduleEvent
                {
                    EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                    ExpectedIn = aContext.Shift.IsFlexible ? aContext.Shift.StartTime : aContext.Shift.StartTime.GetValueOrDefault(),
                    ExpectedOut = aContext.Shift.IsFlexible ? aContext.Shift.EndTime : aContext.Shift.EndTime.GetValueOrDefault(),
                    PersonId = aContext.RawAttendance.PersonId,
                    ScheduleId = aContext.Schedule.Id,
                    ShiftId = aContext.Shift.Id,
                    ActualIn = aContext.RawAttendance.AttendanceDateTime,
                    InTimeRawId = aContext.RawAttendance.Id,
                    InTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    InManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                    InAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    AttendanceEvents = new List<AttendanceEvent>(),
                    TotalStatusId = AttendanceCodes.ScheduleEventStatus.CurrentlyIn
                };

                #endregion
                #region AE

                var attendanceEvent = new AttendanceEvent
                {
                    InTime = aContext.RawAttendance.AttendanceDateTime,
                    ScheduleEvent = scheduleEvent,
                    InTimeRawId = aContext.RawAttendance.Id,
                    InTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    InAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus
                };

                #endregion

                // Calculate If Early Or Late Or In Time
                bool LATEOFFSETRELATEDTOGRACE = aContext.ShiftPolicy.LateComeOffsetRelatedToGraceTime;
                var lateDuration = GetLateDuration(aContext);
                if (lateDuration > 0)   // late
                {
                    var hasTimeOff = HandleTimeOff(AttendanceEventType.In, scheduleEvent, attendanceEvent, aContext, aContext.RawAttendance.AttendanceDateTime.Date,
                        aContext.Shift.StartTime, aContext.RawAttendance.AttendanceDateTime, aContext.Shift.StartTime, aContext.Shift.EndTime, 0);

                    if (!hasTimeOff)
                    {
                        if (aContext.ShiftPolicy != null && aContext.ShiftPolicy.LateComeAbsentThreshold > 0 && Math.Abs(attendanceEvent.CalculatedDuration) >= aContext.ShiftPolicy.LateComeAbsentThreshold) attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.LateAbsent;   //Compare 'attendanceEvent.CalculatedDuration' not 'lateDuration' to take excuses into consideration.
                        else attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameLate;

                        attendanceEvent.CalculatedDuration = LATEOFFSETRELATEDTOGRACE ? lateDuration : attendanceEvent.CalculatedDuration;
                    }
                    else
                    {
                        if (attendanceEvent.CalculatedDuration == 0) attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameOnTime;
                        else if (attendanceEvent.CalculatedDuration > 0)
                        {
                            if (aContext.ShiftPolicy != null && aContext.ShiftPolicy.LateComeAbsentThreshold > 0 && Math.Abs(attendanceEvent.Duration) >= aContext.ShiftPolicy.LateComeAbsentThreshold) attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.LateAbsent;   //Compare 'attendanceEvent.CalculatedDuration' not 'lateDuration' to take excuses into consideration.
                            else attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameLate;
                        }
                        else if (attendanceEvent.CalculatedDuration < 0)
                        {
                            attendanceEvent.CalculatedDuration = Math.Abs(attendanceEvent.CalculatedDuration);
                            attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameEarly;
                        }
                    }
                }
                else if (lateDuration < 0)   // early
                {
                    //check for 'Early Come Grace' as the max value for early.
                    var absDuration = lateDuration * -1;
                    var maxDuration = (aContext.ShiftPolicy != null && absDuration > aContext.ShiftPolicy.EarlyComeGrace) ? aContext.ShiftPolicy.EarlyComeGrace : absDuration;

                    attendanceEvent.Duration = absDuration;
                    attendanceEvent.CalculatedDuration = maxDuration;
                    attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameEarly;

                    HandleTimeOff(AttendanceEventType.In, scheduleEvent, attendanceEvent, aContext, aContext.RawAttendance.AttendanceDateTime.Date,
                        aContext.Shift.StartTime, aContext.RawAttendance.AttendanceDateTime, aContext.Shift.StartTime, aContext.Shift.EndTime, 0);

                    attendanceEvent.Duration = Math.Abs(attendanceEvent.Duration);
                    attendanceEvent.CalculatedDuration = Math.Abs(attendanceEvent.CalculatedDuration);

                    if (aContext.ShiftPolicy != null && aContext.ShiftPolicy.EarlyComeAsOvertime)
                    {
                        var oldOv = scheduleEvent.CalculatedOvertime;

                        var overTime = maxDuration + oldOv;
                        var overTimeWithRate = XMath.Round((maxDuration + oldOv) * aContext.OverTime.Rate);

                        if (aContext.OverTime.Maximum > 0 && overTime >= aContext.OverTime.Maximum) overTime = aContext.OverTime.Maximum;
                        if (aContext.OverTime.Maximum > 0 && overTimeWithRate >= aContext.OverTime.Maximum) overTimeWithRate = aContext.OverTime.Maximum;

                        scheduleEvent.Overtime = overTime;
                        scheduleEvent.CalculatedOvertime = overTimeWithRate;
                        attendanceEvent.CalculatedDuration = (attendanceEvent.CalculatedDuration - overTime) + overTimeWithRate;  //maxDuration;
                    }
                }
                else
                {
                    attendanceEvent.Duration = 0;
                    var duration = aContext.Shift.IsFlexible ? 0 : XMath.Round((aContext.RawAttendance.AttendanceDateTime.TimeOfDay - aContext.Shift.StartTime.Value.TimeOfDay).TotalMinutes);
                    if (duration == 0)
                    {
                        attendanceEvent.Duration = duration;
                        if (!aContext.Shift.IsFlexible)
                        {
                            var hasTimeOff = HandleTimeOff(AttendanceEventType.In, scheduleEvent, attendanceEvent, aContext, aContext.RawAttendance.AttendanceDateTime.Date,
                           aContext.Shift.StartTime, aContext.RawAttendance.AttendanceDateTime, aContext.Shift.StartTime, aContext.Shift.EndTime, 0);
                            attendanceEvent.StatusInId = !hasTimeOff ? AttendanceCodes.AttendanceEventStatus.CameOnTime
                                                                    : AttendanceCodes.AttendanceEventStatus.CameEarly;
                            attendanceEvent.CalculatedDuration = Math.Abs(attendanceEvent.CalculatedDuration);
                        }
                        else
                        {
                            attendanceEvent.CalculatedDuration = duration;
                        }
                    }
                    else
                    {
                        var hasTimeOff = HandleTimeOff(AttendanceEventType.In, scheduleEvent, attendanceEvent, aContext,
                            aContext.RawAttendance.AttendanceDateTime.Date, aContext.Shift.StartTime, aContext.RawAttendance.AttendanceDateTime,
                            aContext.Shift.StartTime, aContext.Shift.EndTime, 0);

                        //attendanceEvent.Duration = duration;

                        if (hasTimeOff)
                        {
                            attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameEarly;
                            attendanceEvent.CalculatedDuration = Math.Abs(attendanceEvent.CalculatedDuration);
                        }
                        else
                        {
                            attendanceEvent.StatusInId = AttendanceCodes.AttendanceEventStatus.CameOnGrace;
                            attendanceEvent.CalculatedDuration = LATEOFFSETRELATEDTOGRACE ? lateDuration : attendanceEvent.CalculatedDuration;
                        }
                    }
                }

                scheduleEvent.AttendanceEvents.Add(attendanceEvent);
                scheduleEvent.StatusInId = attendanceEvent.StatusInId;
                scheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                //scheduleEvent.Location = aContext.RawAttendance.Location;
                aContext.ScheduleEvent = scheduleEvent;
                aContext.ScheduleEventCreate();
            }
        }
        private void HandlePayCodeIn(AttendanceContext aContext)
        {
            //If service added schedule event, remove it and start from scratch.
            if (aContext.ScheduleEvent != null &&
                (
                    aContext.ScheduleEvent.PayCodeStatusId == AttendanceCodes.PayCodeStatus.OnWeekend ||
                    aContext.ScheduleEvent.PayCodeStatusId == AttendanceCodes.PayCodeStatus.OnLeave ||
                    aContext.ScheduleEvent.PayCodeStatusId == AttendanceCodes.PayCodeStatus.OnHoliday ||
                    aContext.ScheduleEvent.PayCodeStatusId == AttendanceCodes.PayCodeStatus.OnAway

                    )
                )
            {
                aContext.ScheduleEventDelete();
            }
            if (aContext.ScheduleEvent != null && aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Attended)
            {
                //'In Time' is always inside the 'aContext.Shift' courtesy of 'CalculationHelper.GetCorrespondingShift'
                /*Consider the period between the last out and this in transaction was Exit During Work*/
                //Calculate Duration
                var lastOut = aContext.ScheduleEvent.ActualOut.Value;
                var lastOutId = aContext.ScheduleEvent.OutTimeRawId;
                var outDuration = XMath.Round((aContext.RawAttendance.AttendanceDateTime.TimeOfDay - lastOut.TimeOfDay).TotalMinutes);
                //var outDurationAfterExcuse = CheckForExcuse( aContext , aContext.RawAttendance.AttendanceDateTime.Date , lastOut , aContext.RawAttendance.AttendanceDateTime );

                var startDate = lastOut;
                var EndDate = aContext.RawAttendance.AttendanceDateTime;

                if (aContext.Shift.IsNightShift)
                {
                    var adjustedStart = startDate.AddHours(aContext.NightShiftDelta);
                    var adjustedEnd = EndDate.AddHours(aContext.NightShiftDelta);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, adjustedStart.Hour, adjustedStart.Minute, adjustedStart.Second);
                    EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, adjustedEnd.Hour, adjustedEnd.Minute, adjustedEnd.Second);

                }

                var timeOff = CheckForTimeOffBreak(aContext, aContext.ScheduleEvent, aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date, startDate, EndDate);
                aContext.ScheduleEvent.OffHours = aContext.ScheduleEvent.OffHours.HasValue ? aContext.ScheduleEvent.OffHours : 0;
                aContext.ScheduleEvent.OffHours += timeOff.BreakExpected;
                aContext.ScheduleEvent.OffHoursCalculated = timeOff.WorkedDuringBreak;

                //aContext.ScheduleEvent.CalculatedHours += timeOff.BreakEffective;

                var outDurationAfterExcuse = timeOff.DurationCalculated;

                //Change total status to Currently In
                aContext.ScheduleEvent.TotalStatusId = AttendanceCodes.ScheduleEventStatus.CurrentlyIn;
                //Remove Last Out.
                aContext.ScheduleEvent.ActualOut = null;
                aContext.ScheduleEvent.OutTimeRawId = new Guid();
                aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                //Remove Early Leave Record.
                RemoveEarlyLeave(aContext);
                AttendanceEvent exit = new AttendanceEvent
                {
                    ScheduleEventId = aContext.ScheduleEvent.Id,
                    OutTime = aContext.RawAttendance.AttendanceDateTime,
                    OutTimeRawId = aContext.RawAttendance.Id,
                    OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    InTime = lastOut,

                    InTimeRawId = lastOutId,//aContext.ScheduleEvent.InTimeRawId,
                    InTimeIsOriginal = aContext.ScheduleEvent.InTimeIsOriginal,
                    InAttendanceSource = aContext.ScheduleEvent.InAttendanceSource,
                    ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,

                    Duration = outDuration,
                    CalculatedDuration = outDurationAfterExcuse,
                    StatusOutId = AttendanceCodes.AttendanceEventStatus.ExitDuringWork,
                    StatusInId = AttendanceCodes.AttendanceEventStatus.ExitDuringWork,
                    BreakId = timeOff.BreakId
                };
                //aContext.ScheduleEvent.AttendanceEvents.Add ( exit );
                aContext.AttendanceEventAdd(exit);
                aContext.ScheduleEventUpdate();
            }
            else
            {
                #region SE

                var scheduleEvent = new ScheduleEvent
                {
                    EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                    ExpectedIn = aContext.Shift != null && !aContext.Shift.IsFlexible ? aContext.Shift.StartTime.GetValueOrDefault() : (DateTime?)null,
                    ExpectedOut = aContext.Shift != null && !aContext.Shift.IsFlexible ? aContext.Shift.EndTime.GetValueOrDefault() : (DateTime?)null,
                    PersonId = aContext.RawAttendance.PersonId,
                    ScheduleId = aContext.Schedule.Id,
                    ShiftId = aContext.Shift != null ? aContext.Shift.Id : Guid.Empty,
                    ActualIn = aContext.RawAttendance.AttendanceDateTime,
                    InTimeRawId = aContext.RawAttendance.Id,
                    InTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    InManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                    InAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    AttendanceEvents = new List<AttendanceEvent>(),
                    TotalStatusId = aContext.Shift != null ? AttendanceCodes.ScheduleEventStatus.CurrentlyIn : AttendanceCodes.ScheduleEventStatus.MissedOutPunch
                };

                #endregion

                var hours = 0;
                if (aContext.Shift != null)
                {
                    hours = aContext.GetShiftDuration().Value;
                    scheduleEvent.ShiftId = aContext.Shift.Id;
                }

                scheduleEvent.Hours = hours;
                scheduleEvent.CalculatedHours = hours;
                scheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                //scheduleEvent.Location = aContext.RawAttendance.Location;
                aContext.ScheduleEvent = scheduleEvent;
                aContext.ScheduleEventCreate();
            }
        }
        private void HandleRepeatedIn(AttendanceContext aContext)
        {
            //if ( CheckForWorngData ( aContext ) ) return;   //Check for wrong data order, if so mark record as dirty

            #region CurrentlyIn

            if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn)
            {
                var lastInRawAttendance = aContext.SERawAttendances.Where(x => x.Type == AttendanceEventType.In).OrderBy(x => x.AttendanceDateTime).LastOrDefault();
                if (lastInRawAttendance != null)
                {
                    var timeDiff = aContext.RawAttendance.AttendanceDateTime - lastInRawAttendance.AttendanceDateTime;
                    if (timeDiff.TotalSeconds > aContext.DuplicatePunchThreshold)
                    {
                        AttendanceEvent duplicatePunchAE = new AttendanceEvent
                        {
                            ScheduleEventId = aContext.ScheduleEvent.Id,
                            ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                            InTime = aContext.RawAttendance.AttendanceDateTime,
                            InTimeRawId = aContext.RawAttendance.Id,
                            InTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                            InAttendanceSource = aContext.RawAttendance.AttendanceSource,
                        };
                        aContext.AttendanceEventAdd(duplicatePunchAE);
                        aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                        //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                        aContext.ScheduleEventUpdate();
                    }
                }

                return;/*DO NOTHING as 2nd IN*/
            }



            #region commented
            //{
            // TODO : need to handle also MissedOutPunch to cancel unused ManualEdit WFs..
            //    if (aContext.RawAttendance.ManualAttendanceStatus == ManualAttendanceStatus.Pending || aContext.RawAttendance.ManualAttendanceStatus == ManualAttendanceStatus.Approved)
            //    {
            //        foreach (var item in aContext.RawAttendance.DataHistoryItems)
            //        {
            //            CancelManualEditWorkflow(item);
            //        }
            //    }
            //    return;
            //}
            #endregion
            #endregion

            if (aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.NormalDay && aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.WorkOnUnScheduled)
            {
                HandlePayCodeIn(aContext);
                return;
            }

            if (aContext.Shift == null)
            {

                #region SE

                var scheduleEvent = new ScheduleEvent
                {
                    EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                    PersonId = aContext.RawAttendance.PersonId,
                    ActualIn = aContext.RawAttendance.AttendanceDateTime,
                    InTimeRawId = aContext.RawAttendance.Id,
                    InTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    InManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                    InAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    AttendanceEvents = new List<AttendanceEvent>(), 
                    TerminalId = aContext.RawAttendance.TerminalId,
                    //Location = aContext.RawAttendance.Location,
                };

                #endregion

                aContext.ScheduleEvent = scheduleEvent;
                aContext.ScheduleEventCreate();
            }

            #region Absent

            if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Absent)
            {
                //Came after he was considered absent.
                //Remove the absent record the start calculation from the start.
                aContext.ScheduleEventDelete();
                HandleFirstIn(aContext);
                return;
            }

            #endregion
            #region Attended

            if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Attended)
            {
                //'In Time' is always inside the 'aContext.Shift' courtesy of 'CalculationHelper.GetCorrespondingShift'
                /*Consider the period between the last out and this in transaction was Exit During Work*/
                //Calculate Duration
                var lastOut = aContext.ScheduleEvent.ActualOut.Value;
                var lastOutTimeRawId = aContext.ScheduleEvent.OutTimeRawId;
                var outDuration = XMath.Round((aContext.RawAttendance.AttendanceDateTime.TimeOfDay - lastOut.TimeOfDay).TotalMinutes);

                var startDate = lastOut;
                var EndDate = aContext.RawAttendance.AttendanceDateTime;

                if (aContext.Shift.IsNightShift)
                {
                    var adjustedStart = startDate.AddHours(aContext.NightShiftDelta);
                    var adjustedEnd = EndDate.AddHours(aContext.NightShiftDelta);
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, adjustedStart.Hour, adjustedStart.Minute, adjustedStart.Second);
                    EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, adjustedEnd.Hour, adjustedEnd.Minute, adjustedEnd.Second);

                }

                var timeOff = CheckForTimeOffBreak(aContext, aContext.ScheduleEvent, aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date, startDate, EndDate);
                aContext.ScheduleEvent.OffHours = aContext.ScheduleEvent.OffHours.HasValue ? aContext.ScheduleEvent.OffHours : 0;
                aContext.ScheduleEvent.OffHours += timeOff.BreakExpected;
                aContext.ScheduleEvent.OffHoursCalculated = timeOff.WorkedDuringBreak;

                var outDurationAfterExcuse = timeOff.DurationCalculated;

                //Change total status to Currently In
                aContext.ScheduleEvent.TotalStatusId = AttendanceCodes.ScheduleEventStatus.CurrentlyIn;
                //Remove Last Out.
                aContext.ScheduleEvent.ActualOut = null;
                aContext.ScheduleEvent.StatusOutId = null;
                aContext.ScheduleEvent.StatusOut = null;

                aContext.ScheduleEvent.OutTimeRawId = new Guid();
                //Remove Early Leave Record.
                RemoveEarlyLeave(aContext);
                //Clear Working Hours.
                aContext.ScheduleEvent.Hours = 0;
                aContext.ScheduleEvent.CalculatedHours = timeOff.BreakEffective;
                aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;

                AttendanceEvent exit = new AttendanceEvent
                {
                    ScheduleEventId = aContext.ScheduleEvent.Id,
                    OutTime = aContext.RawAttendance.AttendanceDateTime,
                    OutTimeRawId = aContext.RawAttendance.Id,
                    OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,

                    InTime = lastOut,
                    InTimeRawId = lastOutTimeRawId,
                    InTimeIsOriginal = aContext.ScheduleEvent.OutTimeIsOriginal,
                    InAttendanceSource = aContext.ScheduleEvent.OutAttendanceSource,
                    Duration = outDuration,
                    CalculatedDuration = outDurationAfterExcuse,
                    StatusOutId = AttendanceCodes.AttendanceEventStatus.ExitDuringWork,
                    StatusInId = AttendanceCodes.AttendanceEventStatus.ExitDuringWork,
                    BreakId = timeOff.BreakId
                };
                aContext.AttendanceEventAdd(exit);
                aContext.ScheduleEventUpdate();
            }

            #endregion
        }
        private bool CheckForWorngData(AttendanceContext aContext)
        {
            if (aContext.ScheduleEvent == null)
                return false;

            var lastIn = aContext.ScheduleEvent.ActualIn;
            if (lastIn.HasValue && aContext.RawAttendance.AttendanceDateTime.TimeOfDay < lastIn.Value.TimeOfDay)
            {
                aContext.ScheduleEvent.IsDirty = true;
                aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                aContext.ScheduleEventUpdate();
                return true;
            }
            return false;

        }
        private void RemoveEarlyLeave(AttendanceContext aContext)
        {
            var earlyLeave = aContext.ScheduleEvent.AttendanceEvents.FirstOrDefault(el => el.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftEarly || el.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftOnGrace);
            if (earlyLeave != null)
                aContext.AttendanceEventDelete(earlyLeave);

        }
        private int GetLateDuration(AttendanceContext aContext)
        {
            bool LATEOFFSETRELATEDTOGRACE = aContext.ShiftPolicy.LateComeOffsetRelatedToGraceTime;

            if (aContext.Shift == null || aContext.Shift.IsFlexible) return 0;

            var lateGrace = aContext.ShiftPolicy != null ? aContext.ShiftPolicy.LateComeGrace : 0;
            var shiftStart = aContext.Shift.StartTime.Value.TimeOfDay;
            var shiftStartWithGrace = aContext.Shift.StartTime.Value.AddMinutes(lateGrace).TimeOfDay;
            var inTime = aContext.RawAttendance.AttendanceDateTime.TimeOfDay;
            if (aContext.Shift.IsNightShift)
            {
                inTime = aContext.RawAttendance.AttendanceDateTime.AddHours(aContext.NightShiftDelta).TimeOfDay;
                shiftStart = aContext.Shift.StartTime.Value.AddHours(aContext.NightShiftDelta).AddDays(-1).TimeOfDay;
                shiftStartWithGrace = aContext.Shift.StartTime.Value.AddHours(aContext.NightShiftDelta).AddDays(-1).AddMinutes(lateGrace).TimeOfDay;
                inTime = inTime.Hours == 0 ? inTime.Add(new TimeSpan(24, 0, 0)) : inTime;
                shiftStart = shiftStart.Hours == 0 ? shiftStart.Add(new TimeSpan(24, 0, 0)) : shiftStart;
                shiftStartWithGrace = shiftStartWithGrace.Hours == 0 ? shiftStartWithGrace.Add(new TimeSpan(24, 0, 0)) : shiftStartWithGrace;
            }

            if (inTime < shiftStart) return XMath.Round((shiftStart - inTime).TotalMinutes) * -1;   // Early

            if (inTime > shiftStartWithGrace) return XMath.Round((inTime - (LATEOFFSETRELATEDTOGRACE ? shiftStartWithGrace : shiftStart)).TotalMinutes);   // late after grace.
            return 0;
        }

        #endregion
        #region OUT

        private void HandleOutEvent(AttendanceContext aContext)
        {
            if (aContext.ScheduleEvent != null)
            {
                HandleRepeatedOut(aContext);
            }
            else
            {
                HandleFirstOut(aContext);
            }
        }
        private void HandleFirstOut(AttendanceContext aContext)
        {
            //Search for 'Currently In' Schedule Event in the last 24 hours.
            var yesterdayEvents = aContext.ScheduleEventsGet(aContext.RawAttendance.PersonId, aContext.RawAttendance.AttendanceDateTime.Date.AddDays(-1));
            var yesterdayEventCurrentlyIn = yesterdayEvents.FirstOrDefault(x => x.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn || x.TotalStatusId == AttendanceCodes.ScheduleEventStatus.MissedOutPunch);
            if (yesterdayEventCurrentlyIn != null)
            {
                if (yesterdayEventCurrentlyIn.Shift != null && yesterdayEventCurrentlyIn.Shift.IsNightShift)
                {
                    //Check if the raw attendance time is schedule time before add it to shift
                    var shiftStartMargin = aContext.ShiftPolicy != null ? aContext.ShiftPolicy.ShiftStartMargin : 0;
                    var shiftEndMargin = aContext.ShiftPolicy != null ? aContext.ShiftPolicy.ShiftEndMargin : 0;
                    var shiftStart = aContext.RawAttendance.Type == AttendanceEventType.In ? yesterdayEventCurrentlyIn.Shift.StartTime.Value.AddMinutes(shiftStartMargin).TimeOfDay : yesterdayEventCurrentlyIn.Shift.StartTime.Value.TimeOfDay;
                    var shiftEnd = aContext.RawAttendance.Type == AttendanceEventType.Out ? yesterdayEventCurrentlyIn.Shift.EndTime.Value.AddMinutes(shiftEndMargin).TimeOfDay : yesterdayEventCurrentlyIn.Shift.EndTime.Value.TimeOfDay;
                    if (aContext.RawAttendance.AttendanceDateTime.TimeOfDay >= shiftStart
                        || aContext.RawAttendance.AttendanceDateTime.TimeOfDay <= shiftEnd)
                    {
                        aContext.ScheduleEvent = yesterdayEventCurrentlyIn;
                        aContext.Shift = yesterdayEventCurrentlyIn.Shift;
                        aContext.PayCodeStatus = yesterdayEventCurrentlyIn.PayCodeStatusId != null ? yesterdayEventCurrentlyIn.PayCodeStatusId.Value : aContext.PayCodeStatus;
                        HandleRepeatedOut(aContext);
                        return;
                    }
                }
            }
            if (aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.NormalDay && aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.WorkOnUnScheduled)
            {
                HandlePayCodeFirstOut(aContext);
                return;
            }
            if (aContext.Shift == null)
            {
                var scheduleEvent = new ScheduleEvent
                {
                    EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                    PersonId = aContext.RawAttendance.PersonId,
                    ActualOut = aContext.RawAttendance.AttendanceDateTime,
                    OutTimeRawId = aContext.RawAttendance.Id,
                    OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                    OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    AttendanceEvents = new List<AttendanceEvent>(),
                    TerminalId = aContext.RawAttendance.TerminalId,
                    //Location = aContext.RawAttendance.Location,
                };
                aContext.ScheduleEvent = scheduleEvent;
                aContext.ScheduleEventCreate();
            }
            else
            {
                //TO DO, Handle 2 days shift.
                /* No Transactions in this day */
                /* Check the past day for transactions */
                /* No Transaction in the past 24 hours */
                /* Insert New Transaction with Abnormal status */
                var scheduleEvent = new ScheduleEvent
                {
                    EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                    ExpectedIn = aContext.Shift.IsFlexible ? aContext.Shift.StartTime : aContext.Shift.StartTime.GetValueOrDefault(),
                    ExpectedOut = aContext.Shift.IsFlexible ? aContext.Shift.EndTime : aContext.Shift.EndTime.GetValueOrDefault(),
                    PersonId = aContext.RawAttendance.PersonId,
                    ScheduleId = aContext.Schedule.Id,
                    ShiftId = aContext.Shift.Id,
                    ActualOut = aContext.RawAttendance.AttendanceDateTime,
                    OutTimeRawId = aContext.RawAttendance.Id,
                    OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                    OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                    OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                    AttendanceEvents = new List<AttendanceEvent>(),
                    TotalStatusId = AttendanceCodes.ScheduleEventStatus.MissedInPunch,
                    TerminalId = aContext.RawAttendance.TerminalId,
                    //Location = aContext.RawAttendance.Location,
                };
                aContext.ScheduleEvent = scheduleEvent;
                aContext.ScheduleEventCreate();
            }
        }
        private void HandleRepeatedOut(AttendanceContext aContext)
        {
            if (aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.NormalDay && aContext.PayCodeStatus != AttendanceCodes.PayCodeStatus.WorkOnUnScheduled)
            {
                HandlePayCodeOut(aContext);
                return;
            }

            if (aContext.Shift == null)
            {
                if (aContext.ScheduleEvent == null)
                {
                    var scheduleEvent = new ScheduleEvent
                    {
                        EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                        PersonId = aContext.RawAttendance.PersonId,
                        ActualOut = aContext.RawAttendance.AttendanceDateTime,
                        OutTimeRawId = aContext.RawAttendance.Id,
                        OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                        OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                        OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                        AttendanceEvents = new List<AttendanceEvent>(), 
                        TerminalId = aContext.RawAttendance.TerminalId,
                        //Location = aContext.RawAttendance.Location,
                    };
                    aContext.ScheduleEvent = scheduleEvent;
                    aContext.ScheduleEventCreate();
                }
                else
                {
                    if (aContext.ScheduleEvent.Shift == null)
                    {
                        aContext.ScheduleEvent.ActualOut = aContext.RawAttendance.AttendanceDateTime;
                        aContext.ScheduleEvent.OutTimeRawId = aContext.RawAttendance.Id;
                        aContext.ScheduleEvent.OutTimeIsOriginal = aContext.RawAttendance.IsOriginal;
                        aContext.ScheduleEvent.OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus;
                        aContext.ScheduleEvent.OutAttendanceSource = aContext.RawAttendance.AttendanceSource;
                        aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                        //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                        aContext.ScheduleEventUpdate();
                    }
                    else
                    {
                        var scheduleEvent = new ScheduleEvent
                        {
                            EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                            PersonId = aContext.RawAttendance.PersonId,
                            ActualOut = aContext.RawAttendance.AttendanceDateTime,
                            OutTimeRawId = aContext.RawAttendance.Id,
                            OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                            OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                            OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                            AttendanceEvents = new List<AttendanceEvent>(),
                            TerminalId = aContext.RawAttendance.TerminalId,
                            //Location = aContext.RawAttendance.Location,
                        };
                        aContext.ScheduleEvent = scheduleEvent;
                        aContext.ScheduleEventCreate();
                    }
                }
            }
            else
            {
                //Check current Transaction status.

                #region Currently In or Attended Or Missed OutPunch

                //If currently in complete the transaction.
                if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn ||
                    aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Attended ||
                    aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.MissedOutPunch)
                {
                    //In case on 'Attended' that means the last action was out, we need to remove the last out action.
                    if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Attended)
                    {
                        //in case on multi-out we should check if the last out had overtime and we should subtract it from the total overtime.
                        var lateLeave = GetLateLeave(aContext);

                        if (lateLeave != null)
                        {
                            aContext.ScheduleEvent.CalculatedOvertime -= lateLeave.CalculatedDuration;
                            aContext.ScheduleEvent.CalculatedOvertime = Math.Max(0, aContext.ScheduleEvent.CalculatedOvertime);
                        }
                        
                        RemoveLastOut(aContext);

                        // TODO : this is wrong, instead of marking this value to be 0, we should get its previous value before the last out we just deleted, 
                        // but in order to do that, we need to store that value in a temp place, because it will be hard to recreate it (need the raw data of the deleted last out)
                        // other options, is to not maintain any needed breaks values in the CalculatedHours, but in a temporary field (stored), in the first place.
                        aContext.ScheduleEvent.Hours = 0;
                        aContext.ScheduleEvent.CalculatedHours = 0;
                    }
                    //Update schedule Event status to Attended.
                    aContext.ScheduleEvent.TotalStatusId = AttendanceCodes.ScheduleEventStatus.Attended;
                    //To calculate working hours we should 1st sum all out duration(s) and any early come value.
                    //Working hours is the intersection between the duration of (shift_start - shift_end ) and the duration of(actual_Out - actual_In).
                    //Working Hours = (actualOut - actualIn) - OutDurations - EarlyCome

                    var workingHours = XMath.Round((aContext.RawAttendance.AttendanceDateTime - aContext.ScheduleEvent.ActualIn.Value).TotalMinutes -
                                       GetOutDurations(aContext) - GetEarlyDuration(aContext));

                    workingHours = Math.Max(workingHours, 0);
                    var shiftDuration = aContext.GetShiftDuration().Value;
                    //Check if left early or late or in time.
                    var diff = aContext.Shift.IsFlexible ? 0 : XMath.Round((aContext.RawAttendance.AttendanceDateTime.TimeOfDay - aContext.Shift.EndTime.Value.TimeOfDay).TotalMinutes);
                    if (aContext.Shift.IsNightShift)
                    {
                        var NormalizeOut = aContext.RawAttendance.AttendanceDateTime.AddHours(aContext.NightShiftDelta).AddDays(-1);
                        var Normalizeend = aContext.Shift.EndTime.Value.AddHours(aContext.NightShiftDelta).AddDays(-1);
                        diff = XMath.Round((NormalizeOut.TimeOfDay - Normalizeend.TimeOfDay).TotalMinutes);
                    }

                    //check for late come record.
                    var lateCome = aContext.ScheduleEvent.AttendanceEvents.Where(l => l.StatusInId == AttendanceCodes.AttendanceEventStatus.CameLate
                                                                                      || l.StatusInId == AttendanceCodes.AttendanceEventStatus.CameOnGrace
                                                                                      || l.StatusInId == AttendanceCodes.AttendanceEventStatus.LateAbsent).FirstOrDefault();
                    var lateDurationIN = lateCome != null ? Math.Abs(lateCome.Duration) : 0;
                    var lateCalculatedDuration = lateCome != null ? lateCome.CalculatedDuration : 0;
                    var hasEarlyTimeOff = false;
                    #region left late (Overtime)

                    if (diff > 0)
                    {
                        AttendanceEvent leftLateEvent = new AttendanceEvent
                        {
                            ScheduleEventId = aContext.ScheduleEvent.Id,
                            OutTime = aContext.RawAttendance.AttendanceDateTime,
                            OutTimeRawId = aContext.RawAttendance.Id,
                            OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                            OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                            ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                            Duration = diff,
                            StatusOutId = AttendanceCodes.AttendanceEventStatus.LeftLate
                        };

                        var newDiff = diff;
                        if (aContext.ShiftPolicy != null && aContext.ShiftPolicy.LateComeDeductFromOvertime)
                        {

                            if (lateCome != null)
                            {


                                if (aContext.ShiftPolicy.EarlyLeaveOffsetRelatedToGraceTime)
                                {
                                    // get previous breaks (SS-IN)
                                    // get intersection with GR (if intersected => then omit the Grace handling from the ILF)
                                    // BUT ... later when we handle partial intersection between graces and excuses, we need to modify in here to take this effect ...
                                    // compare ILF with (XDs + GR)

                                    var TMP_SE = new ScheduleEvent();
                                    var TMP_AE = new AttendanceEvent();

                                    hasEarlyTimeOff = HandleTimeOff(AttendanceEventType.In, TMP_SE, TMP_AE, aContext, lateCome.InTime.Value.Date,
                                        aContext.Shift.StartTime, lateCome.InTime.Value, aContext.Shift.StartTime, aContext.Shift.EndTime, 0);

                                    var ILF = lateCome.CalculatedDuration;  // in late effective
                                    var GRC = aContext.ShiftPolicy.LateComeGrace;  // Grace
                                    var XD = TMP_SE.CalculatedHours;  // break duration
                                    var LC = lateCome.Duration;  // late come
                                    var LL = newDiff;  // late leave
                                    var GRCF = lateCome.StatusInId == AttendanceCodes.AttendanceEventStatus.CameOnGrace ? LC : GRC;  // effective grace to be used for deduction (not attended time during grace)
                                    var LateLeaveDurationAfterGrace = Math.Max(0, LL - (hasEarlyTimeOff ? (ILF == (LC - GRCF - XD) ? GRCF : 0) : GRCF));
                                    XMath.DeductBalance(ref LateLeaveDurationAfterGrace, ref ILF);
                                    lateCome.CalculatedDuration = ILF;
                                    newDiff = LateLeaveDurationAfterGrace;

                                }
                                else
                                {
                                    XMath.DeductBalance( ref newDiff, ref lateCalculatedDuration);
                                    lateCome.CalculatedDuration = lateCalculatedDuration;
                                }
                                /////////////////////////////////////////////////////////////////////////

                            }
                        }

                        //deduct overtime from working hours.

                        workingHours -= newDiff;
                        aContext.ScheduleEvent.ActualOut = aContext.RawAttendance.AttendanceDateTime;
                        aContext.ScheduleEvent.OutTimeRawId = aContext.RawAttendance.Id;
                        aContext.ScheduleEvent.OutTimeIsOriginal = aContext.RawAttendance.IsOriginal;
                        aContext.ScheduleEvent.OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus;
                        aContext.ScheduleEvent.OutAttendanceSource = aContext.RawAttendance.AttendanceSource;
                        aContext.ScheduleEvent.Hours = workingHours;
                        aContext.ScheduleEvent.CalculatedHours += workingHours;

                        var oldOV = aContext.ScheduleEvent.CalculatedOvertime;

                        if (aContext.OverTimePolicy != null && aContext.OverTimePolicy.IsRelatedToWorkingHours)
                        {

                            if (aContext.ScheduleEvent.CalculatedHours < shiftDuration)
                            {
                                var delta = shiftDuration - aContext.ScheduleEvent.CalculatedHours;
                                var newDelata = delta;
                                XMath.DeductBalance( ref newDelata, ref newDiff);

                                aContext.ScheduleEvent.Hours += (delta - newDelata);
                                aContext.ScheduleEvent.CalculatedHours += (delta - newDelata);
                            }
                        }

                        if (aContext.ScheduleEvent.CalculatedHours > shiftDuration)
                        {
                            aContext.ScheduleEvent.CalculatedHours = shiftDuration;
                        }


                        // check for "Out Offset for Overtime" setting ...
                        if (aContext.ShiftPolicy != null)
                        {
                            newDiff -= aContext.ShiftPolicy.OutOffsetForOvertime;
                            newDiff = Math.Max(newDiff, 0);
                        }

                        aContext.ScheduleEvent.Overtime += newDiff;
                        var lateOvertime = XMath.Round(newDiff * aContext.OverTime.Rate);

                        var total = oldOV + lateOvertime;
                        if (aContext.OverTime.Maximum > 0 && total >= aContext.OverTime.Maximum) total = aContext.OverTime.Maximum;

                        aContext.ScheduleEvent.CalculatedOvertime = total;
                        var hasTimeOff = HandleTimeOff(AttendanceEventType.Out, aContext.ScheduleEvent, leftLateEvent, aContext, aContext.RawAttendance.AttendanceDateTime.Date, aContext.RawAttendance.AttendanceDateTime, aContext.Shift.EndTime, aContext.Shift.StartTime, aContext.Shift.EndTime, lateDurationIN);
                        if (hasTimeOff)
                        {
                            var totalWorkingTime = aContext.ScheduleEvent.CalculatedHours;

                            if (totalWorkingTime > shiftDuration)
                            {
                                aContext.ScheduleEvent.CalculatedHours = shiftDuration;
                            }

                            // WARNING : THIS AREA IS BEING HANDLED INCORRECTLY, WE SHOULD BE MOVING IT INTO THE START OF THIS ROUTINE ( LATE LEAVE )
                            if (aContext.ShiftPolicy != null && aContext.ShiftPolicy.LateComeDeductFromOvertime)
                            {
                                //check for late come record.                               
                                if (lateCome != null)
                                {
                                    if (hasEarlyTimeOff) lateDurationIN = lateCalculatedDuration;
                                    var lateDurationOut = Math.Abs(leftLateEvent.CalculatedDuration);
                                    XMath.DeductBalance( ref lateDurationOut, ref lateDurationIN);

                                    leftLateEvent.CalculatedDuration = lateDurationOut;
                                    lateCome.CalculatedDuration = Math.Max(0, lateDurationIN);
                                }
                            }

                        }
                        else
                        {
                            leftLateEvent.CalculatedDuration = lateOvertime;
                            leftLateEvent.Duration = Math.Abs(leftLateEvent.Duration);
                        }

                        aContext.AttendanceEventAdd(leftLateEvent);
                        aContext.ScheduleEvent.StatusOutId = leftLateEvent.StatusOutId;
                        aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                        //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                        aContext.ScheduleEventUpdate();
                    }
                    #endregion
                    #region left early

                    else if (diff < 0)   //left early
                    {
                        //Update actual out with the current date event value.
                        //update working hours.
                        //add new attendance event with left early status.
                        aContext.ScheduleEvent.ActualOut = aContext.RawAttendance.AttendanceDateTime;
                        aContext.ScheduleEvent.OutTimeRawId = aContext.RawAttendance.Id;
                        aContext.ScheduleEvent.OutTimeIsOriginal = aContext.RawAttendance.IsOriginal;
                        aContext.ScheduleEvent.OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus;
                        aContext.ScheduleEvent.OutAttendanceSource = aContext.RawAttendance.AttendanceSource;

                        var earlyLeaveGracePolicy = aContext.ShiftPolicy != null ? aContext.ShiftPolicy.EarlyLeaveGrace : 0;

                        var overtime = aContext.ScheduleEvent.Overtime;
                        var earlyOffsetActual = diff * -1;
                        var earlyOffsetEffective = earlyOffsetActual;
                        var attendanceEventEarlyLeave = new AttendanceEvent
                        {
                            ScheduleEventId = aContext.ScheduleEvent.Id,
                            OutTime = aContext.RawAttendance.AttendanceDateTime,
                            OutTimeRawId = aContext.RawAttendance.Id,
                            OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                            OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                            ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus
                        };

                        var hasBreak = HandleTimeOff(AttendanceEventType.Out, aContext.ScheduleEvent, attendanceEventEarlyLeave, aContext, aContext.RawAttendance.AttendanceDateTime.Date, aContext.RawAttendance.AttendanceDateTime, aContext.Shift.EndTime, aContext.Shift.StartTime, aContext.Shift.EndTime, lateDurationIN);
                        if (hasBreak)
                        {
                            attendanceEventEarlyLeave.Duration = earlyOffsetActual;
                            earlyOffsetEffective = attendanceEventEarlyLeave.CalculatedDuration * (hasBreak ? 1 : -1);
                            attendanceEventEarlyLeave.StatusOutId = earlyOffsetEffective < 0 ? AttendanceCodes.AttendanceEventStatus.LeftLate
                                                                  : earlyOffsetEffective > 0 ? AttendanceCodes.AttendanceEventStatus.LeftEarly
                                                                                             : AttendanceCodes.AttendanceEventStatus.LeftOnTime;

                            // if came late and left late, update the late in to remove the late leave from it ( if the policy allows )
                            if (attendanceEventEarlyLeave.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftLate && aContext.ShiftPolicy != null && aContext.ShiftPolicy.LateComeDeductFromOvertime)
                            {
                                //check for late come record.                               
                                if (lateCome != null)
                                {
                                    var lateDurationOut = Math.Abs(earlyOffsetEffective);
                                    if (aContext.ShiftPolicy.EarlyLeaveOffsetRelatedToGraceTime)
                                    {
                                        // get previous breaks (SS-IN)
                                        // get intersection with GR (if intersected => then omit the Grace handling from the ILF)
                                        // BUT ... later when we handle partial intersection between graces and excuses, we need to modify in here to take this effect ...
                                        // compare ILF with (XDs + GR)

                                        var TMP_SE = new ScheduleEvent();
                                        var TMP_AE = new AttendanceEvent();

                                        hasEarlyTimeOff = HandleTimeOff(AttendanceEventType.In, TMP_SE, TMP_AE, aContext, lateCome.InTime.Value.Date, aContext.Shift.StartTime, lateCome.InTime.Value, aContext.Shift.StartTime, aContext.Shift.EndTime);

                                        var ILF = lateCome.CalculatedDuration;  // in late effective
                                        var GRC = aContext.ShiftPolicy.LateComeGrace;  // Grace
                                        var XD = TMP_SE.CalculatedHours;  // break duration
                                        var LC = lateCome.Duration;  // late come
                                        var LL = lateDurationOut;  // late leave
                                        var GRCF = lateCome.StatusInId == AttendanceCodes.AttendanceEventStatus.CameOnGrace ? LC : GRC;  // effective grace to be used for deduction (not attended time during grace)
                                        var LateLeaveDurationAfterGrace = Math.Max(0, LL - (hasEarlyTimeOff ? (ILF == (LC - GRCF - XD) ? GRCF : 0) : GRCF));
                                        XMath.DeductBalance( ref LateLeaveDurationAfterGrace, ref ILF);
                                        lateDurationIN = ILF;
                                        lateDurationOut = LateLeaveDurationAfterGrace;
                                    }
                                    else
                                    {
                                        XMath.DeductBalance( ref lateDurationOut, ref lateDurationIN);
                                    }



                                    lateCome.CalculatedDuration = lateDurationIN;
                                    earlyOffsetEffective = lateDurationOut;
                                }
                            }
                        }
                        else
                        {
                            attendanceEventEarlyLeave.Duration = earlyOffsetActual;
                            attendanceEventEarlyLeave.CalculatedDuration = earlyOffsetActual;

                            #region Out Grace Time Status

                            var shiftEndWithGrace = aContext.Shift.EndTime.Value.AddMinutes(-earlyLeaveGracePolicy).TimeOfDay;
                            var diffFromOutGrace = XMath.Round((aContext.RawAttendance.AttendanceDateTime.TimeOfDay - shiftEndWithGrace).TotalMinutes);

                            if (aContext.Shift.IsNightShift)
                            {
                                var NormalizeOut = aContext.RawAttendance.AttendanceDateTime.AddHours(aContext.NightShiftDelta).AddDays(-1);
                                var Normalizeend = aContext.Shift.EndTime.Value.AddMinutes(-earlyLeaveGracePolicy).AddHours(aContext.NightShiftDelta).AddDays(-1);
                                diffFromOutGrace = XMath.Round((NormalizeOut.TimeOfDay - Normalizeend.TimeOfDay).TotalMinutes);
                            }

                            attendanceEventEarlyLeave.StatusOutId = diffFromOutGrace < 0 ? AttendanceCodes.AttendanceEventStatus.LeftEarly : AttendanceCodes.AttendanceEventStatus.LeftOnGrace;

                            #endregion
                        }

                        earlyOffsetEffective = Math.Abs(earlyOffsetEffective);
                        if (aContext.ShiftPolicy != null && aContext.ShiftPolicy.EarlyLeaveDeductFromOvertime &&
                            (attendanceEventEarlyLeave.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftEarly || attendanceEventEarlyLeave.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftOnGrace))
                        {
                            XMath.DeductBalance( ref earlyOffsetEffective, ref overtime);
                        }
                        attendanceEventEarlyLeave.CalculatedDuration = earlyOffsetEffective;

                        // in case of having overtime resulting from excuses or half-day leaves, try to extract it ...
                        var effectiveWorkingHours = aContext.ScheduleEvent.CalculatedHours + workingHours;
                        var ShiftMaxTime = XMath.Round(aContext.Shift.Duration * 60);

                        if (effectiveWorkingHours > ShiftMaxTime)
                        {
                            workingHours -= effectiveWorkingHours - ShiftMaxTime;
                        }

                        // check the out grace offset policy ...
                        if (!hasBreak)
                        {
                            bool EARLYOFFSETRELATEDTOGRACE = aContext.ShiftPolicy.EarlyLeaveOffsetRelatedToGraceTime;
                            attendanceEventEarlyLeave.CalculatedDuration = EARLYOFFSETRELATEDTOGRACE ?
                                Math.Max(0, (aContext.ScheduleEvent.Overtime > earlyLeaveGracePolicy ?
                                attendanceEventEarlyLeave.CalculatedDuration : attendanceEventEarlyLeave.Duration) - earlyLeaveGracePolicy)
                                : attendanceEventEarlyLeave.CalculatedDuration;
                        }

                        // ...
                        aContext.ScheduleEvent.Hours = workingHours;
                        aContext.ScheduleEvent.CalculatedHours += workingHours;
                        aContext.ScheduleEvent.Overtime = overtime;

                        var overTimeWithRate = XMath.Round(overtime * aContext.OverTime.Rate);
                        if (aContext.OverTime.Maximum > 0 && overTimeWithRate >= aContext.OverTime.Maximum) overTimeWithRate = aContext.OverTime.Maximum;

                        aContext.ScheduleEvent.CalculatedOvertime = overTimeWithRate;

                        aContext.AttendanceEventAdd(attendanceEventEarlyLeave);
                        aContext.ScheduleEvent.StatusOutId = attendanceEventEarlyLeave.StatusOutId;
                        aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                        //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                        aContext.ScheduleEventUpdate();
                    }
                    #endregion
                    #region Left on time

                    else
                    {
                        //Left on time.

                        aContext.ScheduleEvent.ActualOut = aContext.RawAttendance.AttendanceDateTime;
                        aContext.ScheduleEvent.OutTimeRawId = aContext.RawAttendance.Id;
                        aContext.ScheduleEvent.OutTimeIsOriginal = aContext.RawAttendance.IsOriginal;
                        aContext.ScheduleEvent.OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus;
                        aContext.ScheduleEvent.OutAttendanceSource = aContext.RawAttendance.AttendanceSource;

                        AttendanceEvent leftOnTimeEvent = new AttendanceEvent
                        {
                            ScheduleEventId = aContext.ScheduleEvent.Id,
                            OutTime = aContext.RawAttendance.AttendanceDateTime,
                            OutTimeRawId = aContext.RawAttendance.Id,
                            OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                            OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                            ManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                            Duration = 0,
                            StatusOutId = aContext.Shift.IsFlexible ? new Guid() : AttendanceCodes.AttendanceEventStatus.LeftOnTime
                        };


                        var hasTimeOff = HandleTimeOff(AttendanceEventType.Out, aContext.ScheduleEvent, leftOnTimeEvent, aContext, aContext.RawAttendance.AttendanceDateTime.Date, aContext.RawAttendance.AttendanceDateTime, aContext.Shift.EndTime, aContext.Shift.StartTime, aContext.Shift.EndTime, lateDurationIN);
                        if (hasTimeOff)
                        {
                            var TimeOffDuration = aContext.ScheduleEvent.CalculatedHours;
                            if (aContext.Shift.IsFlexible)
                            {

                                CalculateFlexibleShiftHours(aContext, workingHours, true, TimeOffDuration);
                            }
                            else
                            {
                                aContext.ScheduleEvent.Hours = workingHours;
                                aContext.ScheduleEvent.CalculatedHours = workingHours + TimeOffDuration;

                                if (aContext.ScheduleEvent.CalculatedHours > shiftDuration)
                                {
                                    aContext.ScheduleEvent.CalculatedHours = shiftDuration;
                                }

                                leftOnTimeEvent.StatusOutId = leftOnTimeEvent.CalculatedDuration < 0 ? AttendanceCodes.AttendanceEventStatus.LeftEarly
                                                            : leftOnTimeEvent.CalculatedDuration > 0 ? AttendanceCodes.AttendanceEventStatus.LeftLate
                                                                                                     : AttendanceCodes.AttendanceEventStatus.LeftOnTime;

                                // if came late and left late, update the late in to remove the late leave from it ( if the policy allows )
                                if ((leftOnTimeEvent.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftLate
                                    || (leftOnTimeEvent.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftOnTime))//&& ShiftIsAnOutsider
                                    && aContext.ShiftPolicy != null && aContext.ShiftPolicy.LateComeDeductFromOvertime)
                                {
                                    //check for late come record.                                    
                                    if (lateCome != null)
                                    {
                                        var lateDurationOut = Math.Abs(leftOnTimeEvent.CalculatedDuration);
                                        var lateDurationINBefore = lateDurationIN;
                                        if (aContext.ShiftPolicy.EarlyLeaveOffsetRelatedToGraceTime)
                                        {
                                            // get previous breaks (SS-IN)
                                            // get intersection with GR (if intersected => then omit the Grace handling from the ILF)
                                            // BUT ... later when we handle partial intersection between graces and excuses, we need to modify in here to take this effect ...
                                            // compare ILF with (XDs + GR)

                                            var TMP_SE = new ScheduleEvent();
                                            var TMP_AE = new AttendanceEvent();

                                            hasEarlyTimeOff = HandleTimeOff(AttendanceEventType.In, TMP_SE, TMP_AE, aContext, lateCome.InTime.Value.Date, aContext.Shift.StartTime, lateCome.InTime.Value, aContext.Shift.StartTime, aContext.Shift.EndTime);

                                            var ILF = lateCome.CalculatedDuration;  // in late effective
                                            var GRC = aContext.ShiftPolicy.LateComeGrace;  // Grace
                                            var XD = TMP_SE.CalculatedHours;  // break duration
                                            var LC = lateCome.Duration;  // late come
                                            var LL = lateDurationOut;  // late leave
                                            var GRCF = lateCome.StatusInId == AttendanceCodes.AttendanceEventStatus.CameOnGrace ? LC : GRC;  // effective grace to be used for deduction (not attended time during grace)
                                            var LateLeaveDurationAfterGrace = Math.Max(0, LL - (hasEarlyTimeOff ? (ILF == (LC - GRCF - XD) ? GRCF : 0) : GRCF));
                                            XMath.DeductBalance( ref LateLeaveDurationAfterGrace, ref ILF);
                                            lateDurationIN = ILF;
                                            lateDurationOut = LateLeaveDurationAfterGrace;
                                        }
                                        else
                                        {
                                            XMath.DeductBalance( ref lateDurationOut, ref lateDurationIN);
                                        }

                                        int lateDurationINBeforeWithGraceConsidered = aContext.ShiftPolicy.LateComeOffsetRelatedToGraceTime ? lateCome.Duration : lateDurationINBefore;

                                        lateCome.CalculatedDuration = lateDurationIN;
                                        leftOnTimeEvent.CalculatedDuration = lateDurationOut;
                                    }
                                }
                            }
                            aContext.ScheduleEvent.Hours = Math.Max(0, aContext.ScheduleEvent.Hours);
                        }
                        else
                        {
                            if (aContext.Shift.IsFlexible)
                            {
                                CalculateFlexibleShiftHours(aContext, workingHours, false, 0);
                            }
                            else
                            {
                                aContext.ScheduleEvent.Hours = workingHours;
                                aContext.ScheduleEvent.CalculatedHours += workingHours;
                                if (aContext.ScheduleEvent.CalculatedHours > shiftDuration)
                                {
                                    aContext.ScheduleEvent.CalculatedHours = shiftDuration;
                                }

                                leftOnTimeEvent.StatusOutId = AttendanceCodes.AttendanceEventStatus.LeftOnTime;
                            }
                        }
                        aContext.AttendanceEventAdd(leftOnTimeEvent);
                        aContext.ScheduleEvent.StatusOutId = leftOnTimeEvent.StatusOutId;
                        aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
                        //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
                        aContext.ScheduleEventUpdate();
                    }

                    #endregion
                }

                #endregion

                #region Absent

                if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.Absent)
                {
                    //Left after he was considered absent.
                    //Remove the absent record the start calculation from the start.
                    aContext.ScheduleEventDelete();
                    HandleFirstOut(aContext);
                    return;
                }

                #endregion

                #region Missed In Punch

                if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.MissedInPunch)
                {
                    aContext.ScheduleEventDelete();
                    HandleFirstOut(aContext);
                    return;
                }

                #endregion
            }
        }

        private static void CalculateFlexibleShiftHours(AttendanceContext aContext, int workingHours, bool hasBreak, int breakDuration)
        {
            int overtime = default(int); int actualWorkingMore = default(int); int actualworkingHours = workingHours;
            var shiftDuration = aContext.GetShiftDuration().Value;
            aContext.ScheduleEvent.CalculatedHours = workingHours;
            if (workingHours > shiftDuration)
            {
                actualWorkingMore = workingHours - shiftDuration;
                overtime = actualWorkingMore;
            }
            else if (hasBreak)
            {
                if (workingHours > (shiftDuration - breakDuration))
                {
                    var morethanExpected = workingHours - (shiftDuration - breakDuration);
                    workingHours -= morethanExpected;
                    aContext.ScheduleEvent.CalculatedHours -= morethanExpected;
                }
                aContext.ScheduleEvent.CalculatedHours += breakDuration;
            }
            if (aContext.ShiftPolicy != null)
            {
                overtime -= aContext.ShiftPolicy.OutOffsetForOvertime;
                overtime = Math.Max(overtime, 0);
            }
            aContext.ScheduleEvent.Overtime += overtime;
            var calculatedOvertime = XMath.Round(overtime * aContext.OverTime.Rate);
            aContext.ScheduleEvent.CalculatedOvertime = calculatedOvertime;

            if (aContext.OverTime.Maximum > 0 && calculatedOvertime >= aContext.OverTime.Maximum) aContext.ScheduleEvent.CalculatedOvertime = aContext.OverTime.Maximum;
            aContext.ScheduleEvent.Hours = workingHours - actualWorkingMore - (hasBreak ? (workingHours > (shiftDuration - breakDuration) ? breakDuration : 0) : 0);
            aContext.ScheduleEvent.CalculatedHours -= actualWorkingMore;
            aContext.ScheduleEvent.HoursStatusId = actualworkingHours > (shiftDuration - (hasBreak ? breakDuration : 0)) ? AttendanceCodes.HourStatus.WorkMore
                                                : (actualworkingHours < (shiftDuration - (hasBreak ? breakDuration : 0)) ? AttendanceCodes.HourStatus.WorkLess : Guid.Empty);
        }

        private void RemoveLastOut(AttendanceContext aContext)
        {
            var lastOut = aContext.ScheduleEvent.AttendanceEvents.
               Where(lo => lo.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftEarly
                   || lo.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftLate
                   || lo.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftOnTime
                   || lo.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftOnGrace
               ).FirstOrDefault();
            if (lastOut != null)
            {
                var timeDiff = aContext.RawAttendance.AttendanceDateTime - lastOut.OutTime.Value;
                if (timeDiff.TotalSeconds > aContext.DuplicatePunchThreshold)
                {
                    AttendanceEvent duplicatePunchAE = new AttendanceEvent
                    {
                        ScheduleEventId = lastOut.ScheduleEventId,
                        ManualAttendanceStatus = lastOut.ManualAttendanceStatus,
                        OutTime = lastOut.OutTime,
                        OutTimeRawId = lastOut.OutTimeRawId,
                        OutTimeIsOriginal = lastOut.OutTimeIsOriginal,
                        OutAttendanceSource = lastOut.OutAttendanceSource,
                    };
                    aContext.AttendanceEventAdd(duplicatePunchAE);
                }
                aContext.AttendanceEventDelete(lastOut);
            }
        }
        private AttendanceEvent GetLateLeave(AttendanceContext aContext)
        {
            var lastOut = aContext.ScheduleEvent.AttendanceEvents.
             Where(lo => lo.StatusOutId == AttendanceCodes.AttendanceEventStatus.LeftLate
             ).FirstOrDefault();
            return lastOut;
        }
        private void HandlePayCodeOut(AttendanceContext aContext)
        {
            if (aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn || aContext.ScheduleEvent.TotalStatusId == AttendanceCodes.ScheduleEventStatus.MissedOutPunch)
                aContext.ScheduleEvent.TotalStatusId = AttendanceCodes.ScheduleEventStatus.Attended;

            if (aContext.ScheduleEvent.TotalStatusId == null)
                aContext.ScheduleEvent.TotalStatusId = AttendanceCodes.ScheduleEventStatus.MissedInPunch;
            aContext.ScheduleEvent.ActualOut = aContext.RawAttendance.AttendanceDateTime;
            aContext.ScheduleEvent.OutTimeRawId = aContext.RawAttendance.Id;
            aContext.ScheduleEvent.OutTimeIsOriginal = aContext.RawAttendance.IsOriginal;
            aContext.ScheduleEvent.OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus;
            aContext.ScheduleEvent.OutAttendanceSource = aContext.RawAttendance.AttendanceSource;

            var hours = AttendanceContext.GetShiftDuration(aContext.Shift ?? aContext.ShiftPrevious).GetValueOrDefault();

            aContext.ScheduleEvent.Hours = hours;
            aContext.ScheduleEvent.CalculatedHours = hours;

            var overtime = 0;
            if (aContext.ScheduleEvent.ActualIn.HasValue)
            {
                overtime = XMath.Round((aContext.RawAttendance.AttendanceDateTime.AddHours(aContext.Shift != null && aContext.Shift.IsNightShift ? aContext.NightShiftDelta : 0).TimeOfDay - aContext.ScheduleEvent.ActualIn.Value.AddHours(aContext.Shift != null && aContext.Shift.IsNightShift ? aContext.NightShiftDelta : 0).TimeOfDay).TotalMinutes);
                overtime -= GetOutDurations(aContext);
                if (aContext.PayCodeStatus == AttendanceCodes.PayCodeStatus.OnAway)
                {
                    overtime = Math.Max(0, (overtime - hours));
                }
            }
            aContext.ScheduleEvent.Overtime = overtime;
            var result = XMath.Round(overtime * aContext.OverTime.Rate);
            if (aContext.OverTime.Maximum > 0 &&
                result >= aContext.OverTime.Maximum)
                result = aContext.OverTime.Maximum;
            if (aContext.Shift != null && aContext.ScheduleEvent.ShiftId == Guid.Empty)
            {
                aContext.ScheduleEvent.ShiftId = aContext.Shift.Id;
                aContext.ScheduleEvent.ExpectedIn = aContext.Shift.StartTime;
                aContext.ScheduleEvent.ExpectedOut = aContext.Shift.EndTime;
            }
            aContext.ScheduleEvent.CalculatedOvertime = result;
            aContext.ScheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
            //aContext.ScheduleEvent.Location = aContext.RawAttendance.Location;
            aContext.ScheduleEventUpdate();
        }
        private void HandlePayCodeFirstOut(AttendanceContext aContext)
        {
            var scheduleEvent = new ScheduleEvent
            {
                EventDate = aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date,
                PersonId = aContext.RawAttendance.PersonId,
                ScheduleId = aContext.Schedule.Id,
                ActualOut = aContext.RawAttendance.AttendanceDateTime,
                OutTimeRawId = aContext.RawAttendance.Id,
                OutTimeIsOriginal = aContext.RawAttendance.IsOriginal,
                OutManualAttendanceStatus = aContext.RawAttendance.ManualAttendanceStatus,
                OutAttendanceSource = aContext.RawAttendance.AttendanceSource,
                AttendanceEvents = new List<AttendanceEvent>(),
                TotalStatusId = AttendanceCodes.ScheduleEventStatus.MissedInPunch,
            };

            var hours = 0;
            if (aContext.Shift != null)
            {
                hours = aContext.GetShiftDuration().GetValueOrDefault();
                scheduleEvent.ShiftId = aContext.Shift.Id;
                scheduleEvent.ExpectedIn = aContext.Shift.StartTime;
                scheduleEvent.ExpectedOut = aContext.Shift.EndTime;
            }

            scheduleEvent.Hours = hours;
            scheduleEvent.CalculatedHours = hours;
            scheduleEvent.TerminalId = aContext.RawAttendance.TerminalId;
            //scheduleEvent.Location = aContext.RawAttendance.Location;

            aContext.ScheduleEvent = scheduleEvent;
            aContext.ScheduleEventCreate();
        }
        private int GetEarlyDuration(AttendanceContext aContext)
        {
            //var earlyRecord = aContext.ScheduleEvent.AttendanceEvents.Where(e => e.StatusInId == AttendanceCodes.AttendanceEventStatus.CameEarly).FirstOrDefault();
            //if (earlyRecord != null)
            //    return earlyRecord.CalculatedDuration;
            //// return earlyRecord.Duration;
            //else
            //    return 0;
            return Math.Max(0, aContext.Shift.IsFlexible ? 0 :
                ((int)((aContext.Shift.IsNightShift ? aContext.Shift.StartTime.Value.AddHours(aContext.NightShiftDelta).TimeOfDay : aContext.Shift.StartTime.Value.TimeOfDay) -
                (aContext.Shift.IsNightShift ? aContext.ScheduleEvent.ActualIn.Value.AddHours(aContext.NightShiftDelta).TimeOfDay : aContext.ScheduleEvent.ActualIn.Value.TimeOfDay)).TotalMinutes));
        }

        private int GetOutDurations(AttendanceContext aContext)
        {

            if (aContext.ScheduleEvent == null) return 0;
            if (!aContext.ShiftPolicy.SubtractBreaksDuration) return 0;
            var outEvents = aContext.ScheduleEvent.AttendanceEvents.Where(e => e.StatusInId == AttendanceCodes.AttendanceEventStatus.ExitDuringWork).Sum(d => (d.Duration));//d.CalculatedDuration
            return outEvents;
        }
        private int GetLateDiff(AttendanceContext aContext)
        {
            var earlyRecord = aContext.ScheduleEvent.AttendanceEvents.Where(e => e.StatusInId == AttendanceCodes.AttendanceEventStatus.CameLate ||
                                                                                  e.StatusInId == AttendanceCodes.AttendanceEventStatus.CameOnGrace ||
                                                                                  e.StatusInId == AttendanceCodes.AttendanceEventStatus.LateAbsent).FirstOrDefault();

            return earlyRecord != null ? Math.Abs(earlyRecord.Duration - earlyRecord.CalculatedDuration) : 0;
        }

        #endregion
        #region Common

        private bool HandleTimeOff(AttendanceEventType attendanceType, ScheduleEvent scheduleEvent, AttendanceEvent attendanceEvent, AttendanceContext aContext, DateTime date, DateTime? possibleStart, DateTime? possibleEnd, DateTime? shiftStart, DateTime? shiftEnd, int compansatingOffset = 0)
        {
            #region normalize times

            possibleStart = possibleStart ?? new DateTime(1800, 1, 1);
            possibleEnd = possibleEnd ?? new DateTime(1800, 1, 1).AddDays(1).AddMinutes(-1);
            shiftStart = shiftStart ?? new DateTime(1800, 1, 1);
            shiftEnd = shiftEnd ?? new DateTime(1800, 1, 1).AddDays(1).AddMinutes(-1);

            #endregion
            #region normalize dates

            // unify dates (not times)
            if (Math.Abs((possibleStart.Value - possibleEnd.Value).TotalDays) > 2)
            {
                if (possibleEnd.Value.Date <= new DateTime(1800, 1, 1).Date || possibleEnd.Value.Date >= DateTime.MaxValue.Date)
                {
                    possibleEnd = new DateTime(date.Year, date.Month, date.Day, possibleEnd.Value.Hour, possibleEnd.Value.Minute, possibleEnd.Value.Second);

                }
                else if (possibleStart.Value.Date <= new DateTime(1800, 1, 1).Date || possibleStart.Value.Date >= DateTime.MaxValue.Date)
                {
                    possibleStart = new DateTime(date.Year, date.Month, date.Day, possibleStart.Value.Hour, possibleStart.Value.Minute, possibleStart.Value.Second);
                }
            }

            if (aContext.Shift.IsNightShift)
            {
                var startDate = possibleStart.Value.Date;
                var EndDate = possibleEnd.Value.Date;
                possibleStart = possibleStart.Value.AddHours(aContext.NightShiftDelta);
                possibleEnd = possibleEnd.Value.AddHours(aContext.NightShiftDelta);
                possibleStart = new DateTime(startDate.Year, startDate.Month, startDate.Day, possibleStart.Value.Hour, possibleStart.Value.Minute, possibleStart.Value.Second);
                possibleEnd = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, possibleEnd.Value.Hour, possibleEnd.Value.Minute, possibleEnd.Value.Second);

            }


            // order ...
            if (possibleStart > possibleEnd) XIntervals.Swap(ref possibleStart, ref possibleEnd);

            #endregion

            var timeOff = CheckForTimeOffBreak(aContext, scheduleEvent, aContext.ScheduleDay != null ? aContext.ScheduleDay.Day : aContext.RawAttendance.AttendanceDateTime.Date, possibleStart.Value, possibleEnd.Value, compansatingOffset);

            #region validate

            if (attendanceType == AttendanceEventType.NotSet) return false;
            if (!aContext.Shift.IsFlexible)
            {
                if ((attendanceType == AttendanceEventType.Out) && (timeOff.TimeStart == shiftStart.Value.TimeOfDay || timeOff.TimeEnd == shiftStart.Value.TimeOfDay)
                 || (attendanceType == AttendanceEventType.In) && (timeOff.TimeStart == shiftEnd.Value.TimeOfDay || timeOff.TimeEnd == shiftEnd.Value.TimeOfDay))
                {
                    attendanceEvent.Duration = timeOff.Duration;
                    attendanceEvent.CalculatedDuration = timeOff.Duration;
                    return false;
                }
            }

            #endregion

            attendanceEvent.Duration = timeOff.Duration;
            attendanceEvent.CalculatedDuration = timeOff.DurationCalculated;
            attendanceEvent.BreakId = timeOff.BreakId;
            scheduleEvent.CalculatedHours += timeOff.BreakEffective;
            scheduleEvent.OffHoursCalculated += timeOff.WorkedDuringBreak;
            scheduleEvent.Comments += (string.IsNullOrWhiteSpace(scheduleEvent.Comments) ? "" : " ") + timeOff.BreakDetail;

            bool hasBreak = timeOff.BreakType.HasValue;
            return hasBreak;
        }

        private TimeOff CheckForTimeOffBreak(AttendanceContext aContext, ScheduleEvent scheduleEvent, DateTime date, DateTime start, DateTime end, int compansatingOffset = 0)
        {
            #region comments
            /*
            * A. excuse in start of the day ...
            * 
            *    AA. early come (before the shift start)
            *        Offset = X + Y (+ve value : early)   [ X = Shift Start - IN , Y = Excuse End - Shift Start ]
            *        
            *    AB. early come (after the shift start)
            *        Offset = Y - X (+ve value : early)   [ X = Shift Start - IN , Y = Excuse End - Shift Start ]
            *        
            *    AC. late come (after the Excuse end)
            *        Offset = Y - X (-ve value : late)    [ X = Shift Start - IN , Y = Excuse End - Shift Start ]
            *        
            *    AD. on time (at Excuse end)
            *        Offset = Y - X ( 0 value : on time)  [ X = Shift Start - IN , Y = Excuse End - Shift Start ]
            * 
            */
            #endregion

            var X = (new XIntervals.TimeRange(aContext.Shift.IsNightShift && start.Hour == 0 ? start.TimeOfDay.Add(new TimeSpan(24, 0, 0)) :
                start.TimeOfDay,
                aContext.Shift.IsNightShift && end.Date > start.Date ? end.TimeOfDay.Add(new TimeSpan(24, 0, 0)) :
                end.TimeOfDay)).GetOrdered();

            var shiftPeriod = new XIntervals.TimeRange(
                              aContext.Shift.IsNightShift ? (aContext.Shift.StartTime.Value.AddHours(aContext.NightShiftDelta).TimeOfDay) : aContext.Shift.IsFlexible ? new DateTime().TimeOfDay : aContext.Shift.StartTime.Value.TimeOfDay,
                              aContext.Shift.IsNightShift ? (aContext.Shift.EndTime.Value.AddHours(aContext.NightShiftDelta).TimeOfDay) : aContext.Shift.IsFlexible ? new TimeSpan(24, 0, 0) : aContext.Shift.EndTime.Value.TimeOfDay);


            var timeOff = new TimeOff { Duration = X.Minutes.GetValueOrDefault(), DurationCalculated = 0 };

            #region check excuses         
            // get only relevant excuse in this required time frame ...
            var excuses = aContext.ExcusesPreCacheLookFor(date, (aContext.Shift.IsFlexible ? new DateTime(1800, 1, 1) : start), (aContext.Shift.IsFlexible ? new DateTime(1800, 1, 1).AddDays(1).AddMinutes(-1) : end), aContext.Shift.IsNightShift, aContext.NightShiftDelta);
            foreach (var excuseMeta in excuses)
            {
                //if (IsExcuseTaken(scheduleEvent,excuseMeta.Excuse.Id)) continue;

                if (excuseMeta.Excuse.ExcuseStatusId == (int)ExcuseStatus.Approved) aContext.ExcuseChangeToTaken(excuseMeta.Excuse);   // mark that excuse as taken

                var Y = new XIntervals.TimeRange(
                  aContext.Shift.IsNightShift ? (excuseMeta.Excuse.StartTime.AddHours(aContext.NightShiftDelta).TimeOfDay)
                  : excuseMeta.Excuse.StartTime.TimeOfDay,
                  aContext.Shift.IsNightShift ? (excuseMeta.Excuse.EndTime.AddHours(aContext.NightShiftDelta).Hour == 0 ? excuseMeta.Excuse.EndTime.AddHours(aContext.NightShiftDelta).TimeOfDay.Add(new TimeSpan(24, 0, 0)) : excuseMeta.Excuse.EndTime.AddHours(aContext.NightShiftDelta).TimeOfDay)
                  : excuseMeta.Excuse.EndTime.TimeOfDay);

                if (aContext.Shift.IsFlexible)
                {
                    timeOff.Break += Y.Minutes.GetValueOrDefault();
                    timeOff.BreakEffective += Y.Minutes.GetValueOrDefault();
                    timeOff.Duration = 0;
                    timeOff.DurationCalculated = 0;
                }
                else
                {
                    var I = XIntervals.Intersect(X, Y);

                    var Xm = X.Minutes.GetValueOrDefault();   // context
                    var Ym = Y.Minutes.GetValueOrDefault();   // excuse
                    var Im = I.Minutes.GetValueOrDefault();   // time inside excuse


                    #region Excuse adjustment in case of having a section outside the shift boundries

                    var effectiveExcusePeriod = XIntervals.Intersect(shiftPeriod, Y);   // intersect excuse with shift

                    var effectiveExcusePeriodInMinutes = effectiveExcusePeriod.Minutes.GetValueOrDefault();
                    if (effectiveExcusePeriodInMinutes > 0 && compansatingOffset > 0)
                    {
                        var effectiveExcusePeriodInMinutesRemainder = Ym - effectiveExcusePeriodInMinutes;
                        var effectiveExcusePeriodInMinutesRemainderToCover = compansatingOffset > effectiveExcusePeriodInMinutesRemainder?
                                                                             effectiveExcusePeriodInMinutesRemainder 
                                                                           : compansatingOffset;
                        Ym = Math.Min(effectiveExcusePeriodInMinutes + effectiveExcusePeriodInMinutesRemainderToCover, Ym);
                    }
                    #endregion

                    if (I.IsPoint())
                    {
                        timeOff.Break += Ym;
                        timeOff.DurationCalculated += Xm + Ym + excuseMeta.ShiftEffectiveOffset;   // if the intersection was just a point, we will join the 2 periods instead of intersecting them ...
                        timeOff.WorkedDuringBreak += Ym - Im + excuseMeta.ShiftEffectiveOffset;
                        timeOff.BreakEffective += Ym - timeOff.WorkedDuringBreak;   // effective break, is the break time, that i didn't work in (break time - working time inside the break)                       
                    }
                    // check if the excuse / break, is having a section outside the shift boundaries, 
                    // and if the intersection between the excuse and the duration (I), is only touching the shift boundaries                    
                    else if (effectiveExcusePeriodInMinutes != Y.Minutes.GetValueOrDefault() && XIntervals.Intersect(I, shiftPeriod).IsPoint())
                    {
                        timeOff.Break += Ym;
                        timeOff.DurationCalculated += effectiveExcusePeriodInMinutes <= 0 ? //Case when all excuse is outside shift boundaries
                                                    (
                                                      Xm >= Ym ?                    //If the overtime larger than the excuse 
                                                      Xm :                          //Take the overtime as the effective break
                                                      compansatingOffset <= Ym ?    //Else check if the late come duration less than the excuse 
                                                      compansatingOffset : Ym       //Take the late come duration as effective excuse else take the whole excuse
                                                    )
                                                      : effectiveExcusePeriodInMinutes + (Xm); //else when excuse is intersected with shift so the effective break is the intersected + the time spent after shift finish (overtime)

                        timeOff.WorkedDuringBreak += effectiveExcusePeriodInMinutes + Im;
                        timeOff.BreakEffective += Ym - timeOff.WorkedDuringBreak;   // effective break, is the break time, that i didn't work in (break time - working time inside the break)
                    }
                    else if (XIntervals.IsSubset(I, Y, false))
                    {
                       
                        timeOff.Break += Ym;
                        timeOff.DurationCalculated += Xm - Ym - (compansatingOffset == 0 || compansatingOffset > Math.Abs(excuseMeta.ShiftEffectiveOffset) ? excuseMeta.ShiftEffectiveOffset : -compansatingOffset);
                        timeOff.WorkedDuringBreak += Ym - Im + (compansatingOffset == 0 || compansatingOffset > Math.Abs(excuseMeta.ShiftEndOffset) ? excuseMeta.ShiftEndOffset : -compansatingOffset);


                        //timeOff.DurationCalculated += Xm - Ym - excuseMeta.ShiftEffectiveOffset ;
                        //timeOff.WorkedDuringBreak += Ym - Im +  excuseMeta.ShiftEndOffset ;

                        timeOff.BreakEffective += Ym - timeOff.WorkedDuringBreak;   // effective break, is the break time, that i didn't work in (break time - working time inside the break)
                    }
                    else
                    {
                        timeOff.Break += Im;
                        timeOff.BreakEffective += Ym;
                        timeOff.DurationCalculated += Xm - Im - excuseMeta.ShiftEffectiveOffset;
                    }
                }
                timeOff.TimeStart = excuseMeta.Excuse.StartTime.TimeOfDay;
                timeOff.TimeEnd = excuseMeta.Excuse.EndTime.TimeOfDay;
                timeOff.BreakType = TimeOff.TimeOffType.Excuse;
                timeOff.BreakId = ((string.IsNullOrWhiteSpace(timeOff.BreakId) ? "" : timeOff.BreakId.ToString() + ";") + excuseMeta.Excuse.Id.ToString());
                timeOff.BreakDetail = string.Format("({0}, From: {1}, To: {2}{3})", excuseMeta.Excuse.IsAwayExcuse() ? "Away" : "Excuse", excuseMeta.Excuse.StartTime.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" + excuseMeta.Excuse.StartTime.TimeOfDay.Minutes.ToString().PadLeft(2, '0'),
                    excuseMeta.Excuse.EndTime.TimeOfDay.Hours.ToString().PadLeft(2, '0')  + ":" + excuseMeta.Excuse.EndTime.TimeOfDay.Minutes.ToString().PadLeft(2, '0') , string.IsNullOrWhiteSpace(excuseMeta.Excuse.Notes) ? "" : ", Note: " + excuseMeta.Excuse.Notes);
                return timeOff;
            }

            #endregion
            #region check half day leaves

            var leave = aContext.LeavesLookForHalfLeaves(date, start, end);
            if (leave != null)
            {
                if (IsBreakTaken(scheduleEvent, leave.Id))
                {
                    timeOff.DurationCalculated = X.Minutes.GetValueOrDefault();
                    return timeOff;
                }
                    
                if (aContext.Shift.IsFlexible)
                {
                    timeOff.BreakEffective = XMath.Round(aContext.Shift.Duration / 2);
                    timeOff.Break = XMath.Round(aContext.Shift.Duration / 2);
                    timeOff.DurationCalculated = 0;
                    timeOff.Duration = 0;
                }
                else
                {
                    var dayStart = aContext.Shift.IsNightShift ? aContext.Shift.StartTime.Value.AddHours(aContext.NightShiftDelta) : aContext.Shift.StartTime.Value;
                    var dayEnd = aContext.Shift.IsNightShift ? aContext.Shift.EndTime.Value.AddHours(aContext.NightShiftDelta) : aContext.Shift.EndTime.Value;
                    var dayMid = dayStart.AddTicks((dayEnd.TimeOfDay - dayStart.TimeOfDay).Ticks / 2);

                    var leaveStart = leave.LeaveModePeriod == LeaveModePeriod.FirstHalf ? dayStart : dayMid;
                    var leaveEnd = leave.LeaveModePeriod == LeaveModePeriod.FirstHalf ? dayMid : dayEnd;

                    TimeSpan timeOffStart = start.TimeOfDay > leaveStart.TimeOfDay ? start.TimeOfDay : leaveStart.TimeOfDay;
                    TimeSpan timeOffEnd = leaveEnd.TimeOfDay < end.TimeOfDay ? leaveEnd.TimeOfDay : end.TimeOfDay;

                    timeOff.TimeStart = leaveStart.TimeOfDay;
                    timeOff.TimeEnd = leaveEnd.TimeOfDay;                               
                    timeOff.BreakEffective =  XMath.Round((timeOffEnd - timeOffStart).TotalMinutes) ;
                    timeOff.Break= timeOff.BreakExpected = XMath.Round((leaveEnd.TimeOfDay - leaveStart.TimeOfDay).TotalMinutes);
                    timeOff.WorkedDuringBreak = Math.Max(0,timeOff.Break-timeOff.BreakEffective);
                    timeOff.DurationCalculated = XIntervals.Intersect(X, shiftPeriod).IsPoint() ?  timeOff.BreakExpected: timeOff.Duration - timeOff.BreakExpected;

                }
                timeOff.BreakType = TimeOff.TimeOffType.Leave;
                timeOff.BreakId = leave.Id.ToString();
                return timeOff;
            }

            #endregion
            #region nothing found
            if (aContext.Shift.IsFlexible)
            {
                timeOff.Duration = 0;
                timeOff.DurationCalculated = 0;
            }
            else
            {
                timeOff.DurationCalculated = X.Minutes.GetValueOrDefault();
            }
            return timeOff;

            #endregion
        }

        private bool IsBreakTaken(ScheduleEvent scheduleEvent, Guid excuseId)
        {
            //Check if the excuse handled before with any existing Attendance Event
            return scheduleEvent.AttendanceEvents.Any(x => x.BreakId != null ? x.BreakId.Contains(excuseId.ToString()) : false);
        }

        #endregion

        internal static bool StartViolationWorkflow(ScheduleEvent scheduleEvent)
        {
            try
            {
                if (scheduleEvent == null) return false;
                var person = scheduleEvent.Person ?? TamamServiceBroker.PersonnelHandler.GetPerson(scheduleEvent.PersonId, SystemRequestContext.Instance).Result;

                bool allowViolationsCreation = person != null && person.AccountInfo.ShowAttendance.GetValueOrDefault();
                if (!allowViolationsCreation) return true;

                bool hasPreviousWFInstance = Broker.WorkflowEngine.InstanceExists(scheduleEvent.Id, violationsWorkflowDefinition.Id);
                bool hasViolations = HasViolations(scheduleEvent);

                if (!hasPreviousWFInstance && !hasViolations) return true;    // normal case ...

                bool status = true;
                
                if (hasPreviousWFInstance)                                    // cancel previous WF, then get out ...
                {
                    status = Broker.WorkflowEngine.CancelAndDelete(scheduleEvent, violationsWorkflowDefinition.Id);
                    if (!status) XLogger.Error(string.Format("Error : can't delete previous WF instance for (SE id : {0} , Person id : {1})" , scheduleEvent.Id , scheduleEvent.PersonId));
                    //return Broker.WorkflowEngine.Cancel(scheduleEvent, violationsWorkflowDefinition.Id);
                }
                if (hasViolations)
                {
                    status = Broker.WorkflowEngine.Initialize(scheduleEvent, violationsWorkflowDefinition);
                    if (!status) XLogger.Error(string.Format("Error : can't create WF instance for (SE id : {0} , Person id : {1})", scheduleEvent.Id, scheduleEvent.PersonId));
                }

                return status;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        internal static bool CancelViolationWorkflow(ScheduleEvent scheduleEvent)
        {
            try
            {
                if (!Broker.WorkflowEngine.InstanceExists(scheduleEvent.Id, violationsWorkflowDefinition.Id)) return true;

                //bool status = Broker.WorkflowEngine.Cancel(scheduleEvent, violationsWorkflowDefinition.Id);
                bool status = Broker.WorkflowEngine.CancelAndDelete(scheduleEvent, violationsWorkflowDefinition.Id);
                if (!status) throw new Exception(scheduleEvent.Id.ToString());

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        internal static bool CancelManualEditWorkflow(AttendanceRawDataHistoryItem historyItem)
        {
            try
            {
                if (!Broker.WorkflowEngine.InstanceExists(historyItem.Id, manualEditDefinition.Id)) return true;

                //bool status = Broker.WorkflowEngine.Cancel(historyItem, manualEditDefinition.Id);
                bool status = Broker.WorkflowEngine.CancelAndDelete(historyItem, manualEditDefinition.Id);
                if (!status) throw new Exception(historyItem.Id.ToString());

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        private static bool HasViolations(ScheduleEvent scheduleEvent)
        {
            var status = scheduleEvent.GetDetailedStatusModel();

            var attendancePolicy = GetAttendancePolicy(scheduleEvent.PersonId);
            if (attendancePolicy == null) return false;
            if (attendancePolicy.ConsiderAbsentAsViolation && status.TotalStatus.HasValue && status.TotalStatus.Value == AttendanceCodes.ScheduleEventStatus.Absent) return true;
            if (attendancePolicy.ConsiderMissedPunchAsViolation && status.TotalStatus.HasValue && status.TotalStatus.Value == AttendanceCodes.ScheduleEventStatus.MissedInPunch) return true;
            if (attendancePolicy.ConsiderMissedPunchAsViolation && status.TotalStatus.HasValue && status.TotalStatus.Value == AttendanceCodes.ScheduleEventStatus.MissedOutPunch) return true;

            if (attendancePolicy.ConsiderInLateAsViolation && status.InStatus.HasValue && status.InStatus.Value == AttendanceCodes.AttendanceEventStatus.CameLate) return true;
            if (attendancePolicy.ConsiderLateAbsentAsViolation && status.InStatus.HasValue && status.InStatus.Value == AttendanceCodes.AttendanceEventStatus.LateAbsent) return true;
            if (attendancePolicy.ConsiderEarlyLeaveAsViolation && status.OutStatus.HasValue && status.OutStatus.Value == AttendanceCodes.AttendanceEventStatus.LeftEarly) return true;
            if (attendancePolicy.ConsiderWorkingLessAsViolation && scheduleEvent.Shift != null && scheduleEvent.CalculatedHours != (scheduleEvent.Shift.Duration * 60) && scheduleEvent.CalculatedHours != 0) return true;

            return false;
        }


        private static AttendancePolicy GetAttendancePolicy(Guid PersonId)
        {
            var policies = GetPolicies(PersonId);
            if (policies == null) return null;

            var nativePolicy = policies.FirstOrDefault(p => p.PolicyTypeId == Guid.Parse(PolicyTypes.AttendancePolicyType));
            if (nativePolicy == null) return null;

            var policy = new AttendancePolicy(nativePolicy);

            return policy;
        }

        private static List<Policy> GetPolicies(Guid personId)
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies(personId, SystemRequestContext.Instance);
            if (response.Type != ResponseState.Success || response.Result == null || response.Result.Count == 0) return null;

            return response.Result;
        }
        #endregion
    }
}
