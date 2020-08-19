using System;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    public interface IEffectiveSchedule : XIntervals.ITimeRange
    {
        Guid Id { get; set; }
        Guid ScheduleId { get; set; }

        DateTime StartDate { get; set; }
        DateTime? EndDate { get; set; }
    }
}
