using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IInternalServiceHandler : IBaseHandler
    {
        ExecutionResponse<bool> CreateJob( Guid scheduleId , Guid scheduleEventId , DateTime time );
        ExecutionResponse<bool> DeleteJob( List<Guid> eventsId , Guid scheduleId );
        ExecutionResponse<bool> DeleteJob( Guid scheduleId );
    }
}
