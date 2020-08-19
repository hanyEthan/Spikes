using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Context;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Organization.Events
{
    [Serializable]
    public class PolicyEditEvent : EventCell, IXIncludable
    {
        #region Properties

        public Policy Policy { get; set; }
        private OrganizationDataHandler dataHandler;

        #endregion

        # region cst..

        public PolicyEditEvent() : base()
        {

        }
        public PolicyEditEvent( Policy policy ) : this()
        {
            this.Policy = policy;
        }

        # endregion
        #region EventCell

        [XDontSerialize] public override string ContentType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }
        [XDontSerialize] public override string TargetId
        {
            get { return ""; }
        }
        [XDontSerialize] public override string TargetType
        {
            get { return typeof( Policy ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            try
            {
                # region Prep

                if ( !ValidateData() ) return false;
                PrepareDataLayer();

                # endregion

                #region Leave Credits ...

                // Update Leave Credits if LeaveType is (Leaves Policies or Holidays)
                if ( Policy.PolicyTypeId == new Guid( PolicyTypes.LeavePolicyType ) ||
                     Policy.PolicyTypeId == new Guid( PolicyTypes.HolidayPolicyType ) ||
                     Policy.PolicyTypeId == new Guid( PolicyTypes.HRPolicyType ) ||
                     Policy.PolicyTypeId == new Guid( PolicyTypes.ApprovalPolicyType ) ||
                     Policy.PolicyTypeId == new Guid( PolicyTypes.AccrualPolicyType ) ||
                     Policy.PolicyTypeId == new Guid( PolicyTypes.ExcusesPolicyType ) )
                {
                    var groups = GetGroupsByPolicy( Policy );
                    var allPeople = GetGroupPeople( groups );
                    foreach ( var id in allPeople )
                    {
                        var responseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( id , SystemRequestContext.Instance );
                        if ( responseRecalculate.Type != ResponseState.Success )
                        {
                            // TODO: Cannot Recalculate Leaves Credit
                        }

                        // Validate Work flow
                        TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );
                        if ( Policy.PolicyTypeId == new Guid( PolicyTypes.HolidayPolicyType ) )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , Policy , SystemRequestContext.Instance );
                        }
                    }
                }

                #endregion
                # region Excuses Duration

                // Update Excuses Duration if PolicyType is (Holidays)
                if ( Policy.PolicyTypeId == new Guid( PolicyTypes.HolidayPolicyType ) )
                {
                    var groups = GetGroupsByPolicy( Policy );
                    var allPeople = GetGroupPeople( groups );
                    foreach ( var id in allPeople )
                    {
                        var responseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( id , SystemRequestContext.Instance );
                        if ( responseRecalculate.Type != ResponseState.Success )
                        {
                            // TODO: Cannot Recalculate Leaves Credit
                        }
                    }
                }

                # endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #region Helpers

        private bool ValidateData()
        {
            return Policy != null;
        }
        private void PrepareDataLayer()
        {
            // TODO : should be done through the data broker ...
            dataHandler = new OrganizationDataHandler();
        }
        
        private List<PolicyGroup> GetGroupsByPolicy( Policy policy )
        {
            var groups = new List<PolicyGroup>();
            if ( policy.PolicyTypeId == new Guid( PolicyTypes.ApprovalPolicyType ) )
            {
                var leavePolicies = dataHandler.GetPolicies( Guid.Parse( PolicyTypes.LeavePolicyType ) )
                    .Result.Select( leave => new LeavePolicy( leave ) )
                    .Where( l => l.ApprovalPolicy.Policy.Id == policy.Id ).ToList();

                foreach ( var item in leavePolicies )
                {
                    var tempGroups = dataHandler.GetGroupsAssociatedWithPolicy( item.Policy.Id ).Result;
                    foreach ( var g in tempGroups )
                    {
                        if ( groups.All( gr => gr.Id != g.Id ) )
                            groups.Add( g );
                    }
                }
            }
            else
            {
                // get policygroups that uses this policy
                groups = dataHandler.GetGroupsAssociatedWithPolicy( policy.Id ).Result;
            }
            return groups;
        }
        private List<Guid> GetGroupPeople( List<PolicyGroup> groups )
        {
            var allPeople = new List<Guid>();

            foreach ( var policyGroup in groups )
            {
                // get policygroup people
                var policyGroupPeople = TamamServiceBroker.OrganizationHandler.GetPolicyGroupPersons( policyGroup.Id , SystemRequestContext.Instance ).Result.Select( x => x.Id ).ToList();
                allPeople.AddRange( policyGroupPeople );

                // get policygroup departments people
                var policyGroupDepartments = TamamServiceBroker.OrganizationHandler.GetPolicyGroupDepartments( policyGroup.Id , SystemRequestContext.Instance ).Result.Select( x => x.Id ).ToList();
                var departmentPeople = SystemBroker.OrganizationHandler.GetDepartmentsPeople( policyGroupDepartments , SystemRequestContext.Instance ).Result;
                allPeople.AddRange( departmentPeople );
            }
            allPeople = allPeople.Distinct().ToList();
            return allPeople;
        }

        #endregion
    }
}