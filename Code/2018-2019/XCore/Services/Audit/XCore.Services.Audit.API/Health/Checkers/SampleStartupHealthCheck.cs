using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace XCore.Services.Audit.API.Health.Checkers
{
    public class SampleStartupHealthCheck : IHealthCheck
    {
        #region props.

        private volatile bool _taskCompleted = false;
        public bool TaskCompleted
        {
            get => _taskCompleted;
            set => _taskCompleted = value;
        }
        
        public string Name => "Sample Startup Health Check";

        #endregion
        #region IHealthCheck

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (this.TaskCompleted)
            {
                return HealthCheckResult.Healthy("The sample startup task is finished.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("The sample startup task is not finished.");
            }
        }

        #endregion
    }
}
