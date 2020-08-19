using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using System.Threading;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Common;
using XCore.Framework.Utilities;

namespace ADS.Tamam.Modules.Attendance.Handlers
{
    public class AttendanceWorkers
    {
        #region props ...

        public bool Initialized { get; private set; }

        private readonly RequestContext requestContext;

        private readonly Task mainTask;
        private readonly Task handleDirtyTask;
        private readonly Task handleOldData;
        private readonly Task pullTask;
        private readonly Task statsTask;
        private TimeSpan DirtyCheckThreadDelay { get; set; }
        private TimeSpan ShiftStartThreadDelay { get; set; }
        private TimeSpan ShiftEndThreadDelay { get; set; }
        private TimeSpan PullInterval { get; set; }
        private TimeSpan StatsInterval { get; set; }
        private int OldDataInterval { get; set; }

        private bool oldInProgressFlag = true;
        private string ShiftGraceInProgressFlagForPerson;

        private object mutext = new object();

        #endregion
        #region cst ...

        public AttendanceWorkers(RequestContext context)
        {
            if (!TamamServiceBroker.Status.Initialized || !InitializeConfigurations()) return;

            try
            {
                requestContext = context;
                mainTask = new Task(MainTask);
                handleDirtyTask = new Task(HandleDirtyTask);
                handleOldData = new Task(HandleOldData);
                pullTask = new Task(PullTask);    
                statsTask = new Task(StatsTask);           
                Initialized = true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                Initialized = false;
            }
        }



        #endregion
        #region publics ...

        public void StartHandleDirty()
        {
            handleDirtyTask.Start();
        }
        public void StartDaily()
        {
            mainTask.Start();
        }
        public void StartHandleOldData()
        {
            handleOldData.Start();
        }
        public void StartPull()
        {
            pullTask.Start();
        }

        public void StartStatsTask()
        {
            statsTask.Start();
        }
        public void Terminate()
        {
            if (mainTask != null && mainTask.Status == TaskStatus.Running) ;
        }

        #endregion
        #region models ...

        private struct ScheduleDayShift
        {
            public DateTime BaseDate { get; set; }

            public Shift Shift { get; set; }
            public ScheduleDay ScheduleDay { get; set; }
        }

        private class PersonGraceTimeModel
        {
            public Guid PersonId { get; set; }
            public TimeSpan GraceTime { get; set; }
        }

        #endregion

        #region Workers

        private void HandleOldData()
        {
            // JUST CREATING EMPTY SEs, WITH CARING FOR LEAVES OR WEAKENDS ...

            #region LOG

            XLogger.Info("Attendance Recovery : Started.");

            #endregion

            // TODO : instead of evaluating the old date from just the config, also take the last running time in consideration ...
            var date = DateTime.Now.AddDays(-OldDataInterval);

            if (date.Date < DateTime.Now.Date) // service stopped more than a day ?
            {
                var dates = XDate.Sequence(date.Date, DateTime.Now.Date, true);

                #region LOG

                XLogger.Info("Attendance Recovery : Service Stopped for " + dates.Count.ToString() + " days. Re-processing...");

                #endregion

                dates.ForEach(d => CalculateDay(d, false)); // false for a sequential processing (not concurrent)
            }

            oldInProgressFlag = false;

            #region LOG

            XLogger.Info("Attendance Recovery : Finished.");

            #endregion
        }
        private async void HandleDirtyTask()
        {
            //! Dirty schedule events should be deleted and get raw attendance that match its criteria to be reprocessed

            #region wait : for previous days processing ...

            while (oldInProgressFlag)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            #endregion

            #region LOG

            XLogger.Info("Handle Dirty Task: Started.");

            #endregion

            while (true)
            {
                var dirtySES = GetScheduleEventsDirty();
                if (dirtySES == null) continue;

                foreach (var dirtySE in dirtySES)
                {
                    #region LOG

                    XLogger.Info(
                        string.Format(
                            "Handle Dirty Task: Start Handling dirty schedule event for person Id: {0} on date {1} ,Schedule Event ID: {2}",
                            dirtySE.PersonId, dirtySE.EventDate.ToShortDateString(), dirtySE.Id.ToString()));

                    #endregion

                    var personId = dirtySE.PersonId;
                    var date = dirtySE.EventDate;
                    var eventDateSEs = GetScheduleEvents(dirtySE.PersonId, dirtySE.EventDate);

                    var rawAttendance = GetAttendanceRaw(dirtySE.PersonId, dirtySE.EventDate, eventDateSEs ?? new List<ScheduleEvent>() { dirtySE });
                    if (rawAttendance == null) continue;

                    if (!DeleteScheduleEvents(eventDateSEs)) continue;
                    //if (!DeleteScheduleEvents(dirtySE)) continue;


                    //the is Dirty task will not handle the raw attendance by itself but it will mark it as need process and let the handling for pull task                   
                    foreach (var rawItem in rawAttendance)
                    {
                        rawItem.IsProcessed = false;
                    }

                    TamamServiceBroker.AttendanceHandler.UpdateAttendanceRawData(rawAttendance);

                    //in order to take care about the shifts or days that not have raw attendance data (other shift, on leave,..)
                    var dateforCalculate = dirtySE.EventDate.Date == DateTime.Today.Date ? dirtySE.EventDate.Date.AddTicks(DateTime.Now.TimeOfDay.Ticks) : dirtySE.EventDate.AddDays(1).AddMilliseconds(-1);
                    CalculateDayForPerson(dateforCalculate, dirtySE.PersonId);
                }

                #region cache

                if (dirtySES.Count > 0)
                {
                    Broker.Cache.Invalidate(TamamCacheClusters.Attendance);
                }

                #endregion
                #region wait ...

                var delay = DirtyCheckThreadDelay;
                await Task.Delay(delay);

                #endregion
            }
        }
        private async void MainTask()
        {
            // CREATING THE SHIFT START AND SHIFT END THREADS, TO HANDLE ATTENDANCE CASES BASED ON TIME (INCLUDING MISSING PUNCHES)

            while (oldInProgressFlag)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            XLogger.Info("Main Sync Task Started.");
            while (true)
            {
                CalculateDay(DateTime.Now, true);
                var delay = (DateTime.Now.Date.AddDays(1) - DateTime.Now).Add(TimeSpan.FromMinutes(5));

                XLogger.Info("Main Sync Task: Wait for hour 00:05 in next day. Delay for: " + delay);
                await Task.Delay(delay); // Wait for hour 00:05 next day.
            }
        }
        private async void PullTask()
        {
            #region Wait for thread : handle old data

            while (oldInProgressFlag)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            #endregion

            while (true)
            {
                CalculateUnprocessed();

                await Task.Delay(PullInterval);
            }
        }
        private async void StatsTask()
        {
            while (true)
            {
                CalculateStats();

                await Task.Delay(StatsInterval);
            }
        }
    
