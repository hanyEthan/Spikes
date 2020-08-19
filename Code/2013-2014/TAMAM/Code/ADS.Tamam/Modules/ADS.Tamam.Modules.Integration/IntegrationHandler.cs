using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Modules.Integration.Helpers;
using ADS.Tamam.Modules.Integration.Integrators;
using ADS.Tamam.Modules.Integration.Repositories;
using System.Collections.Generic;

namespace ADS.Tamam.Modules.Integration
{
    public class IntegrationHandler : IIntegrationHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "IntegrationHandler"; } }

        #endregion
        #region cst.

        public IntegrationHandler()
        {
            XLogger.Trace("");
            XLogger.Settings.Sensitivity = XLogger.Enums.LogStatus.Info;

            Initialized = TamamServiceBroker.Initialized;
        }

        #endregion

        #region IIntegrationHandler

        public ExecutionResponse<bool> Synchronize(RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region Logic ...

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating job titles ..." ) );
                #endregion
                var detailCodeRepository = new DetailCodeRepository(TamamConstants.MasterCodes.JobTitle);
                new DetailCodeIntegrator(new DataHandlers.JobTitlesDataHandler(), detailCodeRepository).Integrate();
                
                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating MaritalStatuses ..." ) );
                #endregion
                detailCodeRepository = new DetailCodeRepository(TamamConstants.MasterCodes.MaritalStatus);
                new DetailCodeIntegrator(new DataHandlers.MaritalStatusesDataHandler(), detailCodeRepository).Integrate();
                
                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating Nationalities ..." ) );
                #endregion
                detailCodeRepository = new DetailCodeRepository(TamamConstants.MasterCodes.Nationality);
                new DetailCodeIntegrator(new DataHandlers.NationalitiesDataHandler(), detailCodeRepository).Integrate();
                
                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating LeaveTypes ..." ) );
                #endregion                
                detailCodeRepository = new DetailCodeRepository(TamamConstants.MasterCodes.LeaveType);
                new LeaveTypesIntegrator( new DataHandlers.LeaveTypesDataHandler(), detailCodeRepository ).Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating Genders ..." ) );
                #endregion                
                detailCodeRepository = new DetailCodeRepository(TamamConstants.MasterCodes.Gender);
                new DetailCodeIntegrator(new DataHandlers.GendersDataHandler(), detailCodeRepository).Integrate();
                
                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating Relegions ..." ) );
                #endregion                
                detailCodeRepository = new DetailCodeRepository(TamamConstants.MasterCodes.Religion);
                new DetailCodeIntegrator(new DataHandlers.ReligionsDataHandler(), detailCodeRepository).Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating holidays ..." ) );
                #endregion                
                var holidaysIntegrator = new HolidaysIntegrator();
                holidaysIntegrator.Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating departments ..." ) );
                #endregion                
                var departmentsIntegrator = new DepartmentsIntegrator();
                departmentsIntegrator.Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating personnel ..." ) );
                #endregion                
                var personnelIntegrator = new PersonnelIntegrator();
                personnelIntegrator.Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating delegates ..." ) );
                #endregion                
                var delegatesIntegrator = new DelegatesIntegrator();
                delegatesIntegrator.Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating leaves ..." ) );
                #endregion                
                var leavesIntegrator = new LeavesIntegrator();
                leavesIntegrator.Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating excuses ..." ) );
                #endregion                
                var excusesIntegrator = new ExcusesIntegrator();
                excusesIntegrator.Integrate();

                #region LOG
                XLogger.Info( LogHelper.BuildMessage( "Start migrating Leave Policies ..." ) );
                #endregion
                var leavePoliciesIntegrator = new LeavePoliciesIntegrator();
                leavePoliciesIntegrator.Integrate();

                #endregion

            }, requestContext);

            return context.Response;
        }

        #endregion
    }
}
