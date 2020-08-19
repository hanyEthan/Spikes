using System;
using System.Collections.Generic;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Context;
using Quartz;
using Quartz.Impl;

using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Common.Context;
using Quartz.Impl.Matchers;

namespace ADS.Tamam.Common.Handlers.Automation
{
    public class QuartzHandler : IBaseHandler
    {
        # region prop

        public bool Initialized { get; private set; }
        public string Name { get { return "QuartzHandler"; } }

        private ISchedulerFactory schedulerFactory;
        private IScheduler scheduler;

        private static QuartzHandler _instance;
        public static QuartzHandler Instance
        {
            get
            {
                //lock (Instance)
                {
                    return _instance ?? ( _instance = new QuartzHandler() );
                }
            }
        }

        # endregion
        #region cst ...

        private QuartzHandler()
        {
            try
            {
                XLogger.Info( "QuartzHandler ..." );

                Initialize();
                Start();

                XLogger.Info( "QuartzHandler ... Quartz Started." );

                Initialized = true;
            }
            catch(Exception x)
            {
                XLogger.Error( "QuartzHandler ... Exception : " + x );
            }
        }
        
        #endregion
        #region publics

        public void Initialize()
        {
            schedulerFactory = new StdSchedulerFactory();
            scheduler = schedulerFactory.GetScheduler();
        }
        public void Start()
        {
            if ( !scheduler.IsStarted ) scheduler.Start();
        }
        public void Stop()
        {
            if ( !scheduler.IsShutdown ) scheduler.Shutdown();
        }
        public void Pause()
        {
            scheduler.PauseAll();
        }
        public void Resume()
        {
            scheduler.ResumeAll();
        }

        public ExecutionResponse<bool> ScheduleJob( Guid Id , Guid groupId , DateTime time , string listenerType )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                ScheduleQuartzJob( Id , groupId , time , listenerType );

                 context.Response.Set( ResponseState.Success , true , new List<ModelMetaPair>());

                #endregion
            } );

            return context.Response;
        }
        public ExecutionResponse<bool> UnScheduleJob( List<Guid> ids , Guid groupId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                UnScheduleQuartzJob( ids , groupId );

                context.Response.Set( ResponseState.Success , true , new List<ModelMetaPair>());

                #endregion
            } );

            return context.Response;
        }
        public ExecutionResponse<bool> UnScheduleGroup( Guid groupId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                UnScheduleQuartzGroup( groupId );

                 context.Response.Set( ResponseState.Success , true , new List<ModelMetaPair>());

                #endregion
            } );

            return context.Response;
        }

        #endregion

        #region helpers

        private void ScheduleQuartzJob( Guid Id , Guid groupId , DateTime time , string listenerType )
        {
            var job = JobBuilder.Create( Type.GetType( listenerType ) )
                                .WithIdentity( Id.ToString() , groupId.ToString() )
                                .Build();

            job.JobDataMap.Add( "FireTime" , time );

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity( Id.ToString() , groupId.ToString() )
                                        .StartAt( time )
                                        .Build();

            scheduler.ScheduleJob( job , trigger );
        }
        private void UnScheduleQuartzGroup( Guid groupId )
        {
            var jobsGroupMatcher = GroupMatcher<JobKey>.GroupContains( groupId.ToString() );
            var jobKeys = scheduler.GetJobKeys( jobsGroupMatcher );

            scheduler.DeleteJobs( new List<JobKey>( jobKeys ) );
        }
        private void UnScheduleQuartzJob( List<Guid> ids , Guid groupId )
        {
            foreach ( var id in ids )
            {
                scheduler.UnscheduleJob( new TriggerKey( id.ToString() , groupId.ToString() ) );
                scheduler.DeleteJob( new JobKey( id.ToString() , groupId.ToString() ) );
            }
        }

        #endregion
    }
}