        #endregion

        #region Helpers

        private bool InitializeConfigurations()
        {
            try
            {
                XLogger.Info("Loading Configurations...");

                string dirtyCheckDelay = Broker.ConfigurationHandler.GetValue(Constants.TamamEngineConfig.Section, Constants.TamamEngineConfig.WorkerDirtyInterval);
                DirtyCheckThreadDelay = TimeSpan.Parse(dirtyCheckDelay);

                //! This configuration will not be used again due to usage of ( Late Come Grace Duration ) as abase to set the ( Absent State ) for personnel
                string shiftStartThreadDelaystr = Broker.ConfigurationHandler.GetValue(Constants.TamamEngineConfig.Section,Constants.TamamEngineConfig.WorkerShiftStartDelay);
                ShiftStartThreadDelay = TimeSpan.Parse(shiftStartThreadDelaystr);

                string shiftEndThreadDelay = Broker.ConfigurationHandler.GetValue(Constants.TamamEngineConfig.Section,Constants.TamamEngineConfig.WorkerShiftEndDelay);
                ShiftEndThreadDelay = TimeSpan.Parse(shiftEndThreadDelay);

                string oldDataInterval = Broker.ConfigurationHandler.GetValue(Constants.TamamEngineConfig.Section,Constants.TamamEngineConfig.DataCalculationPreviousDays);
                OldDataInterval = int.Parse(oldDataInterval);

                string pullIntervalstr = Broker.ConfigurationHandler.GetValue(Constants.TamamEngineConfig.Section,Constants.TamamEngineConfig.WorkerCalculationsInterval);
                PullInterval = TimeSpan.Parse(pullIntervalstr);

                string statsIntervalstr = Broker.ConfigurationHandler.GetValue(Constants.TamamEngineConfig.Section, Constants.TamamEngineConfig.StatsCalculationInterval);
                StatsInterval = TimeSpan.Parse(statsIntervalstr);

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Configurations Error : Exception : " + x);
                return false;
            }
        }

