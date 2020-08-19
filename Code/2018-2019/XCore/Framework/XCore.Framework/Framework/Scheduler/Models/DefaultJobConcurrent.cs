using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using XCore.Framework.Framework.Scheduler.Contracts;

namespace XCore.Framework.Framework.Scheduler.Models
{
    internal class DefaultJob
    {
        #region Concurrent

        internal class Concurrent<T> : IJob where T : IScheduledJob, new()
        {
            #region props.

            public IScheduledJob Nested { get; set; }

            #endregion
            #region cst.

            public Concurrent()
            {
                this.Nested = new T();
            }

            #endregion
            #region IJob

            public Task Execute( IJobExecutionContext context )
            {
                return Nested.Execute();
            }

            #endregion
        }

        #endregion
        #region NotConcurrent

        [DisallowConcurrentExecution]
        internal class NotConcurrent<T> : Concurrent<T> where T : IScheduledJob, new()
        {
        }

        #endregion
    }
}
