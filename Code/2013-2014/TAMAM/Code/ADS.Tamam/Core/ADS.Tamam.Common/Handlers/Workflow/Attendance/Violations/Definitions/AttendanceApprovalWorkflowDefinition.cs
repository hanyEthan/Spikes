using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ADS.Common;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Models.Enums;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Actions;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Evaluators;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Models;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;
using ADS.Tamam.Common.Handlers.Workflow.Notifications.Models;
using ADS.Tamam.Common.Workflow.Notifications.Actions;
using ADS.Tamam.Common.Workflow.Notifications.Data;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Definitions
{
    [DataContract(IsReference = true)]
    public class AttendanceApprovalWorkflowDefinition : IWorkflowDefinition
    {
        # region IWorkflowDefinition

        public Guid Id { get { return new Guid("43EBA421-B57F-4BC0-B72D-B31A48222E47"); } }
        public List<string> WorkflowSupportingTypes
        {
            get
            {
                return new List<string>()
                {
                    typeof( AttendanceApprovalWorkflowDefinition ).AssemblyQualifiedName ,
                    typeof( AttendanceJustificationWorkflowData ).AssemblyQualifiedName ,
                    typeof( AttendanceJustificationWorkflowAction ).AssemblyQualifiedName ,
                    typeof( AttendanceCancellationWorkflowAction ).AssemblyQualifiedName , 
                    typeof(AttendanceInitializationWorkflowAction).AssemblyQualifiedName,
                    typeof(AttendanceFinalizationWorkflowData).AssemblyQualifiedName,
                    typeof(AttendanceFinalizationWorkflowAction).AssemblyQualifiedName,

                    typeof( AttendanceReviewWorkflowEvaluator ).AssemblyQualifiedName ,
                    typeof( AttendanceReviewStatus ).AssemblyQualifiedName ,

                    typeof( NotificationWorkflowAction ).AssemblyQualifiedName ,
                    typeof( NotificationWorkflowData ).AssemblyQualifiedName ,
                    typeof( NotificationResetWorkflowAction ).AssemblyQualifiedName , 

                    typeof( NotificationsSettingsWorkflowData ).AssemblyQualifiedName , 

                    typeof( AttendanceReviewIterationsWorkflowEvaluator ).AssemblyQualifiedName , 
                    
                };
            }
        }
        public WorkflowInstance Process(IWorkflowTarget target)
        {
            try
            {
                var policy = Policies.GetAttendancePolicy( target );
                var instance = BuildInstance( target , policy );

                return instance;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }

        # endregion

        # region Helpers

        private WorkflowInstance BuildInstance( IWorkflowTarget target , AttendancePolicy attendancePolicy )
        {
            #region prepping data ...

            if ( target == null ) return null;
            if ( attendancePolicy == null || attendancePolicy.ViolationsApprovalPolicy == null ) return null;

            var approvalPolicy = attendancePolicy.ViolationsApprovalPolicy;
            var maxIterations = WorkflowHelpers.GetMaxDenialIterations( approvalPolicy );
            var includePassedStepsPerIteration = WorkflowHelpers.GetIncludePassedStepsPerIteration( approvalPolicy );

            var metadata = "";

            #endregion
            #region nodes ( main )

            var instance = new WorkflowInstance( approvalPolicy.Policy.Id.ToString() );
            instance.Data = new NotificationsSettingsWorkflowData( maxIterations , includePassedStepsPerIteration );

            var initial = instance.Initial<WorkflowStep>().Run<AttendanceInitializationWorkflowAction>(null);

            var cancelled = instance.Cancellation<WorkflowStep>().Run<AttendanceCancellationWorkflowAction>(null);
            cancelled.Next<WorkflowStep>().Run<NotificationResetWorkflowAction>(null);

            var reset = instance.Resetting<WorkflowStep>().Run<AttendanceCancellationWorkflowAction>(null);
            reset.Next<WorkflowStep>().Run<NotificationResetWorkflowAction>(null);

            var currunt = initial;

            #endregion
            #region nodes ( logic )

            #region prepping data ...

            var employeeId = target.PersonId.ToString();
            var employee = GetPerson( target.PersonId );
            var manager = GetManagerId( target.PersonId );
            var managerEmail = manager.HasValue ? GetManagerEmailAddress( manager.Value ) : string.Empty;
            var directManagerId = manager.HasValue ? manager.ToString() : "";

            var personHRPolicy = GetHRPolicy( employee );
            var employeeCCs = string.IsNullOrEmpty( managerEmail ) ? personHRPolicy.CCs : string.Join( "," , personHRPolicy.CCs , managerEmail );
            var CCs = personHRPolicy.CCs;
            
            #endregion

            if ( !string.IsNullOrWhiteSpace( directManagerId ) )
            {
                #region prepping data ...

                var SE = GetScheduleEvent( target.Id );

                var notificationTargetType = GetNotificationTargetType( SE );

                string date = SE != null ? SE.EventDate.ToString( "dd/MM/yyyy" ) : string.Empty;
                string state_EN = SE != null ? SE.GetLocalizedDetailedStatus_Full( "en" ) : string.Empty;
                string state_AR = SE != null ? SE.GetLocalizedDetailedStatus_Full( "ar" ) : string.Empty;

                string messageForEmployeeCode = Guid.NewGuid().ToString();
                string messageForEmployee_EN = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployee ) , state_EN , date );
                string messageForEmployee_AR = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeAr ) , state_AR , date );
                var messageForEmployeeData = new NotificationWorkflowData( messageForEmployeeCode , employeeId , target.Id.ToString() , messageForEmployee_EN , messageForEmployee_AR , "" , "" , NotificationType.Comment , GetActionUrl( messageForEmployeeCode ) , WorkflowNotificationTargetType.AttendanceViolations.ToString().ToLower() , notificationTargetType , false , employeeCCs );

                string messageForEmployeeFinishedApprovedCode = Guid.NewGuid().ToString();
                string messageForEmployeeFinishedApproved_EN = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeApproved ) , state_EN , date );
                string messageForEmployeeFinishedApproved_AR = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeApprovedAr ) , state_AR , date );
                var messageForEmployeeFinishedApprovedData = new NotificationWorkflowData( messageForEmployeeFinishedApprovedCode , employeeId , target.Id.ToString() , messageForEmployeeFinishedApproved_EN , messageForEmployeeFinishedApproved_AR , "" , "" , NotificationType.Information , GetActionUrl( messageForEmployeeFinishedApprovedCode ) , WorkflowNotificationTargetType.AttendanceViolations.ToString().ToLower() , notificationTargetType , false , employeeCCs );

                string messageForEmployeeFinishedRejectedCode = Guid.NewGuid().ToString();
                string messageForEmployeeFinishedRejected_EN = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeRejected ) , state_EN , date );
                string messageForEmployeeFinishedRejected_AR = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeRejectedAr ) , state_AR , date );
                var messageForEmployeeFinishedRejectedData = new NotificationWorkflowData( messageForEmployeeFinishedRejectedCode , employeeId , target.Id.ToString() , messageForEmployeeFinishedRejected_EN , messageForEmployeeFinishedRejected_AR , "" , "" , NotificationType.Information , GetActionUrl( messageForEmployeeFinishedRejectedCode ) , WorkflowNotificationTargetType.AttendanceViolations.ToString().ToLower() , notificationTargetType , false , employeeCCs );

                string messageForEmployeePartialRejectedCode = Guid.NewGuid().ToString();
                string messageForEmployeePartialRejected_EN = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeePartialRejected ) , state_EN , date );
                string messageForEmployeePartialRejected_AR = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeePartialRejectedAr ) , state_AR , date );
                var messageForEmployeePartialRejectedData = new NotificationWorkflowData( messageForEmployeePartialRejectedCode , employeeId , target.Id.ToString() , messageForEmployeePartialRejected_EN , messageForEmployeePartialRejected_AR , "" , "" , NotificationType.Comment , GetActionUrl( messageForEmployeePartialRejectedCode ) , WorkflowNotificationTargetType.AttendanceViolations.ToString().ToLower() , notificationTargetType , false , employeeCCs );

                string messageForManagerCode = Guid.NewGuid().ToString();
                var managerMessage = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeManager ) , employee.FullName , state_EN , date );
                var managerMessageArabic = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeManagerAr ) , state_AR , date , employee.NameArabic );
                var managerMessageForDelegate = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeManagerDelegate ) , employee.FullName , state_EN , date );
                var managerMessageArabicForDelegate = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AttendanceViolations_JustifyEmployeeManagerDelegateAr ) , state_AR , date , employee.NameArabic );

                var justificationData = new AttendanceJustificationWorkflowData() { ScheduleEventId = target.Id.ToString() , PersonId = employeeId };

                #endregion

                if ( employee.EnableAttendanceViolations == false )
                {
                    // finish up the workflow ...
                    currunt.Next<WorkflowStep>().Run<AttendanceFinalizationWorkflowAction>( new AttendanceFinalizationWorkflowData( true ) );
                    return instance;
                }
                else
                {
                    #region prepping steps from policy

                    // get approval steps filtered by condition and sorted by sequence..
                    var approvalSteps = approvalPolicy.ApprovalSteps.OrderBy( s => s.Sequance ).ToList();

                    // get valid list of approvals Ids..
                    var Approvals = new List<Guid>();
                    foreach ( var step in approvalSteps )
                    {
                        var approverId = WorkflowHelpers.GetPersonIdByStepType( employee , personHRPolicy , step );
                        if ( IsValidApprover( approverId , target.PersonId ) ) Approvals.Add( approverId );
                    }

                    #endregion

                    var notifyEmployee = currunt.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForEmployeeData );   // notify
                    var justification = notifyEmployee.Next<WorkflowStepCommand>().Run<AttendanceJustificationWorkflowAction>( justificationData ).Checkpoint( justificationData );
                    var initialLoopBackPoint = justification.Next<WorkflowStep>();
                    currunt = initialLoopBackPoint;

                    foreach ( var approverId in Approvals )
                    {
                        #region prepping the declination iteration logic ...

                        var peronJustificationStep = instance.Step<WorkflowStep>().Run<NotificationResetWorkflowAction>( null );

                        var iterationBackPoint = peronJustificationStep.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForEmployeePartialRejectedData )
                                                                       .Next<WorkflowStepCommand>().Run<AttendanceJustificationWorkflowAction>( justificationData )
                                                                       .CheckpointPartial( justificationData )
                                                                       .Next<WorkflowStep>().Run<NotificationResetWorkflowAction>( null );

                        var finalDenial = instance.Step<WorkflowStep>().Run<NotificationResetWorkflowAction>( null )
                                                  .Next<WorkflowStep>().Run<AttendanceFinalizationWorkflowAction>( new AttendanceFinalizationWorkflowData( false ) )
                                                  .Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForEmployeeFinishedRejectedData );
                        
                        var iterationQuestion = instance.Step<WorkflowStep , AttendanceReviewIterationsWorkflowEvaluator>( instance.Data );
                        iterationQuestion.When( "max" ).Next( finalDenial );
                        iterationQuestion.When( "" ).Next( peronJustificationStep );

                        #endregion
                        #region prepping step data ...

                        // data ...
                        var messageForReviewer    = new NotificationWorkflowData( messageForManagerCode , approverId.ToString() , target.Id.ToString() , managerMessage , managerMessageArabic , managerMessageForDelegate , managerMessageArabicForDelegate , NotificationType.Action , GetActionUrl( messageForManagerCode ) , WorkflowNotificationTargetType.AttendanceViolations.ToString().ToLower() , notificationTargetType , true , CCs );
                        var approvalData = new AttendanceJustificationWorkflowData() { ScheduleEventId = target.Id.ToString() , PersonId = approverId.ToString() };

                        #endregion
                        #region steps ...

                        // notify
                        var notify = currunt.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForReviewer );

                        // question ...
                        var approval = notify.Next<WorkflowStepCommand , AttendanceReviewWorkflowEvaluator>().Run<AttendanceJustificationWorkflowAction>( approvalData )
                                             .Checkpoint( approvalData );
                        // if approved, then ...
                        currunt = approval.When( AttendanceReviewStatus.Approved.ToString().ToLower() )
                                          .Next<WorkflowStep>().Run<NotificationResetWorkflowAction>( null )
                                          .Next<WorkflowStep>();

                        // if denied, then ...
                        var managerDenied = approval.When( AttendanceReviewStatus.Denied.ToString().ToLower() )
                                                    .Next( iterationQuestion );

                        #endregion
                        #region continue : prepping the declination iteration logic ...

                        iterationBackPoint.Next( includePassedStepsPerIteration ? initialLoopBackPoint : notify );

                        #endregion

                        metadata += approverId + ",";
                    }

                    currunt.Next<WorkflowStep>().Run<AttendanceFinalizationWorkflowAction>( new AttendanceFinalizationWorkflowData( true ) )
                           .Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForEmployeeFinishedApprovedData );

                    instance.Metadata = metadata;
                    return instance;
                }
            }

            #endregion

            return instance;
        }

        private HRPolicy GetHRPolicy(Person person)
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies(person,
                new PolicyFilters(new Guid(PolicyTypes.HRPolicyType), true), SystemRequestContext.Instance);
            if (response.Type != ResponseState.Success || response.Result == null || response.Result.Count == 0) return null;
            return new HRPolicy(response.Result.FirstOrDefault());

        }
        private Guid? GetManagerId(Guid personId)
        {
            var person = GetPerson(personId);
            if (person == null) return null;

            if (person.AccountInfo.ReportingTo == null) return null;
            return person.AccountInfo.ReportingTo.AccountInfo.Activated ? person.AccountInfo.ReportingToId : null;
        }
        private string GetManagerEmailAddress(Guid personId)
        {
            var person = GetPerson(personId);
            if (person == null) return null;

            return person.ContactInfo.Email;
        }
        private Person GetPerson(Guid personId)
        {
            var response = TamamServiceBroker.PersonnelHandler.GetPerson(personId, SystemRequestContext.Instance);
            if (response.Type != ResponseState.Success) return null;

            return response.Result;
        }
        private string GetActionUrl(string code)
        {
            return Broker.CommunicationHandler.EncryptQueryString(string.Format("Notifications?Code={0}", code));
        }
        private bool IsValidApprover( Guid approverId , Guid targetOwner )
        {
            if ( approverId == Guid.Empty ) return false;
            return approverId != targetOwner;
        }

        // to support Notifications policy..
        private NotificationTargetType GetNotificationTargetType(ScheduleEvent SE)
        {
            var status = SE.GetDetailedStatusModel();

            // In Late
            if (status.InStatus.HasValue && status.InStatus.Value == AttendanceCodes.AttendanceEventStatus.CameLate) return NotificationTargetType.InLate;
            if (status.InStatus.HasValue && status.InStatus.Value == AttendanceCodes.AttendanceEventStatus.LateAbsent) return NotificationTargetType.LateAbsent;
            if (status.TotalStatus.HasValue && status.TotalStatus.Value == AttendanceCodes.ScheduleEventStatus.Absent) return NotificationTargetType.Absent;
            if (status.OutStatus.HasValue && status.OutStatus.Value == AttendanceCodes.AttendanceEventStatus.LeftEarly) return NotificationTargetType.EarlyLeave;

            //if (status.OutStatus.HasValue &&
            //    status.OutStatus.Value == AttendanceCodes.AttendanceEventStatus.LeftOnGrace &&
            //    status.InStatus.HasValue &&
            //    (status.InStatus.Value == AttendanceCodes.AttendanceEventStatus.CameLate ||
            //    status.InStatus.Value == AttendanceCodes.AttendanceEventStatus.LateAbsent)) return NotificationTargetType.LeftOnGrace;

            if ((status.TotalStatus.HasValue && status.TotalStatus.Value == AttendanceCodes.ScheduleEventStatus.MissedInPunch) || (status.TotalStatus.HasValue && status.TotalStatus.Value == AttendanceCodes.ScheduleEventStatus.MissedOutPunch)) return NotificationTargetType.MissedPunch;
            if (SE.CalculatedHours != (SE.Shift.Duration * 60) && SE.CalculatedHours != 0) return NotificationTargetType.WorkingLess;

            return NotificationTargetType.NotSet;
        }

        private ScheduleEvent GetScheduleEvent( Guid id )
        {
            var response = SystemBroker.AttendanceHandler.GetScheduleEvent( id );
            return response.Result;
        }

        # endregion

        # region nested

        static class WorkflowHelpers
        {
            public static Person GetWorkflowTargetPerson( IWorkflowTarget target )
            {
                var dataHandlerPersonnel = new PersonnelDataHandler();
                var person = dataHandlerPersonnel.GetPerson( target.PersonId , SystemSecurityContext.Instance ).Result;
                return person;
            }
            public static int GetMaxDenialIterations( ApprovalPolicy policy )
            {
                if ( policy == null ) return 0;
                if ( policy.EnableIterations.HasValue && policy.EnableIterations.Value == false ) return 0;
                if ( policy.EnableIterations.HasValue && policy.EnableIterations.Value )
                {
                    return policy.MaxIterationsCount.HasValue ? policy.MaxIterationsCount.Value : 0;
                }
                return 0;
            }
            public static bool GetIncludePassedStepsPerIteration( ApprovalPolicy policy )
            {
                if ( policy == null ) return true;      // TEMP: set as true (need to used from policy)
                return policy.IncludePassedStepsPerIteration.HasValue && policy.IncludePassedStepsPerIteration.Value;
            }
            public static Guid GetPersonIdByStepType( Person person , HRPolicy personHRPolicy , ApprovalStep item )
            {
                try
                {
                    switch ( item.StepType )
                    {
                        case StepType.Manager: return GetPersonDirectManagerId( person );
                        case StepType.SuperManager: return GetPersonSuperManagerId( person );
                        case StepType.HR: return GetPersonHRPersonId( personHRPolicy );
                        default: return Guid.Empty;
                    }
                }
                catch
                {
                    return Guid.Empty;
                }
            }

            private static Guid GetPersonDirectManagerId( Person person )
            {
                var manager = person.AccountInfo.ReportingTo;
                if ( manager == null ) return Guid.Empty;
                if ( manager.AccountInfo.Activated == false ) return Guid.Empty;
                return manager.Id;
            }
            private static Guid GetPersonSuperManagerId( Person person )
            {
                var directManagerId = GetPersonDirectManagerId( person );
                if ( directManagerId == Guid.Empty ) return Guid.Empty;

                var superManager = person.AccountInfo.ReportingTo.AccountInfo.ReportingTo;
                if ( superManager == null ) return Guid.Empty;
                if ( superManager.AccountInfo.Activated == false ) return Guid.Empty;
                return superManager.Id;
            }
            private static Guid GetPersonHRPersonId( HRPolicy policy )
            {
                if ( policy == null ) return Guid.Empty;
                var HRPerson = Policies.GetHRPerson( policy );
                return HRPerson != null ? HRPerson.Id : Guid.Empty;
            }
        }
        static class Policies
        {
            public static List<Policy> PersonPolicies { get; private set; }
            public static AttendancePolicy GetAttendancePolicy( IWorkflowTarget target )
            {
                if ( target == null ) return null;

                var policies = GetPolicies( target.PersonId );
                if ( policies == null ) return null;

                var nativePolicy = policies.FirstOrDefault( p => p.PolicyTypeId == Guid.Parse( PolicyTypes.AttendancePolicyType ) );
                if ( nativePolicy == null ) return null;

                var policy = new AttendancePolicy( nativePolicy );

                return policy;
            }
            public static HRPolicy GetHRPolicy()
            {
                var policy = PersonPolicies.FirstOrDefault( x => x.PolicyTypeId == new Guid( PolicyTypes.HRPolicyType ) );
                return new HRPolicy( policy );
            }
            public static Person GetHRPerson( HRPolicy HRPolicy )
            {
                var dataHandlerPersonnel = new PersonnelDataHandler();
                var dataHandlerOrganization = new OrganizationDataHandler();

                var HRDepartment = HRPolicy.HRDepartment;
                if ( HRDepartment == null ) return null;

                var HRDepartments = dataHandlerOrganization.GetDepartmentsByRoot( HRDepartment.Id ).Result;

                var HRPerson = dataHandlerPersonnel.GetPersonnel( new PersonSearchCriteria
                {
                    ActivationStatus = true ,
                    Departments = HRDepartments.Select( d => d.Id ).ToList() ,
                    Titles = new List<int> { HRPolicy.HRRole.Id } ,
                } , SystemSecurityContext.Instance ).Result.Persons.FirstOrDefault();

                return HRPerson;
            }

            private static List<Policy> GetPolicies( Guid personId )
            {
                var response = TamamServiceBroker.OrganizationHandler.GetPolicies( personId , SystemRequestContext.Instance );
                if ( response.Type != ResponseState.Success || response.Result == null || response.Result.Count == 0 ) return null;
                return response.Result;
            }
        }
        static class Messages
        {
            #region Urls

            private const string LeaveNotificationUrl = "LeaveView?Id={0}";
            private const string LeaveApprovalUrl = "LeaveView?Id={0}&RequestId={1}";

            private const string ExcuseNotificationUrl = "ExcuseView?Id={0}";
            private const string ExcuseApprovalUrl = "ExcuseView?Id={0}&RequestId={1}";

            private const string AwayNotificationUrl = "AwayView?Id={0}";
            private const string AwayApprovalUrl = "AwayView?Id={0}&RequestId={1}";

            #endregion

            public static string GetNotificationUrl( WorkflowLeaveTargetType type )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return ExcuseNotificationUrl;
                    case WorkflowLeaveTargetType.Away:
                        return AwayNotificationUrl;
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return LeaveNotificationUrl;
                }
            }
            public static string GetApprovalUrl( WorkflowLeaveTargetType type )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return ExcuseApprovalUrl;
                    case WorkflowLeaveTargetType.Away:
                        return AwayApprovalUrl;
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return LeaveApprovalUrl;
                }
            }

            public static string GetApprovalMessage( WorkflowLeaveTargetType type , Person person )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.ExcuseRequest ) , person.FullName );
                    case WorkflowLeaveTargetType.Away:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AwayRequest ) , person.FullName );
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.LeaveRequest ) , person.FullName );
                }
            }
            public static string GetApprovalMessageArabic( WorkflowLeaveTargetType type , Person person )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.ExcuseRequestAr ) , person.DetailedInfo.FullNameCultureVarient );
                    case WorkflowLeaveTargetType.Away:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AwayRequestAr ) , person.DetailedInfo.FullNameCultureVarient );
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.LeaveRequestAr ) , person.DetailedInfo.FullNameCultureVarient );
                }
            }

            public static string GetApprovalMessageForDelegate( WorkflowLeaveTargetType type , Person person )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.ExcuseRequestForDelegate ) , person.FullName );
                    case WorkflowLeaveTargetType.Away:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AwayRequestForDelegate ) , person.FullName );
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.LeaveRequestForDelegate ) , person.FullName );
                }
            }
            public static string GetApprovalMessageForDelegateArabic( WorkflowLeaveTargetType type , Person person )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.ExcuseRequestArForDelegate ) , person.DetailedInfo.FullNameCultureVarient );
                    case WorkflowLeaveTargetType.Away:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AwayRequestArForDelegate ) , person.DetailedInfo.FullNameCultureVarient );
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.LeaveRequestArForDelegate ) , person.DetailedInfo.FullNameCultureVarient );
                }
            }

            public static string GetRequestJustifyEmployee( WorkflowLeaveTargetType type )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.ExcuseRequestJustifyEmployee );
                    case WorkflowLeaveTargetType.Away:
                        return Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AwayRequestJustifyEmployee );
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.LeaveRequestJustifyEmployee );
                }
            }
            public static string GetRequestJustifyEmployeeArabic( WorkflowLeaveTargetType type )
            {
                switch ( type )
                {
                    case WorkflowLeaveTargetType.Excuse:
                        return Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.ExcuseRequestJustifyEmployeeAr );
                    case WorkflowLeaveTargetType.Away:
                        return Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.AwayRequestJustifyEmployeeAr );
                    case WorkflowLeaveTargetType.Leave:
                    default:
                        return Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.LeaveRequestJustifyEmployeeAr );
                }
            }
        }

        # endregion
    }
}