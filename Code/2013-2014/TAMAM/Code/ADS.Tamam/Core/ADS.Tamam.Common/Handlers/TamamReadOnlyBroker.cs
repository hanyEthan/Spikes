using System;

using ADS.Common;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Handlers.Automation;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Context;

namespace ADS.Tamam.Common.Handlers
{
    public static class TamamReadOnlyBroker
    {
        #region Properties ...

        public static bool Initialized { get; private set; }

        #endregion
        #region Services

        public static IReadOnlyPersonnelHandler PersonnelHandler { get; private set; }
        public static IReadOnlyAttendanceHandler AttendanceHandler { get; private set; }
        public static IReadOnlySchedulesHandler SchedulesHandler { get; private set; }
        public static IReadOnlyLeavesHandler LeavesHandler { get; private set; }
        public static IReadOnlyOrganizationHandler OrganizationHandler { get; private set; }
        public static IReadOnlyReportingHandler ReportingHandler { get; private set; }

        #endregion

        #region cst.

        /// <summary>
        /// initiating underlying components ...
        /// </summary>
        static TamamReadOnlyBroker()
        {
            XLogger.Info( "TamamReadOnlyBroker ..." );

            try
            {
                if (!Broker.Initialized) return;
                if (!TamamDataBroker.Initialized) return;
                Broker.LicenseHandler.LicenseExpired += LicenseExpired;

                #region Personnel handler

                PersonnelHandler = Broker.InitializeHandler<IReadOnlyPersonnelHandler>( TamamConstants.PersonnelHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if ( PersonnelHandler == null || !PersonnelHandler.Initialized )
                {
                    XLogger.Error( "TamamReadOnlyBroker ... FAILED to initialize the personnel module." );
                    return;
                }

                #endregion
                #region Attendance handler

                AttendanceHandler = Broker.InitializeHandler<IReadOnlyAttendanceHandler>( TamamConstants.AttendanceHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if ( AttendanceHandler == null || !AttendanceHandler.Initialized )
                {
                    XLogger.Error( "TamamReadOnlyBroker ... FAILED to initialize the attendance module." );
                    return;
                }

                #endregion
                #region Schedules handler

                SchedulesHandler = Broker.InitializeHandler<IReadOnlySchedulesHandler>( TamamConstants.SchedulesHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if ( SchedulesHandler == null || !SchedulesHandler.Initialized )
                {
                    XLogger.Error( "TamamReadOnlyBroker ... FAILED to initialize the Schedules module." );
                    return;
                }

                #endregion
                #region Leaves handler

                LeavesHandler = Broker.InitializeHandler<IReadOnlyLeavesHandler>( TamamConstants.LeavesHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if (LeavesHandler == null || !LeavesHandler.Initialized)
                {
                    XLogger.Error( "TamamReadOnlyBroker ... FAILED to initialize the leaves module." );
                    return;
                }

                #endregion
                #region Organization handler

                OrganizationHandler = Broker.InitializeHandler<IReadOnlyOrganizationHandler>( TamamConstants.OrganizationHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if (OrganizationHandler == null || !OrganizationHandler.Initialized)
                {
                    XLogger.Error( "TamamReadOnlyBroker ... FAILED to initialize the Organization module." );
                    return;
                }

                #endregion
                #region Reporting handler

                ReportingHandler = Broker.InitializeHandler<IReadOnlyReportingHandler>( TamamConstants.ReportingHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if ( ReportingHandler == null || !ReportingHandler.Initialized )
                {
                    XLogger.Error( "TamamReadOnlyBroker ... FAILED to initialize the Reporting module." );
                    return;
                }

                #endregion

                XLogger.Info( "TamamReadOnlyBroker started successfully." );
                Initialized = true;
            }
            catch (Exception x)
            {
                XLogger.Error( "TamamReadOnlyBroker FAILED to start." );
                XLogger.Error( "TamamReadOnlyBroker ... Exception: " + x );
            }
        }

        #endregion
        #region helpers.

        private static void LicenseExpired(object sender, EventArgs e)
        {
            Initialized = false;

            //PersonnelHandler = null;
            //AttendanceHandler = null;
            //SchedulesHandler = null;
            //LeavesHandler = null;
            //OrganizationHandler = null;
            //ReportingHandler = null;
        }

        #endregion
    }
}
