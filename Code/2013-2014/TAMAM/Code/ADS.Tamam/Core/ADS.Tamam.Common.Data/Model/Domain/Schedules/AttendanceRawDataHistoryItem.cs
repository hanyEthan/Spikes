using System;
using System.Globalization;
using System.Threading;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class AttendanceRawDataHistoryItem : IWorkflowTarget , IXSerializable
    {
        public Guid Id { get; set; }

        public Guid AttendanceRawDataId { get; set; }
        public AttendanceRawData AttendanceRawData { get; set; }

        public DateTime? ValueOld { get; set; }
        public DateTime ValueNew { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ChangedByUserId { get; set; }
        public string HistoryComment { get; set; }
        public ManualAttendanceStatus ManualAttendanceStatus { get; set; }
        public Guid PersonId { get; set; }

        #region cst ...

        public AttendanceRawDataHistoryItem() { }
        public AttendanceRawDataHistoryItem( Guid id , DateTime? oldValue , DateTime newValue , Guid personId , DateTime updateTime , string changedByUserId , string comment , ManualAttendanceStatus status )
        {
            Id = id;
            ValueOld = oldValue;
            ValueNew = newValue;
            PersonId = personId;
            UpdateTime = updateTime;
            ChangedByUserId = changedByUserId;
            HistoryComment = comment;
            ManualAttendanceStatus = status;
        }

        #endregion
        # region IWorkflowTarget

        [XDontSerialize] public double EffectiveAmount
        {
            get { throw new NotImplementedException(); }
        }

        # endregion

        # region ToString

        public override string ToString()
        {
            var isLocalized = Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains( "ar" );

            string meta = isLocalized ? "{0} ({1}) تعدلت من {2} الى {3} بواسطة {4} بتاريخ {5}" : "{0} ({1}) changed from {2} to {3} by {4} at date {5}";

            string Attendance = isLocalized ? "الحضور و الانصراف" : "Attendance";
            string Type = TypeLocalized( AttendanceRawData.Type , isLocalized );
            string From = string.Empty;
            if ( ValueOld.HasValue )
            {
                From = isLocalized ? ValueOld.Value.ToString( "hh:mm tt" , CultureInfo.CreateSpecificCulture( "ar-EG" ) ) : ValueOld.Value.ToString( "hh:mm tt" , CultureInfo.CreateSpecificCulture( "en-US" ) );
            }
            else
            {
                From = isLocalized ? "فارغ" : "Empty";
            }

            string To = isLocalized ? ValueNew.ToString( "hh:mm tt" , CultureInfo.CreateSpecificCulture( "ar-EG" ) ) : ValueNew.ToString( "hh:mm tt" , CultureInfo.CreateSpecificCulture( "en-US" ) );
            string By = ChangedByUserId;
            string Date = UpdateTime.ToString();

            return string.Format( meta , Attendance , Type , From , To , By , Date );
        }
        private string TypeLocalized( AttendanceEventType type , bool isLocalized )
        {
            if ( isLocalized )
            {
                if ( type == AttendanceEventType.In ) return "دخول";
                if ( type == AttendanceEventType.Out ) return "خروج";
                return string.Empty;
            }

            return type.ToString();
        }

        # endregion
    }
}
