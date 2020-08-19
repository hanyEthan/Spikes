using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Logging.Helpers;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.Behaviours
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        #region props.

        private readonly ILogger _logger;

        #endregion
        #region cst.

        public RequestLogger(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        #endregion
        #region IRequestPreProcessor

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = (request as BaseRequestContext)?.Header?.UserId;

            _logger.LogDebug("{MicroserviceName} : (Request: {Name}) {@UserId} {@Request}", LoggingHelpers.ServiceName, requestName, userId, request);
        }

        #endregion
    }
}
