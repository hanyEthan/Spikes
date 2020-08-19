using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class ScheduleChangeStatusEvent : EventCell, IXIncludable
    {
        #region Properties

        public Guid ScheduleId { get; set; }
        public List<Department> OldScheduleDepartments { get; set; }
        public List<Person> OldSchedulePersonnel { get; set; }

        private SchedulesDataHandler dataHandler;

        #endregion
        # region cst..

        public ScheduleChangeStatusEvent() : base() 
        {
            if ( OldScheduleDepartments == null ) OldScheduleDepartments = new List<Department>();
            if ( OldSchedulePersonnel == null ) OldSchedulePersonnel = new List<Person>();
        }
        public ScheduleChangeStatusEvent( Guid scheduleId, List<Department> oldScheduleDepartments, List<Person> oldSchedulePersonnel )
            : this()
        {
            this.ScheduleId = scheduleId;
            this.OldScheduleDepartments = oldScheduleDepartments;
            this.OldSchedulePersonnel = oldSchedulePersonnel;
        }

        # endregion
        #region EventCell

        [XDontSerialize]
        public override string ContentType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }
        [XDontSerialize]
        public override string TargetId
        {
            get { return ""; }
        }
        [XDontSerialize]
        public override string TargetType
        {
            get { return typeof( Schedule ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            try
            {
                //return true;

                # region Prep

                if ( !ValidateData() ) return false;
                PrepareDataLayer();

                # endregion

                #region Update Effective Schedule Departments ...

                var ScheduleDepartmentsResponse = dataHandler.GetScheduleDepartments_ScheduleId( ScheduleId, SystemSecurityContext.Instance );
                if ( ScheduleDepartmentsResponse.Type != ResponseState.Success )
                {
                    //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) , schedule.Id.ToString() );
                    return false;
                }
                var ScheduleDepartments = ScheduleDepartmentsResponse.Result.Select( x => x.Department );

                var allDepartments = OldScheduleDepartments.Union( ScheduleDepartments ).Distinct( new DepartmentComparer() ).ToList();

                foreach ( var d1 in allDepartments )
                {
                    // if d1 is at the highest level
                    // if d1 is not a child to any other existing department
                    if ( allDepartments.Where( d2 => d2.Id != d1.Id ).All( d2 => !d1.Hashcode.Contains( d2.Hashcode ) ) )
                    {
                        TamamServiceBroker.SchedulesHandler.ReCalculateEffectiveSchedulesForDepartment( d1 );
                    }
                }

                #endregion
                #region  Update Leaves Credit & Excuses Duration & Person Effective Schedule ...

                foreach ( var person in OldSchedulePersonnel )
                {
                    var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( person.Id, SystemRequestContext.Instance );
                    if ( response.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Leaves Credit
                    }

                    // excuses
                    var response_excuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( person.Id, SystemRequestContext.Instance );
                    if ( response_excuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Person Effectipe Schedule 
                    var updatePersonScheduleResponse = TamamServiceBroker.SchedulesHandler.ReCalculateEffectiveSchedulesForPerson( person );
                    if ( updatePersonScheduleResponse.Type != ResponseState.Success )
                    {
                        //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) , schedule.Id.ToString() );
                        return false;
                    }
                }

                #endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

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
            return ScheduleId != Guid.Empty;
        }
        private void PrepareDataLayer()
        {
            // TODO : should be done through the data broker ...
            dataHandler = new SchedulesDataHandler();
        }

        #endregion

        # region inner

        class DepartmentComparer : IEqualityComparer<Department> 
        {
            public bool Equals( Department x, Department y )
            {
                return x.Id == y.Id;
            }

            public int GetHashCode( Department obj )
            {
                return obj.GetHashCode();
            }
        }

        # endregion
    }
}