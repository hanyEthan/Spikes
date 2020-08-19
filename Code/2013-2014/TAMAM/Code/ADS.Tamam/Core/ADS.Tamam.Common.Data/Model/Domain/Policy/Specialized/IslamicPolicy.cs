using System;

using ADS.Common.Utilities;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class IslamicPolicy : AbstractSpecialPolicy
    {
        #region props.

        public Shift RamadanShift { get; private set; }

        #endregion
        #region cst.

        public IslamicPolicy( PolicyModel policy ) : base( policy )
        {
            // ...
            Guid? shiftId = base.GetGuid( PolicyFields.IslamicPolicy.RamadanShift );
            if ( shiftId.HasValue )
            {
                //var dataHandler = new SchedulesDataHandler();
                //RamadanShift = dataHandler.GetShift( shiftId.Value ).Result;
            }
            else
            {
                RamadanShift = null;
            }
        }

        #endregion

        #region statics

        public static IslamicPolicy GetInstance( PolicyModel policy )
        {
            try
            {
                // validate ...
                if ( policy == null || policy.PolicyTypeId.ToString() != PolicyTypes.IslamicPolicyType ) return null;

                // compose ...
                return new IslamicPolicy( policy );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        #endregion
    }
}