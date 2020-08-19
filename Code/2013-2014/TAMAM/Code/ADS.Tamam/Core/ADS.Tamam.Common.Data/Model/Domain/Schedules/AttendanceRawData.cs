using System;
using System.Collections.Generic;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class AttendanceRawData : IXSerializable
    {
        public Guid Id { get; set; }
        public DateTime AttendanceDateTime { get; set; }
        public AttendanceEventType Type { get; set; }
        public Guid PersonId { get; set; }
        public DateTime? CreationDate { get; set; }
        public string TerminalId { get; set; }
        public string Username { get; set; }
        public string RawComment { get; set; }
        public bool IsManual { get; set; }
        public bool IsOriginal { get; set; }
        public bool IsProcessed { get; set; }
        public string AttendanceSource { get; set; }
        public string AttendanceOrgin { get; set; }
        public bool ConsiderAsAttendance { get; set; }
        public bool HandleForShiftEnd { get; set; }
        public string Location { get; set; } 
        public ManualAttendanceStatus ManualAttendanceStatus { get; set; }
        public Person Person { get; set; }
        public IList<AttendanceRawDataHistoryItem> DataHistoryItems { get; set; }

        [XDontSerialize] public string PunchTypeLocalized
        {
            get
            {
                string punchType = Resources.Culture.Attendance.NotSet;
                switch ( Type )
                {
                    case AttendanceEventType.In:
                        punchType = Resources.Culture.Attendance.In;
                        break;
                    case AttendanceEventType.Out:
                        punchType = Resources.Culture.Attendance.Out;
                        break;
                }

                return punchType;
            }
        }

        #region cst ...

        public AttendanceRawData()
        {
            DataHistoryItems = new List<AttendanceRawDataHistoryItem>();
        }
        public AttendanceRawData(Guid id, DateTime dateTime, AttendanceEventType mode, Guid personId, string lastUpdatingUserId, string comment, bool isOriginal, bool isManual, bool isProcessed, bool considerAsAttendance)
        {
            Id = id;
            AttendanceDateTime = dateTime;
            Type = mode;
            PersonId = personId;
            Username = lastUpdatingUserId;
            RawComment = comment;
            IsOriginal = isOriginal;
            IsManual = isManual;
            ConsiderAsAttendance = considerAsAttendance;          
            IsProcessed = isProcessed;
            CreationDate = DateTime.Now;
            ManualAttendanceStatus = ManualAttendanceStatus.Pending;

            DataHistoryItems = new List<AttendanceRawDataHistoryItem>();
        }

        #endregion
        #region Helpers

        public void AddHistoryItem( AttendanceRawDataHistoryItem item )
        {
            if ( this.DataHistoryItems == null ) DataHistoryItems = new List<AttendanceRawDataHistoryItem>();

            item.AttendanceRawDataId = this.Id;

            this.DataHistoryItems.Add( item );
        }
        public void AddHistoryItem( Guid id, DateTime? oldValue , DateTime newValue ,Guid personId, DateTime updateTime , string changedByUserId , string comment , ManualAttendanceStatus status )
        {
            if ( this.DataHistoryItems == null ) DataHistoryItems = new List<AttendanceRawDataHistoryItem>();

            var item = new AttendanceRawDataHistoryItem( id , oldValue , newValue , personId , updateTime , changedByUserId , comment , status );

            item.AttendanceRawDataId = this.Id;
            this.DataHistoryItems.Add( item );
        }

        #endregion
    }
}