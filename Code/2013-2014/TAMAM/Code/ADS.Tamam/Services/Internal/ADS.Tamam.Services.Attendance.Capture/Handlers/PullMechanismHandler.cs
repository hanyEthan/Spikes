using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Services.DataAcquisition.DataHandler;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Modules.Attendance.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Handlers;

namespace ADS.Tamam.Services.DataAcquisition.Handlers
{
    public class PullMechanismHandler
    {
        #region props ...

        private PullMechanismDataHandler _DataHandler;
        private AttendanceDataHandler _AttendanceDataHandler;
        private PersonnelDataHandler _PersonnelDataHandler;
        private readonly ISystemAttendanceHandler _SystemAttendanceHandler;
    
        #endregion
        #region cst ...

        public PullMechanismHandler()
        {
            _DataHandler = new PullMechanismDataHandler();
            _AttendanceDataHandler = new AttendanceDataHandler();
            _PersonnelDataHandler = new PersonnelDataHandler();
            _SystemAttendanceHandler = new AttendanceHandler();

        }
        #endregion

        public void TransferData()
        {

            bool autoDetectedType;
            if (!bool.TryParse(Broker.ConfigurationHandler.GetValue(ADS.Common.Constants.TamamCaptureConfig.Section, ADS.Common.Constants.TamamCaptureConfig.TypeAutoDetectionMode),
                out autoDetectedType)) autoDetectedType = false;
        

            var rawEvnets = _DataHandler.GetUnProcessedEvents();

            foreach (var item in rawEvnets)
            {
                var personIdResult = _PersonnelDataHandler.GetPersonIdBySecurityId(item.PersonId);
                if (personIdResult.Type == ResponseState.Success)
                {
                    var attendanceRawData = new AttendanceRawData
                     {
                         PersonId = personIdResult.Result,
                         AttendanceDateTime = item.EventDate,
                         CreationDate = DateTime.Now,
                         IsProcessed = false,
                         TerminalId = item.TerminalId,
                         Type = (AttendanceEventType)item.EventType,
                         IsOriginal = true,
                         AttendanceSource = item.Logsource,
                         AttendanceOrgin = item.LogOrgin,                        
                         ConsiderAsAttendance = item.ConsiderLogForAttendance == null ? true : item.ConsiderLogForAttendance.ToLower().Trim() == "n" ? false : true,
                         //Location = item.LocationName
                     };
                    ExecutionResponse<Guid> result = new ExecutionResponse<Guid>();
                    if (!autoDetectedType)
                        result = _AttendanceDataHandler.CreateAttendanceRaw(attendanceRawData);
                    else
                    {
                        attendanceRawData.Type = AttendanceEventType.NotSet;                  
                        result = _AttendanceDataHandler.CreateAttendanceRaw(attendanceRawData);                     
                    }
                    if (result.Type == ResponseState.Success && InsertTerminalWithLocation(item.TerminalId, item.LocationName))
                        _DataHandler.MarkRawEventAsProcessed(item.Id);
                }
                else
                {
                    XLogger.Warning(string.Format("Found Log Events for Unknown Person. Record Id: {0}, Raw Person Id: {1}", item.Id, item.PersonId));
                }
            }
        }

        private bool InsertTerminalWithLocation(string terminalId, string locationName)
        {
            try
            {
                if (string.IsNullOrEmpty(terminalId)) terminalId = "";
                if (string.IsNullOrEmpty(locationName)) locationName = "";
                return _AttendanceDataHandler.AddTerminalWithLocation(terminalId.Trim(), locationName.Trim()).Result;
            }
            catch (Exception e)
            {
                XLogger.Error(e.Message);
                return false;
            }
        }
    }
}
