using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Organization.Events
{
    [Serializable]
    public class PolicyGroupEvent : EventCell, IXIncludable
    {
        #region Properties

        public List<Guid> Departments { get; set; }
        public List<Guid> Personnel { get; set; }

        #endregion

        # region cst..

        public PolicyGroupEvent() : base()
        {

        }
        public PolicyGroupEvent( List<Guid> departments, List<Guid> personnel ) : this()
        {
            this.Departments = departments;
            this.Personnel = personnel;
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
            get { return typeof( PolicyGroup ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            try
            {
                # region Prep

                var ids = new List<Guid>();

                var DepartmentsPersonnel = GetDepartmentsPersonnel( Departments );
                //var departmentsPersonnel = SystemBroker.OrganizationHandler.GetDepartmentsPeople( Departments , SystemRequestContext.Instance ).Result;

                ids.AddRange( Personnel ?? new List<Guid>() );
                ids.AddRange( DepartmentsPersonnel );
                ids = ids.Distinct().ToList();

                # endregion
                #region Logic

                foreach ( var id in ids )
                {
                    // Update Leaves Credit
                    var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( id, SystemRequestContext.Instance );
                    if ( response.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Leaves Credit
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( id, SystemRequestContext.Instance );
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id, SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id, SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays = personPoliciesResult.Result.Where( h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id, holiday, SystemRequestContext.Instance );
                        }
                    }
                }

                #endregion
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

        # region Helpers

        private List<Guid> GetDepartmentsPersonnel(List<Guid> departments)
        {
            var Ps = new List<Guid>();

            if ( departments != null && departments.Count > 0 )
            {
                var Department_Ps = SystemBroker.OrganizationHandler.GetDepartmentsPeople( Departments, SystemRequestContext.Instance ).Result;
                if ( Department_Ps != null ) Ps.AddRange( Department_Ps );
            }

            return Ps;
        }

        # endregion
    }
}