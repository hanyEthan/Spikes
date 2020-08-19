using System;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Context;
using System.Collections.Generic;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Organization.Events
{
    [Serializable]
    public class DepartmentCreateEvent : EventCell, IXIncludable
    {
        #region Properties

        public Guid DepartmentId { get; set; }

        #endregion

        # region cst..

        public DepartmentCreateEvent() : base()
        {

        }
        public DepartmentCreateEvent( Guid departmentId ) : this()
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

                # endregion
                #region Logic

                var response = TamamServiceBroker.SchedulesHandler.ReCalculateEffectiveSchedulesForDepartment( DepartmentId );
                if ( response.Type != ResponseState.Success ) return false; 

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
        
        #endregion
    }
}