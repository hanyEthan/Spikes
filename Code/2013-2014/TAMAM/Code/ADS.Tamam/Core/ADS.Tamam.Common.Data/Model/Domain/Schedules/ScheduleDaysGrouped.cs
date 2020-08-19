using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleDaysGrouped
    {
        #region props.

        public List<ScheduleDaysGroup> Schedules { get; set; }
        
        #endregion
        #region cst.

        public ScheduleDaysGrouped()
        {
            this.Schedules = new List<ScheduleDaysGroup>();
        }
        
        #endregion
        #region helpers.

        public void Add( Schedule schedule , List<ScheduleDay> scheduleDays )
        {
            if ( schedule == null || scheduleDays == null ) return;

            var group = this.Schedules.Where( x => x.Schedule.Id == schedule.Id ).FirstOrDefault();
            if ( group != null ) group.ScheduleDays.AddRange( scheduleDays );
            else this.Schedules.Add( new ScheduleDaysGroup() { Schedule = schedule , ScheduleDays = scheduleDays , } );
        }
        
        #endregion
    }

    [Serializable]
    public class ScheduleDaysGroup
    {
        #region props.

        public Schedule Schedule { get; set; }
        public List<ScheduleDay> ScheduleDays { get; set; }
        
        #endregion
        #region cst.

        public ScheduleDaysGroup()
        {
            this.ScheduleDays = new List<ScheduleDay>();
        }
        
        #endregion
    }
}