        private void CalculateUnprocessed()
        {
            //Get All Raw data for all persons
            //TODO: Work in not dirty raw data and UnProcessed 
            var response = TamamServiceBroker.AttendanceHandler.GetUnProcessedAttendanceRaw();
            if (response.Type != ResponseState.Success) return;

            var unProcessedRawAttendances = response.Result;
            if (!unProcessedRawAttendances.Any()) return;
            var groupedUnProcessedRawAttendances = unProcessedRawAttendances.GroupBy(x => x.PersonId);
            foreach (var PersonUnProcessedRawAttendances in groupedUnProcessedRawAttendances)
            {
                var personId = PersonUnProcessedRawAttendances.Key;
                #region lock the processed person, from having his planned attendance tasks active (MainTask)

                lock (mutext)
                {
                    ShiftGraceInProgressFlagForPerson = personId.ToString();
                }

                #endregion
                // get associated person ...
                var person = TamamServiceBroker.PersonnelHandler.GetPerson(personId, requestContext).Result;
                var PersonDateUnProcessedRawAttendances = PersonUnProcessedRawAttendances.GroupBy(x => x.AttendanceDateTime.Date);
                foreach (var PersonDateRawData in PersonDateUnProcessedRawAttendances)
                {
                    var rawDataDate = PersonDateRawData.Key;
                    if (person != null)
                    {
                        //! skip / ignore raw attendance that accrued before join date ...
                        if (!person.AccountInfo.JoinDate.HasValue || (person.AccountInfo.JoinDate != null && rawDataDate < person.AccountInfo.JoinDate.Value.Date))
                        {
                            //! mark ignored raw attendance as processed ( to not get it again )
                            //TamamServiceBroker.AttendanceHandler.MarkRawDataAsProcessed(rawData.Id);
                            continue;
                        }
                    }
                    foreach (var rawData in PersonDateRawData)
                    {
                        if (!rawData.ConsiderAsAttendance)
                        {
                            //! mark raw attendance as processed ( to not get it again )
                            TamamServiceBroker.AttendanceHandler.MarkRawDataAsProcessed(rawData.Id);
                            continue;
                        }
                        var AtteContext = new AttendanceContext(rawData, requestContext);
                        var isDirtySE = AtteContext.EvaluateSEDirtyState(); var Success = true;
                        if (isDirtySE)
                        {
                            var SE = AtteContext.ScheduleEvent;
                            SE.IsDirty = true;                            
                            TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent(SE);
                            break;
                        }
                        else
                        {
                            var response_attCalc = TamamServiceBroker.AttendanceHandler.HandleAttendanceEvent(rawData, requestContext);
                            if (response_attCalc.Type != ResponseState.Success)
                            {
                                Success = false;
                            }
                        }
                        if (Success)
                        {
                            TamamServiceBroker.AttendanceHandler.MarkRawDataAsProcessed(rawData.Id);
                        }
                    }
                    List<ScheduleEvent> SEs = new List<ScheduleEvent>();
                    SEs.AddRange(TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, rawDataDate).Result ?? new List<ScheduleEvent>());
                    SEs.AddRange(TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, rawDataDate.AddDays(-1)).Result ?? new List<ScheduleEvent>());   // to check up cases of yesterday's night shifts generated from raw attendance provided today ...
                    foreach (var SE in SEs)
                    {
                        if (SE.Shift != null && SE.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn)
                        {
                            var dayShiftEndTime = SE.Shift.IsFlexible
                                ? rawDataDate.AddDays(1).AddMinutes(-5)
                                : rawDataDate.AddTicks(SE.Shift.EndTime.Value.TimeOfDay.Ticks);
                            var timeForShiftEnd = SE.Shift.IsFlexible
                                ? dayShiftEndTime
                                : ((dayShiftEndTime) + ShiftEndThreadDelay).AddDays(SE.Shift.IsNightShift ? 1 : 0);

                            var timeForNow = DateTime.Now;

                            if (timeForShiftEnd < timeForNow)
                            {
                                SE.TotalStatusId = AttendanceCodes.ScheduleEventStatus.MissedOutPunch;
                                TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent(SE);
                                AttendanceHandler.StartViolationWorkflow(SE);

                            }
                        }
                    }

                }
            }            
            #region Clear the lock on any person, to clear the way for the mainTask process ...
            lock (mutext)
            {
                ShiftGraceInProgressFlagForPerson = null;
            }
            #endregion            
            #region cache
            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);
            #endregion
        }

        private void CalculateStats()
        {
            try
            {
                TamamServiceBroker.AttendanceHandler.GetDepartmentAttendanceStatsByTerminalIds(requestContext);
            }
            catch (Exception e)
            {
                XLogger.Error(e.Message);
            }
        }
        private void HandleMissedOutPunch()
        {
          
            bool autoDetectedType;
            if (!bool.TryParse(Broker.ConfigurationHandler.GetValue(ADS.Common.Constants.TamamCaptureConfig.Section, ADS.Common.Constants.TamamCaptureConfig.TypeAutoDetectionMode),
                out autoDetectedType)) autoDetectedType = false;
            /*
             * 1- Check TypeAutoDetectionMode Configuration if true
             * 2- get all GetAttendanceRaw for this SE order by AttendanceDateTime
             * 3- get the last attendance raw update it's type to be out 
             * 4- mark SE as dirty
             */
            if (autoDetectedType)
            {
                var SEs = GetScheduleEventsMissedOut();
                foreach (var SE in SEs)
                {
                    var responseAttendanceRaw = TamamServiceBroker.AttendanceHandler.GetAttendanceRaw(new List<Guid> { SE.Id }, true, SystemRequestContext.Instance);
                    if (responseAttendanceRaw.Type == ResponseState.Success)
                    {
                        var rawattendanceData = responseAttendanceRaw.Result.OrderBy(x => x.AttendanceDateTime).ToList();
                        if (rawattendanceData.Count > 0)
                        {
                            var lastRawattendance = rawattendanceData[rawattendanceData.Count() - 1];
                            lastRawattendance.HandleForShiftEnd = true;
                            lastRawattendance.Type = AttendanceEventType.Out;
                            TamamServiceBroker.AttendanceHandler.UpdateAttendanceRawData(lastRawattendance);
                            SE.IsDirty = true;
                            TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent(SE);
                        }
                    }
                }

                #region cache
                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);
                #endregion
               
            }         
        }
        private void CalculateDay(DateTime date, bool isConcurrent)
        {
            #region LOG

            var preMessage = "Handle Calculation for day:" + date;
            XLogger.Info(preMessage);

            #endregion

            //! Caching already applied on this following method (GetScheduleDays)
            var response = TamamServiceBroker.SchedulesHandler.GetScheduleDays(date.Date, requestContext);
            var responsePreviousDays = TamamServiceBroker.SchedulesHandler.GetScheduleDays(date.Date.AddDays(-1), requestContext);
            if (response.Type != ResponseState.Success || responsePreviousDays.Type != ResponseState.Success)
            {
                #region LOG

                XLogger.Error(preMessage + ": GetScheduleDays failed due to '" + response.Type + "'");

                #endregion

                return;
            }

            var scheduleDays = response.Result;
            var scheduleDaysPrevious = responsePreviousDays.Result;

            #region LOG

            XLogger.Info(preMessage + ": found " + scheduleDays.Count + " Schedule days");

            #endregion

            //On Case of Night Shift the Main task will handle the end of the previous day (the end on running shift) and the start of the currant day (the start of the next shift)
            //In All other cases the Main task will handle the end of the currant day and the start of the currant day (the start and the end for the next shift)
            foreach (var PreviousScheduleDay in scheduleDaysPrevious)
            {
                HandleNightPreviousDayEnd(date.Date.AddDays(-1), PreviousScheduleDay, isConcurrent);
            }

            foreach (var scheduleDay in scheduleDays)
            {
                CalculateDay(date, scheduleDay, isConcurrent);
            }
            HandleMissedOutPunch();
        }
        private void HandleNightPreviousDayEnd(DateTime date, ScheduleDay previousScheduleDay, bool isConcurrent)
        {
            #region LOG

            var preMessage = "Handle Previous Schedule Day End :" + date.ToShortDateString();
            XLogger.Info(preMessage);

            #endregion

            if (previousScheduleDay.DayShifts != null)
            {
                #region LOG

                XLogger.Info(preMessage + " : Handle Shift");

                #endregion

                var dayShift = previousScheduleDay.DayShifts.Select(s => s.Shift).FirstOrDefault(x => x.IsNightShift);
                if (dayShift == null)
                {
                    #region LOG

                    XLogger.Info(preMessage + " : No Night Shift in Previous Schedule Day");

                    #endregion
                    return;
                }

                #region LOG

                XLogger.Info(preMessage + " : Handle Night Shift in Previous Schedule Day End: " + dayShift.Name + ". Day:" + date);

                #endregion

                var previousScheduleDayShift = new ScheduleDayShift
                {
                    ScheduleDay = previousScheduleDay,
                    Shift = dayShift,
                    BaseDate = date
                };

                if (isConcurrent)
                {
                    // handle the end of the previous day shift (if exists, and if is night shift) ...                         
                    var prerviousShiftEndTask = new Task(HandleShiftEnd, previousScheduleDayShift);
                    prerviousShiftEndTask.Start();
                }
                else
                {
                    HandleShiftEnd(previousScheduleDayShift);
                }
            }
        }
        private void CalculateDay(DateTime date, ScheduleDay scheduleDay, bool isConcurrent)
        {
            #region LOG

            var preMessage = "Handle Schedule day :" + date.ToShortDateString();
            XLogger.Info(preMessage);

            #endregion

            if (scheduleDay.IsDayOff)
            {
                #region LOG

                XLogger.Info(preMessage + " : Handle day off.");

                #endregion

                //---
                CreateScheduleEventForWeekend(scheduleDay, date, isConcurrent);

                #region cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            }
            else
            {
                if (scheduleDay.DayShifts != null)
                {
                    #region LOG

                    XLogger.Info(preMessage + " : Handle Shift(s)");

                    #endregion

                    foreach (var dayShift in scheduleDay.DayShifts.Select(s => s.Shift))
                    {
                        #region LOG

                        XLogger.Info(preMessage + " : Handle Shift: " + dayShift.Name + ". Day:" + date);

                        #endregion

                        var scheduleDayShift = new ScheduleDayShift
                        {
                            ScheduleDay = scheduleDay,
                            Shift = dayShift,
                            BaseDate = date
                        };

                        if (isConcurrent)
                        {
                            // handle the grace of the current day shift ...
                            var shiftStartTask = new Task(HandleShiftStartAsync, scheduleDayShift);
                            shiftStartTask.Start();

                            // handle the end of the current day shift (if is not a night shift) ...
                            //End day in case of Night Shift will handle in Calculate Previous Day
                            if (!scheduleDayShift.Shift.IsNightShift)
                            {
                                var shiftEndTask = new Task(HandleShiftEnd, scheduleDayShift);
                                shiftEndTask.Start();
                            }
                        }
                        else
                        {
                            // handle the grace of the current day shift ...
                            HandleShiftStartAsync(scheduleDayShift);

                            // handle the end of the current day shift (if is not a night shift) ...
                            //End day in case of Night Shift will handle in Calculate Previous Day
                            if (!scheduleDayShift.Shift.IsNightShift)
                            {
                                HandleShiftEnd(scheduleDayShift);
                            }
                        }
                    }
                }
            }
        }
        public void CalculateDay(DateTime date, Guid personId)
        {
            #region LOG

            var preMessage = string.Format("Handle Calculation for day: {0} and person {1}", date.ToShortDateString(),
                personId);
            XLogger.Info(preMessage);

            #endregion

            var response = TamamServiceBroker.SchedulesHandler.GetPersonScheduleDay(personId, date.Date, requestContext);
            if (response.Type == ResponseState.Success)
            {
                var scheduleDay = response.Result;
                CalculateDay(date, scheduleDay, false);
            }
            else
            {
                #region LOG

                XLogger.Error(preMessage + ": GetPersonScheduleDay failed due to '" + response.Type + "'");

                #endregion
            }
        }
        private async void HandleShiftStart(object scheduleDayShift)
        {
            if (!(scheduleDayShift is ScheduleDayShift)) return;

            #region Time Delay ...

            var dayShift = (ScheduleDayShift)scheduleDayShift;

            var timeForShiftStart = dayShift.Shift.IsFlexible ? DateTime.Now.Date.AddDays(1).AddMinutes(-5).TimeOfDay : dayShift.Shift.StartTime.Value.TimeOfDay + ShiftStartThreadDelay;
            var timeForDay = dayShift.BaseDate.TimeOfDay;

            if (timeForShiftStart >= timeForDay)
            {
                var delay = timeForShiftStart - timeForDay;

                #region LOG

                XLogger.Info("delay for Shift Start :" + dayShift.Shift.Name + " Hours: " + delay.TotalHours);

                #endregion

                await Task.Delay(delay);
            }

            #region LOG

            XLogger.Info("Start Handling Shift Start : " + dayShift.Shift.Name + ". " + dayShift.BaseDate);

            #endregion

            #endregion

            #region create schedule events ...

            var personnel = TamamServiceBroker.SchedulesHandler.GetScheduleActivePersons(dayShift.ScheduleDay.ScheduleId, dayShift.BaseDate, requestContext).Result;
            if (personnel == null || personnel.Count == 0) return;

            foreach (var person in personnel)
            {
                var SE = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(person.Id, dayShift.Shift.Id, dayShift.BaseDate.Date).Result;
                if (SE != null) continue;

                Leave associatedLeave; Holiday associatedholiday;
                var paycode = GetPayCode(person.Id, dayShift.BaseDate.Date, dayShift, out associatedLeave, out associatedholiday);
                CreateScheduleEvent(paycode, person.Id, dayShift, associatedLeave, associatedholiday);
            }

            #region Cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion

            #endregion
        }
        private async void HandleShiftStartAsync(object scheduleDayShift)
        {

                if (!(scheduleDayShift is ScheduleDayShift)) return;

                #region Time Delay ...

                var dayShift = (ScheduleDayShift)scheduleDayShift;

                //ShiftStartThreadDelay :  Got from configuration data
                //------------------------------------------------------------------//--------------------
                //Step 1 : Get all active scheduled persons belongs to this shift day
                //Step 2 : Get distinct Values for [Come Late Grace Time] from step 1 from these returned persons

                var shiftPersonnel = TamamServiceBroker.SchedulesHandler.GetScheduleActivePersons(dayShift.ScheduleDay.ScheduleId, dayShift.BaseDate, requestContext).Result;

                if (shiftPersonnel == null) return;
                if (dayShift.Shift.IsFlexible)
                {
                    await HandleShiftStartByGraceTimeAsync(dayShift, new TimeSpan(), shiftPersonnel.Select(x => x.Id).ToList());
                }
                else
                {
                    var personGraceModels = new List<PersonGraceTimeModel>();
                    
                    foreach (var person in shiftPersonnel)
                    {
                        //Get Grace time for each person
                        var response = SystemBroker.SchedulesHandler.GetShiftPolicy(person.Id, dayShift.Shift);
                        if (response.Result == null) continue;

                        //var shiftPolicies = TamamServiceBroker.LeavesHandler.GetShiftPolicies(person.Id);
                        //var shiftPolicy = new ShiftPolicy( shiftPolicies.Result[0] );

                        var shiftPolicy = new ShiftPolicy(response.Result);
                       
                        var personGraceModel = new PersonGraceTimeModel
                        {
                            PersonId = person.Id,
                            GraceTime = TimeSpan.FromMinutes(shiftPolicy.LateComeGrace)
                        };
                        personGraceModels.Add(personGraceModel);
                    }

                    if (!personGraceModels.Any()) return;

                    // Grouping Grace times with its related person(s)
                    var groupedPersonsList = personGraceModels.GroupBy(p => p.GraceTime,
                        p => p.PersonId,
                        (key, g) => new
                        {
                            GraceTime = key,
                            PersonIds = g.ToList()
                        });

                    groupedPersonsList = groupedPersonsList.OrderBy(p => p.GraceTime).ToList();

                    // Apply Absent Pay Code to each persons group according to its distinct Grace time
                    foreach (var personnelGroup in groupedPersonsList)
                    {
                        await HandleShiftStartByGraceTimeAsync(dayShift, personnelGroup.GraceTime, personnelGroup.PersonIds);
                    }
                }

                #endregion           
        }
        private async Task HandleShiftStartByGraceTimeAsync(ScheduleDayShift dayShift, TimeSpan comeLateGraceTime, List<Guid> personnelIds)
        {
            var timeForShiftStart = dayShift.Shift.IsFlexible ? DateTime.Now.Date.AddDays(1).AddMinutes(-5).TimeOfDay : dayShift.Shift.StartTime.Value.TimeOfDay + comeLateGraceTime;
            var timeForDay = dayShift.BaseDate.TimeOfDay;

            //Debug.WriteLine("timeForShiftStart: " + timeForShiftStart.ToString());
            //Debug.WriteLine("timeForDay: " + timeForDay.ToString());
            //Debug.WriteLine("delay: " + (timeForShiftStart - timeForDay).ToString());

            //if the date need to handled in less than today -service try to handle old data- so there is no delay time it must handled now..         
            if (dayShift.BaseDate.Date >= DateTime.Now.Date && timeForShiftStart >= timeForDay)
            {
                var delay = timeForShiftStart - timeForDay;

                #region LOG

                XLogger.Info("delay for Shift Start :" + dayShift.Shift.Name + " Hours: " + delay.TotalHours);

                #endregion

                await Task.Delay(delay);
            }

            #region LOG

            XLogger.Info("Start Handling Shift Start : " + dayShift.Shift.Name + ". " + dayShift.BaseDate);

            #endregion

            #region create schedule events ...

            if (personnelIds == null || !personnelIds.Any()) return;

            foreach (var personId in personnelIds)
            {
                #region Wait for a person to process his normal attendance

                if (ShiftGraceInProgressFlagForPerson == personId.ToString())
                {
                    await Task.Delay(new TimeSpan(0, 0, 5));
                }

                #endregion

                var SE = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, dayShift.Shift.Id, dayShift.BaseDate.Date).Result;
                if (SE != null) continue;

                Leave associatedLeave; Holiday associatedholiday;
                var paycode = GetPayCode(personId, dayShift.BaseDate.Date, dayShift, out associatedLeave,out associatedholiday);
                CreateScheduleEvent(paycode, personId, dayShift, associatedLeave, associatedholiday);

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            }

            #endregion
        }
        private async void HandleShiftEnd(object scheduleDayShift)
        {
            if (!(scheduleDayShift is ScheduleDayShift)) return;

            #region Time Delay ...

            var dayShift = (ScheduleDayShift)scheduleDayShift;
            var dayShiftEndTime = dayShift.Shift.IsFlexible ? dayShift.BaseDate.Date.AddDays(1).AddMinutes(-5) : dayShift.BaseDate.Date.AddTicks(dayShift.Shift.EndTime.Value.TimeOfDay.Ticks);
            var timeForShiftEnd = dayShift.Shift.IsFlexible ? dayShiftEndTime : ((dayShiftEndTime) + ShiftEndThreadDelay).AddDays(dayShift.Shift.IsNightShift ? 1 : 0);
            var timeForNow = DateTime.Now;

            //Debug.WriteLine("timeForShiftEnd: " + timeForShiftEnd.ToString());
            //Debug.WriteLine("timeForNow: " + timeForNow.ToString());
            //Debug.WriteLine("delay: " + (timeForShiftEnd - timeForNow).ToString());

            if (timeForShiftEnd >= timeForNow)
            {
                var delay = timeForShiftEnd - timeForNow;

                #region LOG

                XLogger.Info("Delay for Shift End :" + dayShift.Shift.Name + " Hours: " + delay.TotalHours.ToString());

                #endregion

                await Task.Delay(delay);
            }

            #region LOG

            XLogger.Info("Start Handling Shift End : " + dayShift.Shift.Name);

            #endregion

            #endregion

            #region mark currently in's to missed punches ...

            var personnel = TamamServiceBroker.SchedulesHandler.GetScheduleActivePersons(dayShift.ScheduleDay.ScheduleId, dayShift.BaseDate, requestContext).Result;
            if (personnel == null) return;

            foreach (var person in personnel)
            {
                var SE = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(person.Id, dayShift.Shift.Id, dayShift.BaseDate.Date).Result;
                if (SE == null) continue;

                // 'CurrentlyIn' => 'MissedPunch'
                if (SE.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn)
                {
                    SE.TotalStatusId = AttendanceCodes.ScheduleEventStatus.MissedOutPunch;
                    TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent(SE);
                    AttendanceHandler.StartViolationWorkflow(SE);
                    #region Cache

                    Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                    #endregion
                }
            }

            HandleMissedOutPunch();

            #endregion
        }
        private Guid GetPayCode(Guid personId, DateTime date, ScheduleDayShift dayShift, out Leave associatedLeave, out Holiday associatedholiday)
        {
            associatedLeave = null;
            associatedholiday = null;
            // leave ...
            var leave = SystemBroker.LeavesHandler.GetLeave(personId, date).Result;
            if (leave != null)
            {
                AttendanceContext.LeaveChangeToTaken(leave.Id, requestContext);
                if (leave.LeaveMode == LeaveMode.FullDay)
                {
                    associatedLeave = leave;
                    return AttendanceCodes.PayCodeStatus.OnLeave;
                }
            }
            var excuses = TamamServiceBroker.LeavesHandler.SearchExcuses(new ExcuseSearchCriteria
            {
                StartDate = date,
                EndDate = date,
                Personnel = new List<Guid> { personId },
                ExcuseStatuses = new List<int> { (int)ExcuseStatus.Approved, (int)ExcuseStatus.Taken },
                ActivePersonnelStatus = true,
                Duration = dayShift.Shift != null ? Convert.ToDouble(dayShift.Shift.Duration) : 0,
            }, requestContext).Result.Excuses;
            if (excuses.Count > 0)
            {
                foreach (var excuse in excuses)
                {
                    var duration = SystemBroker.SchedulesHandler.GetExcuseEffectiveTime(excuse, dayShift.Shift).Result;
                    if (duration > 0)
                    {
                        if (excuse.ExcuseStatusId == (int)ExcuseStatus.Approved) AttendanceContext.ExcuseChangeToTaken(excuse, requestContext);   // mark that excuse as taken
                        return AttendanceCodes.PayCodeStatus.OnAway;
                    }
                }
            }


            // holiday ...
            var holidays = TamamServiceBroker.OrganizationHandler.GetHolidays(personId, date, date, SystemRequestContext.Instance).Result;
            if (holidays != null && holidays.Count > 0)
            {
                associatedholiday = holidays[0];
                return AttendanceCodes.PayCodeStatus.OnHoliday;
            }

            // normal ...
            return AttendanceCodes.PayCodeStatus.NormalDay;
        }
        private List<ScheduleEvent> GetScheduleEventsDirty()
        {
            var response_dirty = SystemBroker.AttendanceHandler.GetScheduleEventsDirty();
            if (response_dirty.Type != ResponseState.Success)
            {
                XLogger.Error("Handle Dirty Task: GetDirtyScheduleEvents failed due to '" + response_dirty.Type + "'");
                return null;
            }

            return response_dirty.Result;
        }
        private List<ScheduleEvent> GetScheduleEvents(Guid personId, DateTime date)
        {
            var response_SEs = SystemBroker.AttendanceHandler.GetScheduleEvent(personId, date);
            if (response_SEs.Type != ResponseState.Success)
            {
                XLogger.Error("Handle Dirty Task: GetDirtyScheduleEvents failed due to '" + response_SEs.Type + "'");
                return null;
            }

            return response_SEs.Result;
        }


        private List<ScheduleEvent> GetScheduleEventsMissedOut()
        {
            var response_getMissedOut = SystemBroker.AttendanceHandler.GetScheduleEventsMissedOut();
            if (response_getMissedOut.Type != ResponseState.Success)
            {
                XLogger.Error("Handle Dirty Task: GetScheduleEventsMissedOut failed due to '" + response_getMissedOut.Type + "'");
                return null;
            }

            return response_getMissedOut.Result;
        }

        private List<AttendanceRawData> GetAttendanceRaw(Guid personId, DateTime date, List<ScheduleEvent> SEs)
        {
            var response = SystemBroker.AttendanceHandler.GetAttendanceRaw(personId, date, true);
            if (response.Type != ResponseState.Success)
            {
                XLogger.Error("Handle Dirty Task: GetAttendanceRaw failed due to '" + response.Type + "'");
                return null;
            }
            var AERaws = response.Result;
            var uAERaws = new List<AttendanceRawData>();
            response = TamamServiceBroker.AttendanceHandler.GetAttendanceRaw(SEs.Select(x => x.Id).ToList(), true, this.requestContext);
            if (response.Type == ResponseState.Success)
            {
                uAERaws = XModel.DistinctBy(AERaws.Concat(response.Result), new Func<AttendanceRawData, Guid>(item => item.Id)).ToList();
            }

            var personnelHandlerResult = TamamServiceBroker.PersonnelHandler.GetPerson(personId, requestContext).Result;
            var filteredRawDataAttences = new List<AttendanceRawData>();           
            uAERaws = uAERaws.OrderBy(x => x.AttendanceDateTime).ToList();
            var duplicatePunchThreshold = 0;
            if (!int.TryParse(Broker.ConfigurationHandler.GetValue(ADS.Common.Constants.TamamEngineConfig.Section, ADS.Common.Constants.TamamEngineConfig.DuplicatePunchThreshold), out duplicatePunchThreshold)) duplicatePunchThreshold = 0;                   
            for (int i = 0; i < uAERaws.Count; i++)
            {
                // Skip the raw data attendance that its date before person join date, and raw attendance less than duplicate Punch Threshold
                if (personnelHandlerResult.AccountInfo.JoinDate != null && uAERaws[i].AttendanceDateTime >= personnelHandlerResult.AccountInfo.JoinDate.Value)
                {
                    if (filteredRawDataAttences.Count > 0)
                    {
                        var previousRawattendance = filteredRawDataAttences[filteredRawDataAttences.Count - 1];
                        var timeDiff = uAERaws[i].AttendanceDateTime - previousRawattendance.AttendanceDateTime;
                        if (timeDiff.TotalSeconds > duplicatePunchThreshold)
                        {
                            filteredRawDataAttences.Add(uAERaws[i]);
                        }
                    }
                    else
                        filteredRawDataAttences.Add(uAERaws[i]);
                }                               
            }

            return filteredRawDataAttences;
        }
        private void CreateScheduleEvent(Guid payCode, Guid personId, ScheduleDayShift dayShift, Leave associatedLeave, Holiday associatedholiday)
        {
            ScheduleEvent SE = null;
            bool hasViolation = false;

            #region leave

            if (payCode == AttendanceCodes.PayCodeStatus.OnLeave)
            {
                SE = new ScheduleEvent
                {
                    EventDate = dayShift.BaseDate.Date,
                    PersonId = personId,
                    PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnLeave,
                    LeaveTypeId = associatedLeave.LeaveTypeId
                };

                if (dayShift.Shift != null)
                {
                    var duration = (int)AttendanceContext.GetShiftDuration(dayShift.Shift);

                    SE.Hours = duration;
                    SE.CalculatedHours = duration;
                    SE.ShiftId = dayShift.Shift.Id;
                    SE.WorkingHoursExpected = Round(dayShift.Shift.Duration * 60);
                }
            }

            #endregion

            #region Away Or Excuse

            if (payCode == AttendanceCodes.PayCodeStatus.OnAway)
            {
                SE = new ScheduleEvent
                {
                    EventDate = dayShift.BaseDate.Date,
                    PersonId = personId,
                    PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnAway,
                };

                if (dayShift.Shift != null)
                {
                    var duration = (int)AttendanceContext.GetShiftDuration(dayShift.Shift);

                    SE.Hours = duration;
                    SE.CalculatedHours = duration;
                    SE.ShiftId = dayShift.Shift.Id;
                    SE.WorkingHoursExpected = Round(dayShift.Shift.Duration * 60);
                }
            }

            #endregion

            #region holiday

            else if (payCode == AttendanceCodes.PayCodeStatus.OnHoliday)
            {
                SE = new ScheduleEvent
                {
                    EventDate = dayShift.BaseDate.Date,
                    PersonId = personId,
                    PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnHoliday,
                    HolidayName = associatedholiday!= null ? associatedholiday.Name:null,
                    HolidayNameCultureVariant = associatedholiday != null ? associatedholiday.NameCultureVariant : null,

                };

                if (dayShift.Shift != null)
                {
                    var duration = (int)AttendanceContext.GetShiftDuration(dayShift.Shift);

                    SE.Hours = duration;
                    SE.CalculatedHours = duration;
                    SE.ShiftId = dayShift.Shift.Id;
                    SE.WorkingHoursExpected = Round(dayShift.Shift.Duration * 60);
                }
            }

            #endregion
            #region normal

            else if (payCode == AttendanceCodes.PayCodeStatus.NormalDay)
            {
                SE = new ScheduleEvent
                {
                    Id = Guid.NewGuid(),
                    EventDate = dayShift.BaseDate.Date,
                    PersonId = personId,
                    PayCodeStatusId = AttendanceCodes.PayCodeStatus.NormalDay,
                    TotalStatusId = AttendanceCodes.ScheduleEventStatus.Absent
                };

                if (dayShift.Shift != null)
                {
                    SE.ExpectedIn = dayShift.Shift.StartTime;
                    SE.ExpectedOut = dayShift.Shift.EndTime;
                    SE.ShiftId = dayShift.Shift.Id;
                    SE.WorkingHoursExpected = Round(dayShift.Shift.Duration * 60);
                }

                hasViolation = true;
            }

            #endregion

            if (SE != null) TamamServiceBroker.AttendanceHandler.CreateScheduleEvent(SE);
            if (hasViolation) AttendanceHandler.StartViolationWorkflow(SE);
        }
        private void CreateScheduleEventForWeekend(ScheduleDay day, DateTime baseDate, bool isConcurrent)
        {
            #region LOG

            var preMessage = "Add Weekend Event for day :" + baseDate.ToShortDateString();
            XLogger.Info(preMessage);

            #endregion

            var personnel = TamamServiceBroker.SchedulesHandler.GetScheduleActivePersons(day.ScheduleId, baseDate, requestContext).Result;
            if (personnel == null) return;

            foreach (var person in personnel)
            {
                #region LOG

                XLogger.Info(preMessage + " : check if not schedule event found add weekend event. Person: " +
                             person.Id);

                #endregion

                if (day.DayShifts != null && day.DayShifts.Count > 0)
                {
                    foreach (var shift in day.DayShifts.Select(d => d.Shift))
                    {
                        var SE =
                            TamamServiceBroker.AttendanceHandler.GetScheduleEvent(person.Id, shift.Id, baseDate.Date)
                                .Result;
                        if (SE == null)
                        {
                            SE = new ScheduleEvent
                            {
                                EventDate = baseDate.Date,
                                PersonId = person.Id,
                                PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnWeekend
                            };
                            var duration = AttendanceContext.GetShiftDuration(shift);
                            SE.Hours = (int)duration;
                            SE.CalculatedHours = (int)duration;
                            SE.ShiftId = shift.Id;
                            SE.ExpectedIn = shift.StartTime;
                            SE.ExpectedOut = shift.EndTime;
                            SE.WorkingHoursExpected = 0; // Round( shift.Duration * 60 );
                            TamamServiceBroker.AttendanceHandler.CreateScheduleEvent(SE);
                        }
                        else
                        {
                            var scheduleDayShift = new ScheduleDayShift
                            {
                                ScheduleDay = day,
                                Shift = shift,
                                BaseDate = baseDate
                            };

                            if (isConcurrent)
                            {
                                var shiftEndTask = new Task(() => HandleShiftEndForPerson(scheduleDayShift, person.Id));
                                shiftEndTask.Start();
                            }
                            else
                            {
                                //HandleShiftEnd(scheduleDayShift);
                                HandleShiftEndForPerson(scheduleDayShift, person.Id);
                            }
                        }
                    }
                }
                else
                {
                    //check if no schedule event found, add weekend event.
                    var SES = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(person.Id, baseDate.Date).Result;
                    if (SES == null || SES.Count == 0)
                    {
                        var SE = new ScheduleEvent
                        {
                            EventDate = baseDate.Date,
                            PersonId = person.Id,
                            PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnWeekend
                        };
                        TamamServiceBroker.AttendanceHandler.CreateScheduleEvent(SE);
                    }
                }
            }
        }

        //private bool DeleteScheduleEvents(ScheduleEvent SE)
        //{
        //    // get all SEs in that day ...
        //    // cancel and delete their WFs
        //    // delete them (like the current code is doing)
        //    // done


        //    AttendanceHandler.CancelViolationWorkflow(SE);
        //    //var response_delete = SystemBroker.AttendanceHandler.DeleteScheduleEvent(SE);
        //    var response_delete = SystemBroker.AttendanceHandler.DeleteScheduleEvents(SE.PersonId, SE.EventDate);
        //    if (response_delete.Type != ResponseState.Success)
        //    {
        //        #region LOG

        //        XLogger.Error("Handle Dirty Task: DeleteScheduleEvent failed due to '" + response_delete.Type.ToString() +
        //                      "'");

        //        #endregion

        //        return false;
        //    }

        //    #region LOG

        //    XLogger.Info(string.Format("Handle Dirty Task: Old Schedule Event Deleted. Start re-process raw events."));

        //    #endregion

        //    return true;
        //}


        private bool DeleteScheduleEvents(List<ScheduleEvent> SEs)
        {
            // get all SEs in that day ...
            // cancel and delete their WFs
            // delete them (like the current code is doing)
            // done
            foreach (var SE in SEs)
            {
                AttendanceHandler.CancelViolationWorkflow(SE);
                var response_delete = SystemBroker.AttendanceHandler.DeleteScheduleEvent(SE);
                if (response_delete.Type != ResponseState.Success)
                {
                    #region LOG

                    XLogger.Error("Handle Dirty Task: DeleteScheduleEvent failed due to '" + response_delete.Type.ToString() +
                                  "'");

                    #endregion

                    return false;
                }
            }



            #region LOG

            XLogger.Info(string.Format("Handle Dirty Task: Old Schedule Event Deleted. Start re-process raw events."));

            #endregion

            return true;
        }
        private int Round(decimal value)
        {
            var round = Math.Round(value, MidpointRounding.AwayFromZero);
            return (int)round;
        }

        #endregion
        #region Helpers

        public void CalculateDayForPerson(DateTime date, Guid personId)
        {
            #region LOG

            var preMessage = string.Format("Handle Calculation for day: {0} and person {1}", date.ToShortDateString(),
                personId);
            XLogger.Info(preMessage);

            #endregion

            var response = TamamServiceBroker.SchedulesHandler.GetPersonScheduleDay(personId, date.Date, requestContext);
            if (response.Type == ResponseState.Success)
            {
                var scheduleDay = response.Result;
                CalculateDayForPerson(date, personId, scheduleDay);
            }
            else
            {
                #region LOG

                XLogger.Error(preMessage + ": GetPersonScheduleDay failed due to '" + response.Type.ToString() + "'");

                #endregion
            }
        }
        private void CalculateDayForPerson(DateTime date, Guid personId, ScheduleDay scheduleDay)
        {
            #region LOG

            var preMessage = "Handle Schedule day :" + date.ToShortDateString();
            XLogger.Info(preMessage);

            #endregion

            if (scheduleDay.IsDayOff)
            {
                #region LOG

                XLogger.Info(preMessage + " : Handle day off.");

                #endregion

                CreatScheduleEventForWeekendForPerson(scheduleDay, personId, date);

                #region cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            }
            else
            {
                if (scheduleDay.DayShifts != null)
                {
                    #region LOG

                    XLogger.Info(preMessage + " : Handle Shift(s)");

                    #endregion

                    foreach (var dayShift in scheduleDay.DayShifts.Select(s => s.Shift))
                    {
                        #region LOG

                        XLogger.Info(preMessage + " : Handle Shift: " + dayShift.Name + ". Day:" + date.ToString());

                        #endregion

                        var scheduleDayShift = new ScheduleDayShift
                        {
                            ScheduleDay = scheduleDay,
                            Shift = dayShift,
                            BaseDate = date
                        };
                        HandleShiftStartForPerson(scheduleDayShift, personId);
                        HandleShiftEndForPerson(scheduleDayShift, personId);
                    }
                }
            }
        }
        private async void HandleShiftStartForPerson(object scheduleDayShift, Guid personId)
        {
            if (!(scheduleDayShift is ScheduleDayShift)) return;

            #region Time Delay ...

            var dayShift = (ScheduleDayShift)scheduleDayShift;

            var response = SystemBroker.SchedulesHandler.GetShiftPolicy(personId, dayShift.Shift);
            if (response.Result == null) return;
            var shiftPolicy = new ShiftPolicy(response.Result);

            var comeGrace = TimeSpan.FromMinutes(shiftPolicy.LateComeGrace);


            var timeForShiftStart = dayShift.Shift.IsFlexible ? DateTime.Now.Date.AddDays(1).AddMinutes(-5).TimeOfDay : dayShift.Shift.StartTime.Value.TimeOfDay + comeGrace;
            var timeForDay = dayShift.BaseDate.TimeOfDay;

            //Debug.WriteLine("comeGrace: " + comeGrace.ToString());
            //Debug.WriteLine("timeForShiftStart: " + timeForShiftStart.ToString());
            //Debug.WriteLine("timeForDay: " + timeForDay.ToString());
            //Debug.WriteLine("delay: " + (timeForShiftStart - timeForDay).ToString());

            if (dayShift.BaseDate.Date >= DateTime.Now.Date && timeForShiftStart >= timeForDay)
            {
                var delay = timeForShiftStart - timeForDay;

                #region LOG

                XLogger.Info("delay for Shift Start :" + dayShift.Shift.Name + " Hours: " + delay.TotalHours.ToString());

                #endregion

                await Task.Delay(delay);
            }

            #region LOG

            XLogger.Info("Start Handling Shift Start : " + dayShift.Shift.Name + ". " + dayShift.BaseDate.ToString());

            #endregion

            #endregion

            #region create schedule events ...
            #region Wait for a person to process his normal attendance

            if (ShiftGraceInProgressFlagForPerson == personId.ToString())
            {
                await Task.Delay(new TimeSpan(0, 0, 5));
            }

            #endregion
            var SE = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, dayShift.Shift.Id, dayShift.BaseDate.Date).Result;
            if (SE != null) return;

            Leave associatedLeave; Holiday associatedholiday;
            var paycode = GetPayCode(personId, dayShift.BaseDate.Date, dayShift, out associatedLeave, out associatedholiday);
            CreateScheduleEvent(paycode, personId, dayShift, associatedLeave, associatedholiday);

            #endregion
            #region cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion
        }
        private async void HandleShiftEndForPerson(object scheduleDayShift, Guid personId)
        {
            if (!(scheduleDayShift is ScheduleDayShift)) return;

            #region Time Delay ...

            var dayShift = (ScheduleDayShift)scheduleDayShift;
            var dayShiftEndTime = dayShift.Shift.IsFlexible ? DateTime.Now.Date.AddDays(1).AddMinutes(-5) : dayShift.BaseDate.Date.AddTicks(dayShift.Shift.EndTime.Value.TimeOfDay.Ticks);
            var timeForShiftEnd = ((dayShiftEndTime) + ShiftEndThreadDelay).AddDays(dayShift.Shift.IsNightShift ? 1 : 0);
            var timeForNow = DateTime.Now;

            //var timeForShiftEnd = ( new DateTime().AddTicks( dayShift.Shift.EndTime.Value.TimeOfDay.Ticks ) ) + _ShiftEndThreadDelay;
            //if ( dayShift.Shift.IsNightShift ) timeForShiftEnd = timeForShiftEnd.AddDays( 1 );
            //var timeForDay = new DateTime().AddTicks( dayShift.BaseDate.TimeOfDay.Ticks );

            //Debug.WriteLine( "timeForShiftEnd: " + timeForShiftEnd.ToString() );
            //Debug.WriteLine( "timeForNow: " + timeForNow.ToString() );
            //Debug.WriteLine( "delay: " + ( timeForShiftEnd - timeForNow ).ToString() );

            if (timeForShiftEnd >= timeForNow)
            {
                var delay = timeForShiftEnd - timeForNow;

                #region LOG

                XLogger.Info("Delay for Shift End :" + dayShift.Shift.Name + " Hours: " + delay.TotalHours.ToString());

                #endregion

                await Task.Delay(delay);
            }

            #region LOG

            XLogger.Info("Start Handling Shift End : " + dayShift.Shift.Name);

            #endregion

            #endregion

            #region mark currently in's to missed punches ...

            var SE =
                TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, dayShift.Shift.Id,
                    dayShift.BaseDate.Date).Result;
            if (SE == null) return;

            // 'CurrentlyIn' => 'MissedPunch'
            if (SE.TotalStatusId == AttendanceCodes.ScheduleEventStatus.CurrentlyIn)
            {
                SE.TotalStatusId = AttendanceCodes.ScheduleEventStatus.MissedOutPunch;
                TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent(SE);
                AttendanceHandler.StartViolationWorkflow(SE);
            }

            #endregion
            #region cache

            Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

            #endregion
        }
        private void CreatScheduleEventForWeekendForPerson(ScheduleDay day, Guid personId, DateTime baseDate)
        {
            #region LOG

            var preMessage = "Add Weekend Event for day :" + baseDate.ToShortDateString();
            XLogger.Info(preMessage);
            XLogger.Info(preMessage + " : check if not schedule event found add weekend event. Person: " + personId.ToString());

            #endregion

            if (day.DayShifts != null && day.DayShifts.Count > 0)
            {
                foreach (var shift in day.DayShifts.Select(d => d.Shift))
                {
                    var SE = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, shift.Id, baseDate.Date).Result;
                    if (SE == null)
                    {
                        SE = new ScheduleEvent
                        {
                            EventDate = baseDate.Date,
                            PersonId = personId,
                            PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnWeekend
                        };
                        var duration = AttendanceContext.GetShiftDuration(shift);
                        SE.Hours = (int)duration;
                        SE.CalculatedHours = (int)duration;
                        SE.ShiftId = shift.Id;
                        SE.ExpectedIn = shift.StartTime;
                        SE.ExpectedOut = shift.EndTime;
                        SE.WorkingHoursExpected = 0; // Round( shift.Duration * 60 );
                        TamamServiceBroker.AttendanceHandler.CreateScheduleEvent(SE);
                    }
                    else
                    {
                        var scheduleDayShift = new ScheduleDayShift
                        {
                            ScheduleDay = day,
                            Shift = shift,
                            BaseDate = baseDate
                        };
                        //HandleShiftEnd(scheduleDayShift);
                        HandleShiftEndForPerson(scheduleDayShift, personId);
                    }
                }
            }
            else
            {
                //check if no schedule event found, add weekend event.
                var SES = TamamServiceBroker.AttendanceHandler.GetScheduleEvent(personId, baseDate.Date).Result;
                if (SES == null || SES.Count == 0)
                {
                    var SE = new ScheduleEvent
                    {
                        EventDate = baseDate.Date,
                        PersonId = personId,
                        PayCodeStatusId = AttendanceCodes.PayCodeStatus.OnWeekend
                    };
                    TamamServiceBroker.AttendanceHandler.CreateScheduleEvent(SE);
                }
            }
        }

        #endregion
    }
}