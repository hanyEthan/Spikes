using System;

using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Context;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class HRPolicy : AbstractSpecialPolicy
    {
        #region props.

        public Department HRDepartment { get; private set; }
        public DetailCode HRRole { get; private set; }
        public string CCs { get; private set; }

        #endregion
        #region cst.

        public HRPolicy( PolicyModel policy ) : base( policy )
        {
            // ...
            Guid? departmentId = base.GetGuid( PolicyFields.HRPolicy.HRDepartment );
            if ( departmentId.HasValue )
            {
                //var dataHandler = new OrganizationDataHandler();
                //HRDepartment = dataHandler.GetDepartment( departmentId.Value , SystemSecurityContext.Instance ).Result;
            }
            else
            {
                HRDepartment = null;
            }

            // ...
            int? roleId = base.GetInt( PolicyFields.HRPolicy.HRRole );
            if ( roleId.HasValue )
            {
                HRRole = Broker.Initialized ? Broker.DetailCodeHandler.GetDetailCode( roleId.Value ) : null;
            }
            else
            {
                HRRole = null;
            }

            // ...
            CCs = GetString( PolicyFields.HRPolicy.CCs );
        }

        #endregion

        #region statics

        public static HRPolicy GetInstance( PolicyModel policy )
        {
            try
            {
                // validate ...
                if ( policy == null || policy.PolicyTypeId.ToString() != PolicyTypes.HRPolicyType ) return null;

                // compose ...
                var HRPolicy = new HRPolicy( policy );

                return HRPolicy;
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