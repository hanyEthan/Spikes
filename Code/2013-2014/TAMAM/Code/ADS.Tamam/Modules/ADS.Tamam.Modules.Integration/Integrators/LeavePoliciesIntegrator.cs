using System;
using System.Collections.Generic;
using System.Linq;

using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Modules.Integration.DataHandlers;
using ADS.Tamam.Modules.Integration.Helpers;
using ADS.Tamam.Modules.Integration.Repositories;

using RAWLeavePolicy = ADS.Tamam.Modules.Integration.Models.LeavePolicy;
using LeavePolicy = ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized.LeavePolicy;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class LeavePoliciesIntegrator
    {
        #region Props

        private const string CreateKey = "Create Leave Policy Action";
        private const string EditKey = "Edit Leave Policy Action";

        private DetailCodeRepository LeaveTypesRepository { get; set; }

        private LeavePoliciesDataHandler _leavesPoliciesDataHandler;
        private LeavePoliciesDataHandler LeavesPoliciesDataHandler
        {
            get
            {
                return _leavesPoliciesDataHandler ?? ( _leavesPoliciesDataHandler = new LeavePoliciesDataHandler() );
            }
        }

        #endregion
        #region Ctor

        public LeavePoliciesIntegrator()
        {
            LeaveTypesRepository = new DetailCodeRepository( TamamConstants.MasterCodes.LeaveType );
        }

        #endregion
        #region publics

        public void Integrate()
        {
            var round = 0;
            XLogger.Info( LogHelper.BuildMessage( "Round {0}", round++ ) );
            IntegrateAll();
        }

        #endregion
        # region Actions

        private void Create( RAWLeavePolicy policy )
        {
            try
            {
                string approvalPolicyId = string.Empty;
                var newPolicy = Map( new Policy(), policy, approvalPolicyId );

                // Check if the ( System Policy Group ) have a leave policy with the same leave type.
                LeavePolicy leavePolicy = new LeavePolicy( newPolicy );

                var containsPolicy = SystemBroker.OrganizationHandler.IsSystemPolicyGroupHasPolicyForLeaveType( ( int )leavePolicy.LeaveType ).Result;
                if (! containsPolicy )
                {
                    // Create Policy itself
                    var PolicyResponse = TamamServiceBroker.OrganizationHandler.CreatePolicy( newPolicy, SystemRequestContext.Instance );
                    if ( PolicyResponse.Type == ResponseState.Success )
                    {
                        // Associate with the ( System Policy Group )..
                        var policyGroup = SystemBroker.OrganizationHandler.GetSystemPolicyGroup().Result;
                        var departmentsIDs = policyGroup.Departments.Select( x => x.Id ).ToList();
                        var policies = policyGroup.Policies;
                        policies.Add( newPolicy );
                        var PoliciesIDs = policies.Select( x => x.Id ).ToList();

                        var PG_Response = TamamServiceBroker.OrganizationHandler.EditPolicyGroup( policyGroup, departmentsIDs, new List<Guid>(), PoliciesIDs, SystemRequestContext.Instance );
                        if ( PG_Response.Type == ResponseState.Success )
                        {
                            ResponseHelper.HandleResponse( PolicyResponse, policy, CreateKey );
                            UpdateAsSynced( policy, PolicyResponse.Type );
                        }
                        else
                        {
                            ResponseHelper.HandleResponse( PG_Response, policy, CreateKey );
                        }
                    }
                    else
                    {
                        ResponseHelper.HandleResponse( PolicyResponse, policy, CreateKey );
                    }
                }
                else
                {
                    XLogger.Error( "Can not integrate the leave policy [{0}], because (System Policy Group) already associated with a leave policy with the same leave type [{1}].", leavePolicy.Name, leavePolicy.LeaveType.ToString() );
                }
            }
            catch ( Exception e )
            {
                XLogger.Error( LogHelper.BuildSkippedMessage( policy, e.Message ) );
            }
        }
        private void Edit( Policy tamamPolicy, RAWLeavePolicy policy )
        {
            try
            {
                string approvalPolicyId = string.Empty;
                var tmpPolicy = new Policy();
                tmpPolicy.Id = tamamPolicy.Id;
                var approvalPolicyField = tamamPolicy.Values.FirstOrDefault( x => x.PolicyFieldId == PolicyFields.LeavePolicy.ApprovalPolicy );
                if ( approvalPolicyField != null && approvalPolicyField.Value != null ) approvalPolicyId = approvalPolicyField.Value;

                var modifiedLeavePolicy = Map( tmpPolicy, policy, approvalPolicyId );
                var response = TamamServiceBroker.OrganizationHandler.EditPolicy( modifiedLeavePolicy, SystemRequestContext.Instance );
                ResponseHelper.HandleResponse( response, policy, EditKey );
                UpdateAsSynced( policy, response.Type );
            }
            catch ( Exception e )
            {
                XLogger.Error( LogHelper.BuildSkippedMessage( policy, e.Message ) );
            }
        }

        #endregion
        #region Helpers

        private void IntegrateAll()
        {
            var policies = LeavesPoliciesDataHandler.GetIntegrationLeavePolicies();
            Integrate( policies );
        }
        private void Integrate( List<RAWLeavePolicy> policies )
        {
            var codes = policies.Select( p => p.Code ).ToList();
            var tamamPolicies = GetTamamPolicies( codes );

            foreach ( RAWLeavePolicy RAWPolicy in policies )
            {
                var tamamPolicy = tamamPolicies.SingleOrDefault( l => l.Code == RAWPolicy.Code );
                if ( tamamPolicy == null )
                {
                    this.Create( RAWPolicy );
                }
                else
                {
                    this.Edit( tamamPolicy, RAWPolicy );
                }
            }
        }

        private List<Policy> GetTamamPolicies( List<string> codes )
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies( codes, SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success ) return null;
            return response.Result;
        }
        private void UpdateAsSynced( RAWLeavePolicy policy, ResponseState response )
        {
            if ( response == ResponseState.Success ) _leavesPoliciesDataHandler.UpdateAsSynced( policy );
        }

        #endregion
        # region Mappers

        private Policy Map( Policy tamamPolicy, RAWLeavePolicy policy, string approvalPolicyId )
        {
            tamamPolicy.Name = policy.Name;
            tamamPolicy.NameCultureVarient = policy.NameCultureVarient;
            tamamPolicy.Code = policy.Code;
            tamamPolicy.Active = true;
            tamamPolicy.PolicyTypeId = new Guid( PolicyTypes.LeavePolicyType );

            var policyValues = MapPolicyFields( policy );
            if ( string.IsNullOrWhiteSpace( approvalPolicyId ) )
                policyValues.Add( MapPolicyValues( "ApprovalPolicy", PolicyFields.LeavePolicy.ApprovalPolicy, IntegrationConstants.DefaultSysApprovalPolicy ) );
            else
                policyValues.Add( MapPolicyValues( "ApprovalPolicy", PolicyFields.LeavePolicy.ApprovalPolicy, approvalPolicyId ) );

            tamamPolicy.Values = new List<PolicyFieldValue>();
            tamamPolicy.Values = policyValues;

            return tamamPolicy;
        }
        private List<PolicyFieldValue> MapPolicyFields( RAWLeavePolicy policy )
        {
            var values = new List<PolicyFieldValue>();

            values.Add( MapPolicyValues( "Leave Type", PolicyFields.LeavePolicy.LeaveType, LeaveTypesRepository.Translate( policy.LeaveTypeCode ).ToString() ) );
            values.Add( MapPolicyValues( "Allowed Amount", PolicyFields.LeavePolicy.AllowedAmount, policy.AllowedAmount.ToString() ) );
            values.Add( MapPolicyValues( "Carry Over", PolicyFields.LeavePolicy.CarryOver, policy.CarryOver.ToString() ) );
            values.Add( MapPolicyValues( "Days Before Request", PolicyFields.LeavePolicy.DaysBeforeRequest, policy.DaysBeforeRequest.ToString() ) );
            values.Add( MapPolicyValues( "Allow Requests", PolicyFields.LeavePolicy.AllowRequests, policy.AllowRequests.ToString() ) );
            values.Add( MapPolicyValues( "Max Carry Over Days", PolicyFields.LeavePolicy.MaxCarryOverDays, policy.MaxCarryOverDays.ToString() ) );
            values.Add( MapPolicyValues( "Allow Half Days", PolicyFields.LeavePolicy.AllowHalfDays, policy.AllowHalfDays.ToString() ) );
            values.Add( MapPolicyValues( "Require Attachments", PolicyFields.LeavePolicy.RequireAttachments, policy.RequireAttachements.ToString() ) );
            values.Add( MapPolicyValues( "Include Week Ends And Holidays", PolicyFields.LeavePolicy.IncludeWeekEndsAndHolidays, policy.IncludeWeekEndsandHolidays.ToString() ) );
            values.Add( MapPolicyValues( "Days Limit For Old Leaves", PolicyFields.LeavePolicy.DaysLimitForOldLeaves, policy.DaysLimitForOldLeaves.ToString() ) );
            values.Add( MapPolicyValues( "Max Days Per Request", PolicyFields.LeavePolicy.MaxDaysPerRequest, policy.MaxDaysPerRequest.ToString() ) );
            values.Add( MapPolicyValues( "Essential Credit", PolicyFields.LeavePolicy.EssentialCredit, policy.EssentialCredit.ToString() ) );
            values.Add( MapPolicyValues( "Disable Planned Leaves", PolicyFields.LeavePolicy.DisablePlannedLeaves, policy.DisablePlannedLeaves.ToString() ) );
            values.Add( MapPolicyValues( "Unlimited Credit", PolicyFields.LeavePolicy.UnlimitedCredit, policy.UnlimitedCredit.ToString() ) );
            values.Add( MapPolicyValues( "Exceeds Progressive Credit", PolicyFields.LeavePolicy.ExceedsProgressiveCredit, policy.ExceedsProgressiveCredit.ToString() ) );

            return values;
        }
        private PolicyFieldValue MapPolicyValues( string name, Guid fieldId, string value )
        {
            return new PolicyFieldValue()
            {
                Name = name,
                PolicyFieldId = fieldId,
                Value = value
            };
        }

        # endregion
    }
}