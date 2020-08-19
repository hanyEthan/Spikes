using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleEventSearchResult : IXSerializable
    {
        public List<ScheduleEvent> Result { get; set; }
        public long ResultTotalCount { get; set; }

        #region cst ...

        public ScheduleEventSearchResult()
        {
            Result = new List<ScheduleEvent>();
        }
        
        #endregion
    }
}