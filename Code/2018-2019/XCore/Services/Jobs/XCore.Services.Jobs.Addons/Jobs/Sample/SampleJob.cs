using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Scheduler.Contracts;
using XCore.Framework.Utilities;

namespace XCore.Services.Jobs.Addons.Jobs.Sample
{
    public class SampleJob : IScheduledJob
    {
        #region props.

        public bool Initialized { get; private set; }

        #endregion
        #region cst.

        public SampleJob()
        {
            this.Initialized = Initialize();

            #region LOG

            if (this.Initialized)
            {
                XLogger.Info("SampleJob : job initialized correctly");
            }
            else
            {
                XLogger.Warning("SampleJob : job is not initialized correctly");
            }

            #endregion
        }

        #endregion

        #region IScheduledJob

        public async Task Execute()
        {
            try
            {
                #region LOG
                XLogger.Trace("Quarts : job : [SampleJob] : triggered");
                #endregion

                // ...

                #region LOG
                XLogger.Trace("Quarts : job : [AuditSyncJob] : sleeping.");
                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            return true;
        }

        #endregion
    }
}
