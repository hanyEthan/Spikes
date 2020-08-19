using System;
using System.Collections.Generic;
using System.Globalization;
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
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Actions;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Data;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Evaluators;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Models;
using ADS.Tamam.Common.Handlers.Workflow.Notifications.Models;
using ADS.Tamam.Common.Workflow.Notifications.Data;
using CultureResources = ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Workflow.Notifications.Actions;
using ADS.Common;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Definitions
{
    [DataContract(IsReference = true)]
    public class AttendanceManualEditApprovalWorkflowDefinition : IWorkflowDefinition
    {
        # region IWorkflowDefinition

        public Guid Id { get { return new Guid("D344276E-694F-4D9C-9FA6-791E9DEB2ADC"); } }
        public List<string> WorkflowSupportingTypes
        {
            get
            {
                return new List<string>()
                {
                    typeof ( AttendanceManualEditApprovalWorkflowDefinition ).AssemblyQualifiedName ,

                    typeof ( AttendanceManualEditReviewWorkflowData ).AssemblyQualifiedName ,
                    typeof ( NotificationWorkflowData ).AssemblyQualifiedName ,
                    typeof ( WorkflowAttendanceManualEditReviewStatus ).AssemblyQualifiedName ,
                    typeof ( ManualAttendanceReviewWorkflowEvaluator ).AssemblyQualifiedName ,

                    typeof ( CancellationWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( InitializationWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( ApprovalWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( ManualAttendanceReviewWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( NotificationResetWorkflowAction ).AssemblyQualifiedName ,
                    typeof ( NotificationWorkflowAction ).AssemblyQualifiedName ,
                };
            }
        }
        public WorkflowInstance Process(IWorkflowTarget target)
        {
            try
            {
                var policy = GetAttendancePolicy(target);
                var instance = BuildInstance(target, policy);

                return instance;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }

        # endregion

        # region Fields

        private const string _notificationUrl = "AttendanceView?Id={0}";                // TODO : need to be modified ...
        private const string _approvalUrl = "AttendanceView?Id={0}";                    // TODO : need to be modified ...

        # endregion

        # region Helpers

        private WorkflowInstance BuildInstance(IWorkflowTarget target, AttendancePolicy policy)
        {
            if (target == null || policy == null) return null;

            var instance = new WorkflowInstance(policy.Policy.Id.ToString());
            instance.Id = Guid.NewGuid();

            # region data ...

            var historyItem = (AttendanceRawDataHistoryItem)target;
            var targetOwner = GetPerson(historyItem.PersonId);

            var targetNotificationUrl = _notificationUrl;
            var targetApprovalUrl = _approvalUrl;
            var targetApprovalMessage = GetApprovalMessage(historyItem, targetOwner);
            var targetApprovalMessage_AR = GetApprovalMessage_AR(historyItem, targetOwner);
            var targetApprovalMessageForDelegate = GetApprovalMessageForDelegate(historyItem, targetOwner);
            var targetApprovalMessageForDelegate_AR = GetApprovalMessageForDelegate_AR(historyItem, targetOwner);



            // wf data ...
            var personId = target.PersonId.ToString();
            var targetId = target.Id.ToString();
            var notificationId = Broker.CommunicationHandler.EncryptQueryString(string.Format(targetNotificationUrl, target.Id.ToString()));

            // message metadata..
            var type_En = GetAttendanceEventType_EN(historyItem);
            var date_En = GetAttendanceDate_EN(historyItem);
            var type_AR = GetAttendanceEventType_AR(historyItem);
            var date_AR = GetAttendanceDate_AR(historyItem);

            // main message ...
            var message = Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.ManualAttendanceApproval_RequestStatus);
            var approvedMessage = string.Format(message, type_En, date_En, CultureResources.Organization.Approved);
            var rejectedMessage = string.Format(message, type_En, date_En, CultureResources.Organization.Rejected);

            message = Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.ManualAttendanceApproval_RequestStatusAr);
            var approvedMessage_AR = string.Format(message, type_AR, date_AR, CultureResources.Organization.ApprovedAr);
            var rejectedMessage_AR = string.Format(message, type_AR, date_AR, CultureResources.Organization.RejectedAr);

            var dataHandlerPersonnel = new PersonnelDataHandler();
            var person = dataHandlerPersonnel.GetPerson(target.PersonId, SystemSecurityContext.Instance).Result;
            var personHRPolicy = GetHRPolicy(person);
            var CCs = personHRPolicy.CCs;

            var workflowNotificationTargetType = WorkflowNotificationTargetType.AttendanceManualEdit.ToString().ToLower();

            var messageForRequesterSucceeded_Code = Guid.NewGuid().ToString();
            var messageForRequesterSucceeded = new NotificationWorkflowData(messageForRequesterSucceeded_Code, personId, targetId, approvedMessage, approvedMessage_AR, null, null, NotificationType.Information, GetActionUrl(messageForRequesterSucceeded_Code), workflowNotificationTargetType, NotificationTargetType.AttendanceManualEdit, false, CCs);

            var messageForRequesterDenied_Code = Guid.NewGuid().ToString();
            var messageForRequesterDenied = new NotificationWorkflowData(messageForRequesterDenied_Code, personId, targetId, rejectedMessage, rejectedMessage_AR, null, null, NotificationType.Information, GetActionUrl(messageForRequesterDenied_Code), workflowNotificationTargetType, NotificationTargetType.AttendanceManualEdit, false, CCs);
            //var messageForRequesterCancelled = new NotificationWorkflowData( "" , personId , targetId , subTargetId , cancelledMessage , cancelledMessage_AR , cancelledMessageForDelegate , cancelledMessageForDelegate_AR , NotificationType.Information , notificationId , workflowNotificationTargetType , NotificationTargetType.AttendanceManualEdit , true , null );

            var dataForTargetApproval = new AttendanceManualEditReviewWorkflowData() { TargetId = target.Id.ToString(), TargetStatus = WorkflowAttendanceManualEditReviewStatus.Approved.ToString() };
            var dataForTargetDenial = new AttendanceManualEditReviewWorkflowData() { TargetId = target.Id.ToString(), TargetStatus = WorkflowAttendanceManualEditReviewStatus.Denied.ToString() };
            // var dataForTargetCancellation = new AttendanceManualEditReviewWorkflowData() { TargetId = target.Id.ToString() , SubTargetId = subTargetId , TargetStatus = WorkflowAttendanceManualEditReviewStatus.Cancelled.ToString() };

            var metadata = "";

            # endregion

            #region nodes ( main )

            var initial = instance.Initial<WorkflowStep>().Run<InitializationWorkflowAction>(null);


            var approved = instance.Step<WorkflowStep>().Run<ApprovalWorkflowAction>(dataForTargetApproval);
            var denied = instance.Step<WorkflowStep>().Run<ApprovalWorkflowAction>(dataForTargetDenial);

            var cancelled = instance.Cancellation<WorkflowStep>().Run<CancellationWorkflowAction>(null);
            cancelled.Next<WorkflowStep>().Run<NotificationResetWorkflowAction>(null);

            var reset = instance.Resetting<WorkflowStep>().Run<CancellationWorkflowAction>(null);
            reset.Next<WorkflowStep>().Run<NotificationResetWorkflowAction>(null);

            var currunt = initial;

            #endregion

            #region nodes ( logic )

            // check person Attendance Policy if it allow Attendance Manual Edit Approvals ..
            if (policy.EnableManualEditApprovals)
            {
                // construct wf node structure based on the selected approval policy ...
                foreach (var item in policy.ManualEditApprovalPolicy.ApprovalSteps.OrderBy(s => s.Sequance).ToList())
                {
                    var approverId = GetPersonIdByStepType(person, personHRPolicy, item);
                    if (approverId != Guid.Empty && approverId != target.PersonId)    // check that there's a valid person in this level ...
                    {
                        var fullTargetApprovalUrl = Broker.CommunicationHandler.EncryptQueryString(string.Format(targetApprovalUrl, target.Id.ToString(), ""));

                        var messageForReviewer_Code = Guid.NewGuid().ToString();
                        var messageForReviewer = new NotificationWorkflowData(messageForReviewer_Code, approverId.ToString(), targetId, targetApprovalMessage, targetApprovalMessage_AR, targetApprovalMessageForDelegate, targetApprovalMessageForDelegate_AR, NotificationType.Action, GetActionUrl(messageForReviewer_Code), workflowNotificationTargetType, NotificationTargetType.AttendanceManualEdit, true, CCs);
                        var approvalData = new AttendanceManualEditReviewWorkflowData()
                        {
                            TargetId = target.Id.ToString(),
                            ApproverIdExpected = approverId.ToString(),
                            PersonId = approverId.ToString()
                        };


                        var notify = currunt.Next<WorkflowStep>().Run<NotificationWorkflowAction>(messageForReviewer);    // notify     
                        var approval = notify.Next<WorkflowStepCommand, ManualAttendanceReviewWorkflowEvaluator>().Run<ManualAttendanceReviewWorkflowAction>(approvalData).Checkpoint(approvalData);

                        // note : the step data will be filled later in the runtime by the action ...                      
                        approval.When(WorkflowAttendanceManualEditReviewStatus.Denied.ToString().ToLower()).Next<WorkflowStep>().Run<NotificationResetWorkflowAction>(null).Next<WorkflowStep>().Run<NotificationWorkflowAction>(messageForRequesterDenied).Next(denied); // denied
                        currunt = approval.When(WorkflowAttendanceManualEditReviewStatus.Approved.ToString().ToLower()).Next<WorkflowStep>().Run<NotificationResetWorkflowAction>(null).Next<WorkflowStep>(); // approved

                        metadata += approverId + ",";
                    }

                }
            }
            currunt.Next<WorkflowStep>().Run<NotificationWorkflowAction>(messageForRequesterSucceeded).Next(approved);   // finish

            #endregion

            return instance;
        }

        // to support policies ..
        private AttendancePolicy GetAttendancePolicy(IWorkflowTarget target)
        {
            if (target == null) return null;

            var policies = GetPolicies(target.PersonId);
            if (policies == null) return null;

            var nativePolicy = policies.FirstOrDefault(p => p.PolicyTypeId == Guid.Parse(PolicyTypes.AttendancePolicyType));
            if (nativePolicy == null) return null;

            var policy = new AttendancePolicy(nativePolicy);

            return policy;
        }
        private List<Policy> GetPolicies(Guid personId)
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies(personId, SystemRequestContext.Instance);
            if (response.Type != ResponseState.Success || response.Result == null || response.Result.Count == 0) return null;

            return response.Result;
        }

        private Guid GetPersonIdByStepType(Person person, HRPolicy personHRPolicy, ApprovalStep item)
        {
            try
            {
                switch (item.StepType)
                {
                    case StepType.Manager: return person.AccountInfo.ReportingTo.AccountInfo.Activated ? person.AccountInfo.ReportingToId.Value : Guid.Empty;
                    case StepType.SuperManager: return person.AccountInfo.ReportingTo.AccountInfo.ReportingTo.AccountInfo.Activated ? person.AccountInfo.ReportingTo.AccountInfo.ReportingToId.Value : Guid.Empty;
                    case StepType.HR:
                        {
                            var HR = personHRPolicy.HRRole == null ? null : GetHR(personHRPolicy);
                            return HR != null ? HR.Id : Guid.Empty;
                        }
                    default: return Guid.Empty;
                }
            }
            catch
            {
                return Guid.Empty;
            }
        }
        private HRPolicy GetHRPolicy(Person person)
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies(person,
                new PolicyFilters(new Guid(PolicyTypes.HRPolicyType), true), SystemRequestContext.Instance);
            if (response.Type != ResponseState.Success || response.Result == null || response.Result.Count == 0) return null;
            return new HRPolicy(response.Result.FirstOrDefault());

        }
        private Person GetHR(HRPolicy HRPolicy)
        {
            var dataHandlerPersonnel = new PersonnelDataHandler();
            var dataHandlerOrganization = new OrganizationDataHandler();

            var HRDepartment = HRPolicy.HRDepartment;
            if (HRDepartment == null) return null;

            var HRDepartments = dataHandlerOrganization.GetDepartmentsByRoot(HRDepartment.Id).Result;

            var HRPerson = dataHandlerPersonnel.GetPersonnel(new PersonSearchCriteria
            {
                ActivationStatus = true,
                Departments = HRDepartments.Select(d => d.Id).ToList(),
                Titles = new List<int> { HRPolicy.HRRole.Id },
            }, SystemSecurityContext.Instance).Result.Persons.FirstOrDefault();

            return HRPerson;
        }


        // to support notifications..
        private string GetApprovalMessage(AttendanceRawDataHistoryItem historyItem, Person targetOwner)
        {
            var personName = targetOwner.FullName;
            var type = GetAttendanceEventType_EN(historyItem);
            var from = GetAttendanceFromTime_EN(historyItem);
            var to = GetAttendanceToTime_EN(historyItem);
            var date = GetAttendanceDate_EN(historyItem);
            var message = Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.ManualAttendanceApproval_Manager);

            return string.Format(message, personName, type, from, to, historyItem.ChangedByUserId, date);
        }
        private string GetApprovalMessage_AR(AttendanceRawDataHistoryItem historyItem, Person targetOwner)
        {
            var personName = targetOwner.DetailedInfo.FullNameCultureVarient;
            var type = GetAttendanceEventType_AR(historyItem);
            var from = GetAttendanceFromTime_AR(historyItem);
            var to = GetAttendanceToTime_AR(historyItem);
            var date = GetAttendanceDate_AR(historyItem);
            var message = Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.ManualAttendanceApproval_ManagerAr);

            return string.Format(message, personName, type, from, to, historyItem.ChangedByUserId, date);
        }
        private string GetApprovalMessageForDelegate(AttendanceRawDataHistoryItem historyItem, Person targetOwner)
        {
            var personName = targetOwner.FullName;
            var type = GetAttendanceEventType_EN(historyItem);
            var from = GetAttendanceFromTime_EN(historyItem);
            var to = GetAttendanceToTime_EN(historyItem);
            var date = GetAttendanceDate_EN(historyItem);
            var message = Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.ManualAttendanceApproval_ManagerDelegate);

            return string.Format(message, personName, type, from, to, historyItem.ChangedByUserId, date);
        }
        private string GetApprovalMessageForDelegate_AR(AttendanceRawDataHistoryItem historyItem, Person targetOwner)
        {
            var personName = targetOwner.DetailedInfo.FullNameCultureVarient;
            var type = GetAttendanceEventType_AR(historyItem);
            var from = GetAttendanceFromTime_AR(historyItem);
            var to = GetAttendanceToTime_AR(historyItem);
            var date = GetAttendanceDate_AR(historyItem);
            var message = Broker.ConfigurationHandler.GetValue(Constants.TamamWorkflowMessages.Section, Constants.TamamWorkflowMessages.ManualAttendanceApproval_ManagerDelegateAr);

            return string.Format(message, personName, type, from, to, historyItem.ChangedByUserId, date);
        }


        // internals..
        private string GetAttendanceEventType_EN(AttendanceRawDataHistoryItem historyItem)
        {
            switch (historyItem.AttendanceRawData.Type)
            {
                case AttendanceEventType.In:
                    return CultureResources.Organization.ManualAttendanceApproval_AttendanceType_In;
                case AttendanceEventType.Out:
                    return CultureResources.Organization.ManualAttendanceApproval_AttendanceType_Out;
                default:
                    return "";
            }
        }
        private string GetAttendanceFromTime_EN(AttendanceRawDataHistoryItem historyItem)
        {
            string oldValue = historyItem.ValueOld.HasValue ? historyItem.ValueOld.Value.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US")) : CultureResources.Organization.ManualAttendanceApproval_Empty;
            return oldValue;
        }
        private string GetAttendanceToTime_EN(AttendanceRawDataHistoryItem historyItem)
        {
            string newValue = historyItem.ValueNew.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
            return newValue;
        }
        private string GetAttendanceDate_EN(AttendanceRawDataHistoryItem historyItem)
        {
            return historyItem.ValueNew.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        }

        private string GetAttendanceEventType_AR(AttendanceRawDataHistoryItem historyItem)
        {
            switch (historyItem.AttendanceRawData.Type)
            {
                case AttendanceEventType.In:
                    return CultureResources.Organization.ManualAttendanceApproval_AttendanceType_InAr;
                case AttendanceEventType.Out:
                    return CultureResources.Organization.ManualAttendanceApproval_AttendanceType_OutAr;
                default:
                    return "";
            }
        }
        private string GetAttendanceFromTime_AR(AttendanceRawDataHistoryItem historyItem)
        {
            string oldValue = historyItem.ValueOld.HasValue ? historyItem.ValueOld.Value.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("ar-EG")) : CultureResources.Organization.ManualAttendanceApproval_EmptyAr;
            return oldValue;
        }
        private string GetAttendanceToTime_AR(AttendanceRawDataHistoryItem historyItem)
        {
            string newValue = historyItem.ValueNew.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
            return newValue;
        }
        private string GetAttendanceDate_AR(AttendanceRawDataHistoryItem historyItem)
        {
            return historyItem.ValueNew.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("ar-EG"));
        }

        private Person GetPerson(Guid personId)
        {
            return TamamServiceBroker.PersonnelHandler.GetPerson(personId, SystemRequestContext.Instance).Result;
        }

        private string GetActionUrl(string code)
        {
            return Broker.CommunicationHandler.EncryptQueryString(string.Format("Notifications?Code={0}", code));
        }

        # endregion
    }
}