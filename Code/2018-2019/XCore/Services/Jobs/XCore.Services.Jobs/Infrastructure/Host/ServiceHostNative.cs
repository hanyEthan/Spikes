using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Scheduler.Handlers;
using XCore.Framework.Framework.Scheduler.Models;
using XCore.Framework.Utilities;
using XCore.Services.Jobs.Addons.Jobs.Sample;

namespace XCore.Services.Jobs.Infrastructure.Host
{
    public class ServiceHostNative : IDisposable
    {
        #region props.

        public JobsScheduler Scheduler { get; set; }

        #endregion
        #region cst.

        public ServiceHostNative()
        {
        }

        #endregion
        #region helpers.

        private async Task<bool> Initialize()
        {
            #region Scheduler

            this.Scheduler = new JobsScheduler();

            var state = await this.Scheduler.Initialize();
            if (!state) return false;

            #endregion
            #region jobs.

            var status = true;
            // TODO : load jobs dynamically ...

            #region Sample Job.

            status = status && await Helpers.Jobs.AddJob_SampleJob(this.Scheduler);

            #endregion

            return status;

            #endregion
        }

        #region Jobs.

        private static class Helpers
        {
            public static class Jobs
            {
                public static async Task<bool> AddJob_SampleJob(JobsScheduler scheduler)
                {
                    //var minutes = XConfig.GetInt("Jobs.SampleJob.Duration.Minutes") ?? 5;  // 5 minutes.
                    var period = new TimeSpan(0, 5, 0);  // 5 minutes.

                    await scheduler.Run<SampleJob>(
                                    name: "SampleJob",
                                    settings: new Interval() { Step = period },
                                    allowConcurrentOverlappedInstances: false);

                    return true;
                }
            }
        }

        #endregion

        #endregion
        #region publics.

        public async Task Start()
        {
            try
            {
                #region LOG
                XLogger.Trace("");
                #endregion

                await Initialize();
                await this.Scheduler.Start();
            }
            #region catch
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
            }
            #endregion
        }
        public async Task Stop()
        {
            try
            {
                await this.Scheduler.Stop();
            }
            #region catch
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
            }
            #endregion
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            try
            {
                Scheduler.Stop();
            }
            catch
            {
            }
        }

        #endregion
    }
}
