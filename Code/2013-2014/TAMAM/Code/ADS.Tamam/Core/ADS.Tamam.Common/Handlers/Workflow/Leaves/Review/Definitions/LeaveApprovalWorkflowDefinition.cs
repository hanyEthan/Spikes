using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Models.Enums;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Actions;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Data;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Evaluators;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;
using ADS.Tamam.Common.Handlers.Workflow.Notifications.Models;
using ADS.Tamam.Common.Workflow.Leaves.Review.Actions;
using ADS.Tamam.Common.Workflow.Leaves.Review.Data;
using ADS.Tamam.Common.Workflow.Leaves.Review.Evaluators;
using ADS.Tamam.Common.Workflow.Notifications.Actions;
using ADS.Tamam.Common.Workflow.Notifications.Data;
using CultureResources = ADS.Tamam.Resources.Culture;
using ADS.Common;

namespace ADS.Tamam.Common.Workflow.Leaves.Review.Definitions
{
    [DataContract( IsReference = true )]
    public class LeaveApprovalWorkflowDefinition : IWorkflowDefinition
    {
        #region IWorkflowDefinition

        public Guid Id { get { return new Guid( "2779b32d-cd73-43c9-98dd-eaf333edf4d3" ); } }
        public List<string> WorkflowSupportingTypes
        {
            get
            {
                return new List<string>
                {
                    typeof ( LeaveApprovalWorkflowDefinition ).AssemblyQualifiedName ,

                    typeof ( LeaveReviewWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( LeaveApprovalWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( NotificationWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( NotificationResetWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( LeaveReviewIterationJustificationWorkflowAction ).AssemblyQualifiedName ,

                    typeof ( WorkflowLeaveReviewStatus ).AssemblyQualifiedName ,

                    typeof ( LeaveReviewWorkflowData ).AssemblyQualifiedName ,
                    typeof ( NotificationWorkflowData ).AssemblyQualifiedName ,
                    typeof ( NotificationsSettingsWorkflowData ).AssemblyQualifiedName ,
                    typeof ( LeaveReviewIterationJustificationWorkflowData ).AssemblyQualifiedName ,

                    typeof ( LeaveReviewWorkflowEvaluator ).AssemblyQualifiedName ,
                    typeof ( LeaveReviewIterationsWorkflowEvaluator ).AssemblyQualifiedName ,
                };
            }
        }
        public WorkflowInstance Process( IWorkflowTarget target )
        {
            try
            {
                var policy = Policies.GetApprovalPolicy( target );      // Important: must get ( ApprovalPolicy ) in here, due to ( Policies ) nested class depends on it to fill ( PersonPolicies ) property, that used in other places after this call.
                var instance = BuildInstance( target , policy );

                return instance;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        #endregion

        # region Helpers

        private WorkflowInstance BuildInstance( IWorkflowTarget target , ApprovalPolicy approvalPolicy )
        {
            if ( target == null || approvalPolicy == null ) return null;

            #region data ...

            var targetType = WorkflowHelpers.GetWorkflowLeaveTargetType( target );
            var targetOwner = WorkflowHelpers.GetWorkflowTargetPerson( target );
            var maxIterations = WorkflowHelpers.GetMaxDenialIterations( approvalPolicy );
            var includePassedStepsPerIteration = WorkflowHelpers.GetIncludePassedStepsPerIteration( approvalPolicy );

            var targetNotificationUrl = Messages.GetNotificationUrl( targetType );
            var targetApprovalUrl = Messages.GetApprovalUrl( targetType );

            var targetApprovalMessage = Messages.GetApprovalMessage( targetType , targetOwner );
            var targetApprovalMessageArabic = Messages.GetApprovalMessageArabic( targetType , targetOwner );
            var targetApprovalMessageForDelegate = Messages.GetApprovalMessageForDelegate( targetType , targetOwner );
            var targetApprovalMessageForDelegateArabic = Messages.GetApprovalMessageForDelegateArabic( targetType , targetOwner );
            var RequestJustifyEmployee = Messages.GetRequestJustifyEmployee( targetType );
            var RequestJustifyEmployeeArabic = Messages.GetRequestJustifyEmployeeArabic( targetType );

            // wf data ...
            var personId = target.PersonId.ToString();
            var targetId = target.Id.ToString();
            var notificationId = Broker.CommunicationHandler.EncryptQueryString( string.Format( targetNotificationUrl , target.Id.ToString() ) );

            // main message ...
            var approvedMessage = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.RequestStatus ) , CultureResources.Organization.Approved );
            var rejectedMessage = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.RequestStatus ) , CultureResources.Organization.Rejected );
            var cancelledMessage = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.RequestStatus ) , CultureResources.Organization.Cancelled );

            var approvedMessageArabic = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.RequestStatusAr ) , CultureResources.Organization.ApprovedAr );
            var rejectedMessageArabic = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.RequestStatusAr ) , CultureResources.Organization.RejectedAr );
            var cancelledMessageArabic = string.Format( Broker.ConfigurationHandler.GetValue( Constants.TamamWorkflowMessages.Section , Constants.TamamWorkflowMessages.RequestStatusAr ) , CultureResources.Organization.CancelledAr );

            //// delegate message ...
            //var approvedMessageForDelegate = string.Format(Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.RequestStatusForDelegate), targetOwner.FullName, CultureResources.Organization.Approved);
            //var rejectedMessageForDelegate = string.Format(Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.RequestStatusForDelegate), targetOwner.FullName, CultureResources.Organization.Rejected);
            //var cancelledMessageForDelegate = string.Format(Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.RequestStatusForDelegate), targetOwner.FullName, CultureResources.Organization.Cancelled);

            //var approvedArabicMessageForDelegate = string.Format(Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.RequestStatusArForDelegate), targetOwner.DetailedInfo.FullNameCultureVarient, CultureResources.Organization.ApprovedAr);
            //var rejectedArabicMessageForDelegate = string.Format(Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.RequestStatusArForDelegate), targetOwner.DetailedInfo.FullNameCultureVarient, CultureResources.Organization.RejectedAr);
            //var cancelledArabicMessageForDelegate = string.Format(Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.RequestStatusArForDelegate), targetOwner.DetailedInfo.FullNameCultureVarient, CultureResources.Organization.CancelledAr);

            var leaveType = targetType == WorkflowLeaveTargetType.Leave ? WorkflowNotificationTargetType.Leaves.ToString().ToLower() : WorkflowNotificationTargetType.Excuse.ToString().ToLower();
            var notificationTargetType = targetType == WorkflowLeaveTargetType.Leave ? NotificationTargetType.Leaves : NotificationTargetType.Excuses;

            var personHRPolicy = Policies.GetHRPolicy();
            var CCs = personHRPolicy.CCs;

            var messageForRequesterSucceeded = new NotificationWorkflowData( "" , personId , targetId , approvedMessage , approvedMessageArabic , null , null , NotificationType.Information , notificationId , leaveType , notificationTargetType , false , CCs );
            var messageForRequesterDenied = new NotificationWorkflowData( "" , personId , targetId , rejectedMessage , rejectedMessageArabic , null , null , NotificationType.Information , notificationId , leaveType , notificationTargetType , false , CCs );
            var messageForRequesterCancelled = new NotificationWorkflowData( "" , personId , targetId , cancelledMessage , cancelledMessageArabic , null , null , NotificationType.Information , notificationId , leaveType , notificationTargetType , false , CCs );
            var messageForRequesterPartialDenied = new NotificationWorkflowData( "" , personId , targetId , RequestJustifyEmployee , RequestJustifyEmployeeArabic , null , null , NotificationType.Comment , notificationId , leaveType , notificationTargetType , false , CCs );
            var justificationData = new LeaveReviewIterationJustificationWorkflowData( target.PersonId.ToString() , target.Id.ToString() , targetType );

            var dataForTargetApproval = new LeaveReviewWorkflowData() { TargetId = target.Id.ToString() , TargetStatus = WorkflowLeaveReviewStatus.Approved.ToString() , TargetType = targetType };
            var dataForTargetDenial = new LeaveReviewWorkflowData() { TargetId = target.Id.ToString() , TargetStatus = WorkflowLeaveReviewStatus.Denied.ToString() , TargetType = targetType };
            var dataForTargetCancellation = new LeaveReviewWorkflowData() { TargetId = target.Id.ToString() , TargetStatus = WorkflowLeaveReviewStatus.Cancelled.ToString() , TargetType = targetType };

            var metadata = "";

            #endregion
            #region nodes ( main )

            var instance = new WorkflowInstance( approvalPolicy.Policy.Id.ToString() );
            instance.Data = new NotificationsSettingsWorkflowData( maxIterations , includePassedStepsPerIteration );

            var initial = instance.Initial<WorkflowStep>();
            var approved = instance.Step<WorkflowStep>().Run<LeaveApprovalWorkflowAction>( dataForTargetApproval );
            var denied = instance.Step<WorkflowStep>().Run<LeaveApprovalWorkflowAction>( dataForTargetDenial );

            var cancelled = instance.Cancellation<WorkflowStep>().Run<LeaveApprovalWorkflowAction>( dataForTargetCancellation );
            cancelled.Next<WorkflowStep>().Run<NotificationResetWorkflowAction>( null ).Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForRequesterCancelled );
            var reset = instance.Resetting<WorkflowStep>().Run<NotificationResetWorkflowAction>( null );

            var currunt = initial;

            #endregion
            #region nodes ( logic )

            #region prepping steps from policy

            // get approval steps filtered by condition and sorted by sequence..
            var approvalSteps = approvalPolicy.ApprovalSteps.Where( x => target.EffectiveAmount >= x.Condition ).OrderBy( s => s.Sequance ).ToList();

            // get valid list of approvals Ids..
            var Approvals = new List<Guid>();
            foreach ( var step in approvalSteps )
            {
                var approverId = WorkflowHelpers.GetPersonIdByStepType( targetOwner , personHRPolicy , step );
                if ( IsValidApprover( approverId , target.PersonId ) ) Approvals.Add( approverId );
            }
            
            #endregion

            foreach ( var approverId in Approvals )
            {
                #region prepping the declination iteration logic ...

                var peronJustificationStep = instance.Step<WorkflowStep>().Run<NotificationResetWorkflowAction>( null );
                var iterationBackPoint = peronJustificationStep.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForRequesterPartialDenied )
                                                               .Next<WorkflowStepCommand>().Run<LeaveReviewIterationJustificationWorkflowAction>( justificationData )
                                                               .Next<WorkflowStep>().Run<NotificationResetWorkflowAction>( null );
                                                               //.Checkpoint( justificationData )
                                                               //.Next( initial );

                var finalDenial = instance.Step<WorkflowStep>().Run<NotificationResetWorkflowAction>( null );
                    finalDenial.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForRequesterDenied )
                               .Next( denied );   // denied

                var iterationQuestion = instance.Step<WorkflowStep , LeaveReviewIterationsWorkflowEvaluator>( instance.Data );
                iterationQuestion.When( "max" ).Next( finalDenial );
                iterationQuestion.When( "" ).Next( peronJustificationStep );//.Checkpoint( justificationData );

                #endregion
                #region prepping step data ...

                // data ...
                var fullTargetApprovalUrl = Broker.CommunicationHandler.EncryptQueryString( string.Format( targetApprovalUrl , target.Id.ToString() , "" ) );
                var messageForReviewer = new NotificationWorkflowData( "" , approverId.ToString() , targetId , targetApprovalMessage , targetApprovalMessageArabic , targetApprovalMessageForDelegate , targetApprovalMessageForDelegateArabic , NotificationType.Action , fullTargetApprovalUrl , leaveType , notificationTargetType , true , CCs );
                var approvalData = new LeaveReviewWorkflowData() { TargetId = target.Id.ToString() , ApproverIdExpected = approverId.ToString() , PersonId = approverId.ToString() };

                #endregion
                #region steps ...

                // notify
                var notify = currunt.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForReviewer );

                // question ...
                var approval = notify.Next<WorkflowStepCommand , LeaveReviewWorkflowEvaluator>().Run<LeaveReviewWorkflowAction>( approvalData )
                                     .Checkpoint( approvalData );
                // if approved, then ...
                currunt = approval.When( WorkflowLeaveReviewStatus.Approved.ToString().ToLower() )
                                  .Next<WorkflowStep>().Run<NotificationResetWorkflowAction>( null )
                                  .Next<WorkflowStep>();

                // if system approved, then ...
                approval.When( WorkflowLeaveReviewStatus.SystemApproved.ToString().ToLower() )
                        .Next<WorkflowStep>().Run<NotificationResetWorkflowAction>( null )
                        .Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForRequesterSucceeded )
                        .Next( approved );   //Finish the work flow as approved

                // if denied, then ...
                var managerDenied = approval.When( WorkflowLeaveReviewStatus.Denied.ToString().ToLower() )
                                            .Next( iterationQuestion );

                #endregion
                #region continue : prepping the declination iteration logic ...

                iterationBackPoint.Next( includePassedStepsPerIteration ? initial : notify );

                #endregion

                metadata += approverId + ",";
            }

            currunt.Next<WorkflowStep>().Run<NotificationWorkflowAction>( messageForRequesterSucceeded ).Next( approved );   // finish

            #endregion

            instance.Metadata = metadata;
            return instance;
        }

        private bool IsValidApprover( Guid approverId , Guid targetOwner )
        {
            if ( approverId == Guid.Empty ) return false;
            return approverId != targetOwner;
        }

        # endregion

        # region nested

        static class WorkflowHelpers
        {
            public static WorkflowLeaveTargetType GetWorkflowLeaveTargetType( IWorkflowTarget target )
            {
                if ( target is Excuse )
                {
                    var excuse = target as Excuse;
                    return excuse.IsAwayExcuse() ? WorkflowLeaveTargetType.Away : WorkflowLeaveTargetType.Excuse;
                }
                return WorkflowLeaveTargetType.Leave;
            }
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
            public static ApprovalPolicy GetApprovalPolicy( IWorkflowTarget target )
            {
                if ( target == null ) return null;

                PersonPolicies = GetPolicies( target.PersonId );
                if ( PersonPolicies == null ) return null;

                if ( target is Leave ) return GetApprovalPolicy( target as Leave );
                if ( target is Excuse ) return GetApprovalPolicy( target as Excuse );

                return null;
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
            private static ApprovalPolicy GetApprovalPolicy( Leave leave )
            {
                var policies = PersonPolicies.Where( p => p.PolicyTypeId == Guid.Parse( PolicyTypes.LeavePolicyType ) ).ToList();
                var policy = LeavePolicy.GetInstances( policies , leave.LeaveTypeId ).FirstOrDefault();
                return policy.ApprovalPolicy;
            }
            private static ApprovalPolicy GetApprovalPolicy( Excuse target )
            {
                var policies = PersonPolicies.Where( p => p.PolicyTypeId == Guid.Parse( PolicyTypes.ExcusesPolicyType ) ).ToList();
                var policy = ExcusePolicy.GetInstances( policies , target.ExcuseTypeId ).FirstOrDefault();
                return policy.ApprovalPolicy;
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