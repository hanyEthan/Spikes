using System.Threading.Tasks;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Services.Audit
{
    public class AuditService : IAuditService
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly ILogger<AuditService> _logger;
        private readonly IAuditClient _auditClient;

        #endregion
        #region cst.

        public AuditService(IAuditClient auditClient,
                            ILogger<AuditService> logger)
        {
            this._logger = logger;
            this._auditClient = auditClient;

            this.Initialized = Initialize();
        }

        #endregion

        #region IAuditService

        public async Task CreateAsync(IAuditMessage request)
        {
            await this._auditClient.CreateAsync(request);
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (_auditClient?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}
