using ADS.Common.Bases.Events.Models;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADS.Tamam.Modules.Organization.Events
{
    [Serializable]
    public class NativeHolidayDeleteEvent : EventCell, IXIncludable
    {
        #region Properties

        public Holiday Holiday { get; set; }

        #endregion

        # region cst..

        public NativeHolidayDeleteEvent() : base()
        {

        }
        public NativeHolidayDeleteEvent( Holiday holiday ) : base()
        {
            this.Holiday = holiday;
        }

        # endregion
        #region EventCell

        [XDontSerialize]public override string ContentType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }
        [XDontSerialize]public override string TargetId
        {
            get { return ""; }
        }
        [XDontSerialize]public override string TargetType
        {
            get { return typeof( Holiday ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            # region Prep

            if ( !ValidateData() ) return false;

            # endregion
            # region Logic

            var allPeople = GetAllPeople();
            foreach ( var personId in allPeople )
            {
                // Leave
                var responseLeaveRecalculate = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( personId, SystemRequestContext.Instance );
                if ( responseLeaveRecalculate.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Leaves Credit
                }

                // Excuse
                var responseExcuseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( personId, SystemRequestContext.Instance );
                if ( responseExcuseRecalculate.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Leaves Credit
                }

                // Validate Work flow
                TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( personId, SystemRequestContext.Instance );

                // Native Holidays..
                TamamServiceBroker.AttendanceHandler.HandleNativeHolidays( personId, Holiday.StartDate, Holiday.EndDate, SystemRequestContext.Instance );
            }

            # endregion
            #region Cache

            Broker.Cache.Invalidate( TamamCacheClusters.Holidays );

            #endregion

            return true;
        }

        # region Helpers

        private bool ValidateData()
        {
            return Holiday != null;
        }
        private List<Guid> GetAllPeople()
        {
            var criteria = new PersonSearchCriteria { ActivationStatus = true };
            var people = TamamServiceBroker.PersonnelHandler.GetPersonnel( criteria, SystemRequestContext.Instance ).Result;
            var allPeople = people.Persons.Select( x => x.Id ).ToList();

            return allPeople;
        }

        # endregion
    }
}