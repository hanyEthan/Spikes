using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using XCore.Framework.Framework.Scheduler.Contracts;
using XCore.Framework.Framework.Scheduler.Models;

namespace XCore.Framework.Framework.Scheduler.Handlers
{
    public class JobsScheduler
    {
        #region props.

        public bool Initialized { get; protected set; }
        public IScheduler Scheduler { get; set; }
        private NameValueCollection Configurations { get; set; }

        #endregion
        #region cst.

        public JobsScheduler()
        {
        }

        #endregion
        #region publics.

        public async Task Start()
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "JobsScheduler is not initialized correctly." );
            #endregion

            await this.Scheduler.Start();
        }
        public async Task Stop()
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "JobsScheduler is not initialized correctly." );
            #endregion

            await this.Scheduler.Shutdown();
        }
        public async Task Run<TJob>( string name , Interval settings , bool allowConcurrentOverlappedInstances = true ) where TJob : IScheduledJob, new()
        {
            var job = allowConcurrentOverlappedInstances
                    ? JobBuilder.Create<DefaultJob.Concurrent<TJob>>()
                                .WithIdentity( name , name + ".group" )
                                .Build()
                    : JobBuilder.Create<DefaultJob.NotConcurrent<TJob>>()
                                .WithIdentity( name , name + ".group" )
                                .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity( name + ".trigger" , name + ".group" )
                                        .StartNow()
                                        .WithSimpleSchedule( x => x
                                             .WithIntervalInSeconds( (int) settings.Step.TotalSeconds )
                                             .RepeatForever() )
                                        .Build();

            await this.Scheduler.ScheduleJob( job , trigger );
        }
        public async Task<bool> Initialize()
        {
            bool state = true;

            state = state && Config();
            state = state && LogSet();
            state = state && await SchedulerCreate();

            return this.Initialized = state;
        }

        #endregion
        #region helpers.

        private bool Config()
        {
            this.Configurations = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };

            return true;
        }
        private bool LogSet()
        {
            LogProvider.SetCurrentLogProvider( new XLogProvider() );

            return true;
        }
        private async Task<bool> SchedulerCreate()
        {
            var factory = new StdSchedulerFactory( this.Configurations );
            this.Scheduler = await factory.GetScheduler();

            return true;
        }

        #endregion
    }
}
