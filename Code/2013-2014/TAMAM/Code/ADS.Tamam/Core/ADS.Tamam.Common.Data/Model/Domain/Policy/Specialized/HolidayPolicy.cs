using System;
using System.Collections.Generic;
using System.Globalization;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Model.Enums;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class HolidayPolicy : AbstractSpecialPolicy
    {
        #region props ...

        public DateTime? DateFrom
        {
            get;
            private set;
        }
        public DateTime? DateTo
        {
            get;
            private set;
        }

        #endregion
        #region cst ...

        public HolidayPolicy( PolicyModel policy )
            : base ( policy )
        {
            DateFrom = GetDate ( PolicyFields.HolidayPolicy.DateFrom );
            DateTo = GetDate ( PolicyFields.HolidayPolicy.DateTo );
        }

        #endregion

        #region statics

        public static HolidayPolicy GetInstance( PolicyModel policy )
        {
            try
            {
                // validate ...
                if ( policy == null || policy.PolicyTypeId.ToString () != PolicyTypes.HolidayPolicyType ) return null;

                // compose ...
                var holidayPolicy = new HolidayPolicy ( policy );

                return holidayPolicy;
            }
            catch ( Exception x )
            {
                XLogger.Error ( "Exception : " + x );
                return null;
            }
        }
        public static List<HolidayPolicy> GetInstances( List<PolicyModel> policies )
        {
            var list = new List<HolidayPolicy> ();

            foreach ( var policy in policies )
            {
                list.Add ( new HolidayPolicy ( policy ) );
            }

            return list;
        }

        #endregion
    }
}
