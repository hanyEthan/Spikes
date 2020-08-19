using System;
using ADS.Common;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Context;

namespace ADS.Tamam.Common.Handlers
{
    public static class TamamServiceBroker
    {
        #region Properties ...

        public static bool Initialized { get; private set; }

        #endregion
        #region Services

        public static IPersonnelHandler PersonnelHandler { get; private set; }
        public static IAttendanceHandler AttendanceHandler { get; private set; }
        public static ISchedulesHandler SchedulesHandler { get; private set; }
        public static ILeavesHandler LeavesHandler { get; private set; }
        public static IOrganizationHandler OrganizationHandler { get; private set; }
        public static IReportingHandler ReportingHandler { get; private set; }

        #endregion
        #region Nested

        public static class Status
        {
            public static bool Initialized { get { return TamamServiceBroker.Initialized; } }
            public static AuthorizationVisibilityMode Visibility { get; internal set; }
        }

        #endregion

        #region cst.

        /// <summary>
        /// initiating underlying components ...
        /// </summary>
        static TamamServiceBroker()
        {
            XLogger.Info("TamamBroker ...");

            try
            {
                if ( !Broker.Initialized ) return;
                if ( !TamamDataBroker.Initialized ) return;
                if ( !SystemBroker.Initialized ) return;

                Broker.LicenseHandler.LicenseExpired += LicenseExpired;

                #region Personnel handler

                PersonnelHandler = Broker.InitializeHandler<IPersonnelHandler>(TamamConstants.PersonnelHandlerName, Constants.SectionBroker, Broker.ConfigurationHandler);
                if (PersonnelHandler == null || !PersonnelHandler.Initialized)
                {
                    XLogger.Error("TamamBroker ... FAILED to initialize the personnel module.");
                    return;
                }

                #endregion
                #region Attendance handler

                AttendanceHandler = Broker.InitializeHandler<IAttendanceHandler>(TamamConstants.AttendanceHandlerName, Constants.SectionBroker, Broker.ConfigurationHandler);
                if (AttendanceHandler == null || !AttendanceHandler.Initialized)
                {
                    XLogger.Error("TamamBroker ... FAILED to initialize the attendance module.");
                    return;
                }

                #endregion
                #region Schedules handler

                SchedulesHandler = Broker.InitializeHandler<ISchedulesHandler>(TamamConstants.SchedulesHandlerName, Constants.SectionBroker, Broker.ConfigurationHandler);
                if (SchedulesHandler == null || !SchedulesHandler.Initialized)
                {
                    XLogger.Error("TamamBroker ... FAILED to initialize the Schedules module.");
                    return;
                }

                #endregion
                #region Leaves handler

                LeavesHandler = Broker.InitializeHandler<ILeavesHandler>(TamamConstants.LeavesHandlerName, Constants.SectionBroker, Broker.ConfigurationHandler);
                if (LeavesHandler == null || !LeavesHandler.Initialized)
                {
                    XLogger.Error("TamamBroker ... FAILED to initialize the leaves module.");
                    return;
                }

                #endregion
                #region Organization handler

                OrganizationHandler = Broker.InitializeHandler<IOrganizationHandler>(TamamConstants.OrganizationHandlerName, Constants.SectionBroker, Broker.ConfigurationHandler);
                if (OrganizationHandler == null || !OrganizationHandler.Initialized)
                {
                    XLogger.Error("TamamBroker ... FAILED to initialize the Organization module.");
                    return;
                }

                #endregion
                #region Reporting handler

                ReportingHandler = Broker.InitializeHandler<IReportingHandler>( TamamConstants.ReportingHandlerName , Constants.SectionBroker , Broker.ConfigurationHandler );
                if ( ReportingHandler == null || !ReportingHandler.Initialized )
                {
                    XLogger.Error( "TamamBroker ... FAILED to initialize the ReportingHandler module." );
                    return;
                }

                #endregion

                #region Visibility ...

                string visibility = Broker.ConfigurationHandler.GetValue(Constants.TamamConfig.Section, Constants.TamamConfig.VisibilityModeKey);
                switch (visibility.ToLower())
                {
                    case Constants.TamamConfig.VisibilityMode_Personnel:
                        Status.Visibility = AuthorizationVisibilityMode.Personnel;
                        break;
                    case Constants.TamamConfig.VisibilityMode_Departments:
                    default:
                        Status.Visibility = AuthorizationVisibilityMode.Departments;
                        break;
                }

                XLogger.Info("TamamBroker : Authorization visibility is set to : " + Status.Visibility.ToString());

                #endregion

                XLogger.Info("TamamBroker started successfully.");
                Initialized = true;
            }
            catch (Exception x)
            {
                XLogger.Error("TamamBroker FAILED to start.");
                XLogger.Error("TamamBroker ... Exception: " + x);

                //ExceptionHandler.Handle( x );
            }
        }

