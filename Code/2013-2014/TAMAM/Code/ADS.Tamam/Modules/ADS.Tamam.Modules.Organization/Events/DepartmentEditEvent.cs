using System;
using System.Linq;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Enums;
using System.Collections.Generic;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Organization.Events
{
    [Serializable]
    public class DepartmentEditEvent : EventCell, IXIncludable
    {
        #region Properties

        public Guid DepartmentId { get; set; }
        private OrganizationDataHandler dataHandler;

        #endregion

        # region cst..

        public DepartmentEditEvent() : base()
        {

        }
        public DepartmentEditEvent( Guid departmentId ) : this()
        {
            this.DepartmentId = departmentId;
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
            get { return typeof( Department ).ToString(); }
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

                # region Leaves Credit / Excuses Duration / Workflows / Holiday Policies

                // Update Leaves Credit
                var departments = dataHandler.GetDepartmentsByRoot( DepartmentId ).Result.Select( x => x.Id ).ToList();
                var people = SystemBroker.OrganizationHandler.GetDepartmentsPeople( departments , SystemRequestContext.Instance ).Result;
                foreach ( var id in people )
                {
                    var responseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( id , SystemRequestContext.Instance );
                    if ( responseRecalculate.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Leaves Credit
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( id , SystemRequestContext.Instance );
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id , SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays = personPoliciesResult.Result.Where( h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , holiday , SystemRequestContext.Instance );
                        }
                    }
                }

                # endregion
                #region Update Effective Schedules

                var RecalculateEffectiveSchedulesResponse = TamamServiceBroker.SchedulesHandler.ReCalculateEffectiveSchedulesForDepartment( DepartmentId );
                if ( RecalculateEffectiveSchedulesResponse.Type != ResponseState.Success ) return false;

                #endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Department );

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
            return DepartmentId != Guid.Empty;
        }
        private void PrepareDataLayer()
        {
            // TODO : should be done through the data broker ...
            dataHandler = new OrganizationDataHandler();
        }
        
        #endregion
    }
}