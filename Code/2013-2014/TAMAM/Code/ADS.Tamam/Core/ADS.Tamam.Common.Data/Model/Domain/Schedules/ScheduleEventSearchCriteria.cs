using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleEventSearchCriteria : IXSerializable
    {
        public List<Guid> Personnel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? JustificationStatus { get; set; }

        public ScheduleEventSearchCriteria()
        {
            Personnel = new List<Guid>();
        }
        public ScheduleEventSearchCriteria( List<Guid> personnel , DateTime? startDate , DateTime? endDate , int? status )
        {
            this.Personnel = personnel;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.JustificationStatus = status;
        }

        #region Helpers

        public override string ToString()
        {
            return string.Format( "{0}_{1}_{2}_{3}" ,

                ListToString<Guid>( Personnel ) ,
                StartDate ,
                EndDate ,
                JustificationStatus );
        }
        private string ListToString<T>( List<T> list )
        {
            if ( list != null ) return string.Join( "," , list.ToArray() );
            return string.Empty;
        }

        #endregion
    }
}
