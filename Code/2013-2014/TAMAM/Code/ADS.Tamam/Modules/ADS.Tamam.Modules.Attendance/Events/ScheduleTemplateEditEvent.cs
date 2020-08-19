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
    public class ScheduleTemplateEditEvent : EventCell, IXIncludable
    {
        #region Properties

        public Guid ScheduleTemplateId { get; set; }
        private SchedulesDataHandler dataHandler;

        #endregion
        # region cst..

        public ScheduleTemplateEditEvent() : base()
        {

        }
        public ScheduleTemplateEditEvent( Guid scheduleTemplateId ) : this()
        {
            this.ScheduleTemplateId = scheduleTemplateId;
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
            get { return typeof( ScheduleTemplate ).ToString(); }
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

                // get all Schedule associated People
                var template = dataHandler.GetScheduleTemplate( ScheduleTemplateId ).Result;
                var allPeople = new List<Guid>();
                foreach ( var schedule in template.Schedules )
                {
                    allPeople.AddRange( schedule.SchedulePersonnel.Select( sp => sp.PersonId ).ToList() );
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
            return ScheduleTemplateId != Guid.Empty;
        }
        private void PrepareDataLayer()
        {
            // TODO : should be done through the data broker ...
            dataHandler = new SchedulesDataHandler();
        }

        #endregion
    }
}