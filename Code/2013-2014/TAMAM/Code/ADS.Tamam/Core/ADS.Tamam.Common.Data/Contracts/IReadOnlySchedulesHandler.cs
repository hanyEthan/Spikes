using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.DTO;
using ADS.Tamam.Common.Data.Context;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IReadOnlySchedulesHandler : IBaseHandler
    {
        ExecutionResponse<List<ScheduleDetailDTO>> GetScheduleDetailsDTOs( Guid personId , RequestContext requestContext );
        ExecutionResponse<bool> SetScheduleDetailsDTOs( List<ScheduleDetailDTO> DTOs , RequestContext requestContext );
    }
}
