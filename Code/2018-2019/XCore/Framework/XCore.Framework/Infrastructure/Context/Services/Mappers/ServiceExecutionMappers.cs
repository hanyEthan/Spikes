using System;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Models;

namespace XCore.Framework.Infrastructure.Context.Services.Mappers
{
    public static class ServiceExecutionMappers
    {
        public static ServiceExecutionRequestDTO<T> Map<T>(RequestContext from)
        {
            if (from == null) return null;

            var to = new ServiceExecutionRequestDTO<T>()
            {
                RequestAppId = from.AppId,
                RequestClientToken = "634a5d14-81c1-4c9b-9484-0b3c21bb2299",
                RequestCorrelationCode = null, // todo
                RequestCulture = from.Culture,
                RequestMetadata = from.Metadata,
                RequestModuleId = from.ModuleId,
                RequestTime = DateTime.Now.ToString("YYYYMMDD hh:mm:ss"),
                RequestUserCode = from.UserId,
                RequestSessionCode = null, // todo
            };

            return to;
        }
    }
}
