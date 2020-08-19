namespace ADS.Common
{
    public static class Constants
    {
        #region Broker

        public const string SectionBroker = "Broker";

        public const string ConfigurationTypeKey = "ADS.Common.Config.Type";
        public const string ConfigurationDatastoreKey = "ADS.Common.Config.Datastore.Connection";
        public const string ConfigurationDatastoreTypeKey = "ADS.Common.Config.Datastore.Type";

        public const string CommonDatastoreKey = "ADS.Common.Datastore.Connection";
        public const string TamamDatastoreKey = "ADS.Tamam.Datastore.Connection";
        public const string IntegrationDatastoreKey = "ADS.Tamam.Integration.Datastore.Connection";

        public const string AuthorizationTypeKey = "Handlers.Authorization";
        public const string AuditTrailTypeKey = "Handlers.AuditTrail";
        public const string CommunicationTypeKey = "Handlers.Communication";
        public const string MasterCodeTypeKey = "Handlers.MasterCode";
        public const string DetailCodeTypeKey = "Handlers.DetailCode";
        public const string LicenseTypeKey = "Handlers.License";
        public const string LicenseDefinitionTypeKey = "Handlers.License.Definitions";

        public const string AuthorizationDatastoreTypeKey = "Handlers.Authorization.Data";
        public const string AuditTrailDatastoreTypeKey = "Handlers.AuditTrail.Data";
        public const string MasterCodeDatastoreTypeKey = "Handlers.MasterCode.Data";
        public const string DetailCodeDatastoreTypeKey = "Handlers.DetailCode.Data";
        public const string UsersDatastoreTypeKey = "Handlers.Users.Data";

        #endregion
        # region Authentication

        public const string AuthenticationDatastoreTypeKey = "Handlers.Authentication.Data";
        public const string AuthenticationProvidersPartialName = "Handlers.Authentication.Provider.";
        public const string AuthenticationIdentityProviderTypeKey = "Handlers.Authentication.Identity.Provider";
        public const string AuthenticationServiceTypeKey = "Handlers.Authentication.Service";
        public const string LDAPDomainKey = "Handlers.Authentication.Servers.LDAP.Domain";

        # endregion

        # region Workflow

        public const string WorkflowDatastoreTypeKey = "Handlers.Workflow.Data";

        # endregion
        #region Notifications

        public const string NotificationsSenderTypeKey = "Handlers.Notifications.Sender";
        public const string NotificationsListnerTypeKey = "Handlers.Notifications.Listner";
        public const string NotificationsHandlerTypeKey = "Handlers.Notifications.Handler";
        public const string NotificationsTypeHandlerPartialName = "Handlers.Notifications.TypeHandler";

        public const string NotificationsSenderDatastoreTypeKey = "Handlers.Notifications.Sender.Data";
        public const string NotificationsListnerDatastoreTypeKey = "Handlers.Notifications.Listner.Data";
        public const string NotificationsHandlerDatastoreTypeKey = "Handlers.Notifications.Handler.Data";
        public const string NotificationListSenderDatastoreTypeKey = "Handlers.Notifications.Subscriber.List.Data";

        public const string NotificationListnerSubscribersPartialName = "Handlers.Notification.Subscriber";
        public const string NotificationsListenerInterval = "Handlers.Notifications.Listner.Interval";

        #endregion
        #region Email Notification

        public const string EmailNotificationSection = "EmailNotification";
        public const string EmailNotificationHostName = "HostName";
        public const string EmailNotificationPort = "Port";
        public const string EmailNotificationUserName = "UserName";
        public const string EmailNotificationPassword = "Password";
        public const string EmailNotificationEnableSSL = "EnableSSL";
        public const string EmailNotificationImplicitSSL = "ImplicitSSL.Enabled";


        public const string EmailNotificationMailSubject = "MailSubject";
        public const string EmailNotificationWebApplicationURL = "WebApplicationURL";
        public const string EmailNotificationMessageTemplate = "MessageTemplate";
        public const string EmailNotificationDataHandlerName = "DataHandler";


        #endregion
        #region SMS Notification

        public const string SMSNotificationSection = "SMSNotification";
        public const string SMSNotificationWebApplicationURL = "WebApplicationURL";
        public const string SMSNotificationMessageTemplate = "MessageTemplate";
        public const string SMSNotificationSubject = "Subject";

        #endregion
        #region Protobuf

        public const string ProtobufSection = "Handlers.Serializers.Protobuf";
        public const string ProtobufAssemblies = "Handlers.Serializers.Protobuf.Assemblies";

        #endregion
        #region Cache Service

        public const string CacheSection = "Cache";
        public const string CacheEnabled = "Cache.Enabled";
        public const string CacheEndpoint = "Cache.Service.Endpoint";

        #endregion

        // Tamam Related Constants
        #region Tamam


        public const string SectionTamam_AttendanceNotifications = "Tamam.Attendance.Notifications";
        public const string AttendanceNotifications_MaxDenialIterations = "Attendance.Notifications.MaxDenialIterations";

        public static class TamamLeaveCreditConfig
        {
            public const string SectionTamam_LeaveCredits = "Tamam.Leaves.Credits";
            public const string InternalServiceTransferCreditsInterval = "Worker.CreditTransfer.Interval";
            public const string ForceBuildPreviousCredit = "ForceBuildPreviousCredit";
        }

        public static class TamamConfig
        {
            public const string Section = "Tamam";
            public const string VisibilityModeKey = "Authorization.Visibility.Mode";

            public const string VisibilityMode_Departments = "departments";
            public const string VisibilityMode_Personnel = "personnel";
        }

        public static class TamamAttendanceConfig
        {
            public const string Section = "Tamam.Attendance";

            public const string DashboardAttendanceStatsLateColorKey = "Dashboard.AttendanceStats.LateColor";
            public const string DashboardAttendanceStatsLeftEarlyColorKey = "Dashboard.AttendanceStats.LeftEarlyColor";
            public const string DashboardAttendanceStatsLeftLateColorKey = "Dashboard.AttendanceStats.LeftLateColor";
            public const string DashboardAttendanceStatsLeaveColorKey = "Dashboard.AttendanceStats.LeaveColor";
            public const string DashboardAttendanceStatsAbsentColorKey = "Dashboard.AttendanceStats.AbsentColor";
            public const string DashboardAttendanceStatsComeOnTimeColorKey = "Dashboard.AttendanceStats.ComeOnTimeColor";
            public const string DashboardAttendanceStatsMissedPunchesColorKey = "Dashboard.AttendanceStats.MissedPunchesColor";
            public const string DashboardAttendanceStatsWorkedLessColorKey = "Dashboard.AttendanceStats.WorkedLessColor";
            public const string DashboardAttendanceStatsWorkedMoreColorKey = "Dashboard.AttendanceStats.WorkedMoreColor";
            public const string DashboardAttendanceStatsOvertimeColorKey = "Dashboard.AttendanceStats.OvertimeColor";

            public const string DashboardAttendanceStatsShowLeftEarlyAfterShiftEndKey = "Dashboard.AttendanceStats.ShowLeftEarlyAfterShiftEnd";
            public const string DashboardAttendanceStatsShowWorkedLessAfterShiftEndKey = "Dashboard.AttendanceStats.ShowWorkedLessAfterShiftEnd";
        }

        public static class TamamDashboardConfig
        {
            public const string Section = "Tamam.Dashboard";

            public const string DashboardGridsMaxItemsCount = "Dashboard.Grids.MaxItemsCount";
        }

        public static class TamamWebClientConfig
        {
            public const string Section = "Tamam.Clients.Web";

            public const string HomePageUrlKey = "HomePageUrl";
            public const string EnableSSO = "EnableSSO";
            public const string QueryStringEncryption = "QueryString.Encryption.Enabled";
            public const string LeavesCreditChart = "Leaves.Credit.Chart";
        }

        public static class TamamWorkflowMessages
        {
            public const string Section = "Tamam.Notifications";

            //Attendance Manual Edit Workflow
            public const string ManualAttendanceApproval_Manager = "ManualAttendanceApproval.Manager";
            public const string ManualAttendanceApproval_ManagerAr = "ManualAttendanceApproval.ManagerAr";
            public const string ManualAttendanceApproval_ManagerDelegate = "ManualAttendanceApproval.ManagerDelegate";
            public const string ManualAttendanceApproval_ManagerDelegateAr = "ManualAttendanceApproval.ManagerDelegateAr";
            public const string ManualAttendanceApproval_RequestStatus = "ManualAttendanceApproval.RequestStatus";
            public const string ManualAttendanceApproval_RequestStatusAr = "ManualAttendanceApproval.RequestStatusAr";

            //Attendance Violations Workflow
            public const string AttendanceViolations_JustifyEmployee = "AttendanceViolations.JustifyEmployee";
            public const string AttendanceViolations_JustifyEmployeeAr = "AttendanceViolations.JustifyEmployeeAr";
            public const string AttendanceViolations_JustifyEmployeeApproved = "AttendanceViolations.JustifyEmployeeApproved";
            public const string AttendanceViolations_JustifyEmployeeApprovedAr = "AttendanceViolations.JustifyEmployeeApprovedAr";
            public const string AttendanceViolations_JustifyEmployeeRejected = "AttendanceViolations.JustifyEmployeeRejected";
            public const string AttendanceViolations_JustifyEmployeeRejectedAr = "AttendanceViolations.JustifyEmployeeRejectedAr";
            public const string AttendanceViolations_JustifyEmployeePartialRejected = "AttendanceViolations.JustifyEmployeePartialRejected";
            public const string AttendanceViolations_JustifyEmployeePartialRejectedAr = "AttendanceViolations.JustifyEmployeePartialRejectedAr";
            public const string AttendanceViolations_JustifyEmployeeManager = "AttendanceViolations.JustifyEmployeeManager";
            public const string AttendanceViolations_JustifyEmployeeManagerAr = "AttendanceViolations.JustifyEmployeeManagerAr";
            public const string AttendanceViolations_JustifyEmployeeManagerDelegate = "AttendanceViolations.JustifyEmployeeManagerDelegate";
            public const string AttendanceViolations_JustifyEmployeeManagerDelegateAr = "AttendanceViolations.JustifyEmployeeManagerDelegateAr";


            //Approval Workflow
            public const string RequestStatus = "RequestStatus";
            public const string RequestStatusAr = "RequestStatusAr";


            //Leave Approval Workflow
            public const string LeaveRequest = "LeaveRequest";
            public const string LeaveRequestAr = "LeaveRequestAr";
            public const string LeaveRequestForDelegate = "LeaveRequestForDelegate";
            public const string LeaveRequestArForDelegate = "LeaveRequestArForDelegate";
            public const string LeaveRequestJustifyEmployee = "LeaveRequestJustifyEmployee";
            public const string LeaveRequestJustifyEmployeeAr = "LeaveRequestJustifyEmployeeAr";

            //Excuse Approval Workflow
            public const string ExcuseRequest = "ExcuseRequest";
            public const string ExcuseRequestAr = "ExcuseRequestAr";
            public const string ExcuseRequestForDelegate = "ExcuseRequestForDelegate";
            public const string ExcuseRequestArForDelegate = "ExcuseRequestArForDelegate";
            public const string ExcuseRequestJustifyEmployee = "ExcuseRequestJustifyEmployee";
            public const string ExcuseRequestJustifyEmployeeAr = "ExcuseRequestJustifyEmployeeAr";

            //Away Approval Workflow
            public const string AwayRequest = "AwayRequest";
            public const string AwayRequestAr = "AwayRequestAr";
            public const string AwayRequestForDelegate = "AwayRequestForDelegate";
            public const string AwayRequestArForDelegate = "AwayRequestArForDelegate";
            public const string AwayRequestJustifyEmployee = "AwayRequestJustifyEmployee";
            public const string AwayRequestJustifyEmployeeAr = "AwayRequestJustifyEmployeeAr";
        }


        public static class TamamCaptureConfig
        {
            public const string Section = "Tamam.Attendance.Capture";

            public const string WorkerPullInterval = "Worker.Pull.Interval";
            public const string TypeAutoDetectionMode = "Type.Auto.Detection.Mode";          
            
        }

        public static class TamamEngineConfig
        {
            public const string Section = "Tamam.Attendance.Engine";

            public const string WorkerDirtyInterval = "Worker.DirtyCheck.Interval";
            public const string WorkerShiftStartDelay = "Worker.ShiftStart.Delay";
            public const string WorkerShiftEndDelay = "Worker.ShiftEnd.Delay";
            public const string DataCalculationPreviousDays = "Data.Calculation.Previous.Days";
            public const string WorkerCalculationsInterval = "Worker.Calculations.Interval";
            public const string StatsCalculationInterval = "Worker.Stats.Calculations.Interval";
            public const string DuplicatePunchThreshold = "Duplicate.Punch.Threshold";

        }
        #endregion
    }
}
