using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace XCore.Services.Audit.API.Health.Checkers
{
    public class MemoryHealthCheck : IHealthCheck
    {
        #region ...

        private readonly IOptionsMonitor<MemoryCheckOptions> _options;
        public string Name => "memory_check";

        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
        {
            _options = options;
        }

        #endregion
        #region IHealthCheck.

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = _options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
            {
                { "AllocatedBytes", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
            };
            var status = (allocated < options.Threshold) ?
                HealthStatus.Healthy : context.Registration.FailureStatus;

            return Task.FromResult(new HealthCheckResult(
                status,
                description: $"Reports degraded status if allocated bytes >= {options.Threshold} bytes.",
                exception: null,
                data: data));
        }

        #endregion
        #region nested.

        public class MemoryCheckOptions
        {
            // Failure threshold (in bytes)
            public long Threshold { get; set; } = 1024L * 1024L * 1024L;
        }

        #endregion
    }
}
