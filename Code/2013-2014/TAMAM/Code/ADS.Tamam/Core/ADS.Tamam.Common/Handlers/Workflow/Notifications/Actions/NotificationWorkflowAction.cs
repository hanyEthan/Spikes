using System;
using System.Runtime.Serialization;
using ADS.Common.Contracts.Notification;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Notifications.Models;
using ADS.Tamam.Common.Workflow.Notifications.Data;
using CultureResources = ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Context;
using System.Linq;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Workflow.Notifications.Actions
{
    [DataContract( IsReference = true )]
    public class NotificationWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                data = ( data != null && ( data is NotificationWorkflowData ) ) ? data : this.Data;   // if nothing's passed, use locally stored data ...

                if ( data == null || !( data is NotificationWorkflowData ) ) return false;
                var notificationData = data as NotificationWorkflowData;

                #region message ...

                WorkflowNotificationTargetType targetType;
                if ( !Enum.TryParse<WorkflowNotificationTargetType>( notificationData.TargetType , true , out targetType ) ) return false;


                /*  delegate */
                Guid? delegateID = null;
                if ( notificationData.IncludeDelegate ) delegateID = GetDelegateId( data.PersonId );

                NotificationMessage message;
                NotificationMessage delegateMessage = null;

                switch ( targetType )
                {
                    case WorkflowNotificationTargetType.Leaves:
                        {
                            message = new LeaveApprovalNotification
                            {
                                Code = notificationData.Code ,
                                PersonId = notificationData.PersonId ,
                                TargetId = notificationData.TargetId ,
                                Message = notificationData.Message ,
                                MessageCultureVariant = notificationData.MessageCultureVariant ,
                                Type = notificationData.Type ,
                                ActionUrl = notificationData.ActionUrl ,
                                ActionName = CultureResources.Organization.View ,
                                ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                TargetType = notificationData.NotificationTargetType ,
                                CreationTime = DateTime.Now ,
                                CCs = notificationData.CCs,
                            };

                            if ( notificationData.IncludeDelegate && delegateID != null )
                            {
                                delegateMessage = new LeaveApprovalNotification
                                {
                                    Code = notificationData.Code ,
                                    PersonId = delegateID.Value.ToString() ,
                                    TargetId = notificationData.TargetId ,
                                    Message = notificationData.Message ,
                                    MessageCultureVariant = notificationData.MessageCultureVariant ,
                                    Type = notificationData.Type ,
                                    ActionUrl = notificationData.ActionUrl ,
                                    ActionName = CultureResources.Organization.View ,
                                    ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                    TargetType = notificationData.NotificationTargetType ,
                                    CreationTime = DateTime.Now ,
                                    CCs = notificationData.CCs,
                                };
                            }
                        }
                        break;
                    case WorkflowNotificationTargetType.Excuse:
                        {
                            message = new ExcuseApprovalNotification
                            {
                                Code = notificationData.Code ,
                                PersonId = notificationData.PersonId ,
                                TargetId = notificationData.TargetId ,
                                Message = notificationData.Message ,
                                MessageCultureVariant = notificationData.MessageCultureVariant ,
                                Type = notificationData.Type ,
                                ActionUrl = notificationData.ActionUrl ,
                                ActionName = CultureResources.Organization.View ,
                                ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                TargetType = notificationData.NotificationTargetType ,
                                CreationTime = DateTime.Now ,
                                CCs = notificationData.CCs,
                            };

                            if ( notificationData.IncludeDelegate && delegateID != null )
                            {
                                delegateMessage = new ExcuseApprovalNotification
                                {
                                    Code = notificationData.Code ,
                                    PersonId = delegateID.Value.ToString() ,
                                    TargetId = notificationData.TargetId ,
                                    Message = notificationData.Message ,
                                    MessageCultureVariant = notificationData.MessageCultureVariant ,
                                    Type = notificationData.Type ,
                                    ActionUrl = notificationData.ActionUrl ,
                                    ActionName = CultureResources.Organization.View ,
                                    ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                    TargetType = notificationData.NotificationTargetType ,
                                    CreationTime = DateTime.Now ,
                                    CCs = notificationData.CCs,
                                };
                            }
                        }
                        break;
                    case WorkflowNotificationTargetType.AttendanceViolations:
                        {
                            message = new LateAttendanceNotification
                            {
                                Code = notificationData.Code ,
                                PersonId = notificationData.PersonId ,
                                TargetId = notificationData.TargetId ,
                                Message = notificationData.Message ,
                                MessageCultureVariant = notificationData.MessageCultureVariant ,
                                Type = notificationData.Type ,
                                ActionUrl = notificationData.ActionUrl ,
                                ActionName = CultureResources.Organization.View ,
                                ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                TargetType = notificationData.NotificationTargetType ,
                                CreationTime = DateTime.Now ,
                                CCs = notificationData.CCs,
                            };

                            if ( notificationData.IncludeDelegate && delegateID != null )
                            {
                                delegateMessage = new LateAttendanceNotification
                                {
                                    Code = notificationData.Code ,
                                    PersonId = delegateID.Value.ToString() ,
                                    TargetId = notificationData.TargetId ,
                                    Message = notificationData.Message ,
                                    MessageCultureVariant = notificationData.MessageCultureVariant ,
                                    Type = notificationData.Type ,
                                    ActionUrl = notificationData.ActionUrl ,
                                    ActionName = CultureResources.Organization.View ,
                                    ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                    TargetType = notificationData.NotificationTargetType ,
                                    CreationTime = DateTime.Now ,
                                    CCs = notificationData.CCs,
                                };
                            }
                        }
                        break;

                    case WorkflowNotificationTargetType.AttendanceManualEdit:
                        {
                            message = new AttendanceManualEditNotification
                            {
                                Code = notificationData.Code ,
                                PersonId = notificationData.PersonId ,
                                TargetId = notificationData.TargetId ,
                                Message = notificationData.Message ,
                                MessageCultureVariant = notificationData.MessageCultureVariant ,
                                Type = notificationData.Type ,
                                ActionUrl = notificationData.ActionUrl ,
                                ActionName = CultureResources.Organization.View ,
                                ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                TargetType = notificationData.NotificationTargetType ,
                                CreationTime = DateTime.Now ,
                                CCs = notificationData.CCs ,
                            };

                            if ( notificationData.IncludeDelegate && delegateID != null )
                            {
                                delegateMessage = new LateAttendanceNotification
                                {
                                    Code = notificationData.Code ,
                                    PersonId = delegateID.Value.ToString() ,
                                    TargetId = notificationData.TargetId ,
                                    Message = notificationData.Message ,
                                    MessageCultureVariant = notificationData.MessageCultureVariant ,
                                    Type = notificationData.Type ,
                                    ActionUrl = notificationData.ActionUrl ,
                                    ActionName = CultureResources.Organization.View ,
                                    ActionNameCultureVariant = CultureResources.Organization.ViewAr ,
                                    TargetType = notificationData.NotificationTargetType ,
                                    CreationTime = DateTime.Now ,
                                    CCs = notificationData.CCs ,
                                };
                            }
                        }
                        break;


                    default:
                        return false;
                }

                #endregion

                var notificationPolicy = GetNotificationPolicy( message.PersonId );
                var status = Broker.NotificationHandler.Notify( message , notificationPolicy );

                if ( delegateID != null && delegateMessage != null )
                {
                    var delegateNotificationPolicy = GetNotificationPolicy( delegateMessage.PersonId );
                    status &= Broker.NotificationHandler.Notify( delegateMessage , delegateNotificationPolicy );
                }
                return status;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        # region Helpers..

        private Guid? GetDelegateId( string personId )
        {
            Guid id = Guid.Parse( personId );
            var criteria = new PersonnelDelegatesSearchCriteria()
            {
                AllowPaging = false ,
                PersonnelIds = new List<Guid>() { id } ,
                StartDate = DateTime.Today ,
                EndDate = DateTime.Today
            };

            var delegatesResponse = TamamServiceBroker.PersonnelHandler.GetPersonnelDelegates( criteria , SystemRequestContext.Instance );
            if ( delegatesResponse.Type != ResponseState.Success )
            {
                return null;
            }
            var delegatePerson = delegatesResponse.Result.PersonnelDelegates != null ? delegatesResponse.Result.PersonnelDelegates.FirstOrDefault() : null;

            if ( delegatePerson == null )
            {
                return null;
            }
            else
            {
                return delegatePerson.DelegateId;
            }
        }

        // Get Notification Policy..
        private INotificationPolicy GetNotificationPolicy( string personId )
        {
            Guid id = Guid.Parse( personId );

            var policies = GetPersonPolicies( id , SystemRequestContext.Instance );
            var notificationPolicies = GetPersonPolicyByType( policies , PolicyTypes.NotificationsPolicyType );
            return notificationPolicies == null || notificationPolicies.Count == 0 ? null : new NotificationsPolicy( notificationPolicies[0] );
        }
        private IEnumerable<Policy> GetPersonPolicies( Guid personId , RequestContext requestContext )
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies( personId , requestContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private List<Policy> GetPersonPolicyByType( IEnumerable<Policy> policies , string PolicyTypeId )
        {
            return policies == null ? null : policies.Where( p => p.PolicyTypeId == new Guid( PolicyTypeId ) ).ToList();
        }

        # endregion
    }
}
