using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class ShiftEditEvent : EventCell, IXIncludable
    {
        #region Properties

        public Guid ShiftId { get; set; }

        private SchedulesDataHandler dataHandler;

        #endregion

        # region cst..

        public ShiftEditEvent() : base()
        {

        }
        public ShiftEditEvent( Guid shiftId ) : this()
        {
            this.ShiftId = shiftId;
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
            get { return typeof( Shift ).ToString(); }
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
                #region Logic

                var templateDays = dataHandler.GetTemplateDaysByShiftId( ShiftId ).Result;
                var templates = templateDays.Select( x => x.Template ).Distinct().ToList();
                foreach ( var template in templates )
                {
                    // get all Schedule associated People
                    var schTemplate = dataHandler.GetScheduleTemplate( template.Id ).Result;
                    var allPeople = new List<Guid>();
                    foreach ( var schedule in schTemplate.Schedules )
                    {
                        allPeople.AddRange( schedule.EffectiveSchedulePersonnel.Select( sp => sp.PersonId ).ToList() );
                    }
                    allPeople = allPeople.Distinct().ToList();

                    // Recalculate Leaves Credit & Excuses Duration
                    foreach ( var id in allPeople )
                    {
                        var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( id, SystemRequestContext.Instance );
                        if ( response.Type != ResponseState.Success ) return false;

                        // excuses
                        var response_excuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( id, SystemRequestContext.Instance );
                        if ( response_excuses.Type != ResponseState.Success )
                        {
                            // TODO: Cannot Recalculate Excuses
                        }
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
            return ShiftId != Guid.Empty;
        }
        private void PrepareDataLayer()
        {
            // TODO : should be done through the data broker ...
            dataHandler = new SchedulesDataHandler();
        }

        #endregion
    }
}