        #endregion

        #region Helpers

        public static bool Authenticate(RequestContext context)
        {
            //return Broker.AuthenticationHandler.Authenticate( context.SecurityToken );
            return true;
        }
        public static bool Authorize(RequestContext context, string target)
        {
            try
            {
                #region Validate

                if ( context is SystemRequestContext ) return true;
                if ( !context.PersonId.HasValue ) throw new ArgumentNullException( "RequestContext associated PersonId is null." );
                
                #endregion
                #region Cache

                var cacheKey = "TamamServiceBroker_Authorize" + context + target;
                var cached = Broker.Cache.Get<bool?>( TamamCacheClusters.Security , cacheKey );
                if ( cached != null ) return cached.Value;

                #endregion

                bool isAuthorized = Broker.AuthorizationHandler.Authorize(context.PersonId.Value, target);

                #region Cache

                Broker.Cache.Add<bool?>( TamamCacheClusters.Security , cacheKey , isAuthorized );

                #endregion

                return isAuthorized;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }

        public static ExecutionResponse<SecurityContext> Secure( ActionContext action , RequestContext request )
        {
            // ...
            var response = new ExecutionResponse<SecurityContext>();

            // ...
            if ( !Status.Initialized )
            {
                response.Set( ResponseState.SystemError , null );
                return response;
            }

            // ...
            if ( !Authenticate( request ) )
            {
                response.Set( ResponseState.AccessDenied , null );
                return response;
            }

            // ...
            if ( !Authorize( request , action.ActionKey ) )
            {
                Audit( request , action , action.MessageForDenied , "" );
                response.Set( ResponseState.AccessDenied , null );
                return response;
            }

            // ...
            var security = request.SecurityContext;

            // Check View All Departments Privilege
            var viewAllDepartments = Authorize( request , TamamConstants.AuthorizationConstants.ViewAllOrganizationsActionKey );
            if ( viewAllDepartments ) security.VisibilityMode = AuthorizationVisibilityMode.None;

            if ( !( security is SystemSecurityContext ) && viewAllDepartments == false )
            {
                security.VisibilityMode = TamamServiceBroker.Status.Visibility;

                switch ( security.VisibilityMode )
                {
                    case AuthorizationVisibilityMode.Personnel:
                        {
                            var response_P = PersonnelHandler.GetPersonnelByRoot( request.PersonId.Value , request );
                            if ( response_P.Type != ResponseState.Success )
                            {
                                response.Set( response_P.Type , null );
                                return response;
                            }
                            var TempPersonnelRange = (string)response_P.Result;
                            var stringLength = 1997;
                            security.PersonnelRange = XModel.SplitStringByLength(TempPersonnelRange, stringLength);
                            
                            //security.PersonnelRange = ( string ) response_P.Result;

                            break;
                        }
                    case AuthorizationVisibilityMode.Departments:
                    default:
                        {
                            var response_P = OrganizationHandler.GetDepartmentsByPerson( request.PersonId.Value , request );
                            if ( response_P.Type != ResponseState.Success )
                            {
                                response.Set( response_P.Type , null );
                                return response;
                            }

                            security.DepartmentsRange = ( string ) response_P.Result;

                            break;
                        }
                }
            }

            // ...
            response.Set( ResponseState.Success , security );
            return response;
        }

        public static void Audit(RequestContext context, ActionContext action, string message, string reference)
        {
            Audit(context, action.ActionId, action.ModuleId, message, reference);
        }
        public static void Audit(RequestContext context, string actionId, string moduleId, string message, string reference)
        {
            Broker.AuditTrailHandler.Log(new AuditTrailLog(context.SecurityToken.ToString(), context.CallerUsername, int.Parse(moduleId), int.Parse(actionId), context.IpAddress, context.MachineName, reference, message));
        }

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
