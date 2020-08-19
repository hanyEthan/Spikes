using System;
using System.Collections.Generic;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Personnel.Events
{
    [Serializable]
    public class PersonDeactivateEvent : EventCell, IXIncludable
    {
        #region Properties

        public Guid PersonId { get; set; }

        #endregion

        # region cst..

        public PersonDeactivateEvent() : base()
        {

        }
        public PersonDeactivateEvent( Guid personId ) : this()
        {
            this.PersonId = personId;
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
            get { return typeof( Person ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            try
            {
                # region Prep

                if ( PersonId == Guid.Empty ) return false;

                # endregion

                # region Effective Schedule

                var EffectiveScheduleResponse = TamamServiceBroker.SchedulesHandler.ReCalculateEffectiveSchedulesForPerson( PersonId );
                if ( EffectiveScheduleResponse.Type != ResponseState.Success ) return false;

                # endregion
                # region Workflows
                
                TamamServiceBroker.LeavesHandler.ApprovalInstancesCancel( PersonId , SystemRequestContext.Instance );

                # endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
    }
}