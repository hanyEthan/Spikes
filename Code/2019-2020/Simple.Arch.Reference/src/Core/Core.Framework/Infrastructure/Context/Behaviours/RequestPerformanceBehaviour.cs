using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Logging.Helpers;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
    {
        #region props.

        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        #endregion
        #region cst.

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        #endregion
        #region IPipelineBehavior

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MediatR.RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userId = (request as BaseRequestContext)?.Header?.UserId;

                _logger.LogWarning("{ServiceName} : Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                                   LoggingHelpers.ServiceName, 
                                   requestName, 
                                   elapsedMilliseconds, 
                                   userId, 
                                   request);
            }

            return response;
        }

        #endregion
    }
}
