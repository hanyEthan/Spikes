using System;
using System.Linq;
using System.Text;

using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Modules.Integration.DataHandlers;
using ADS.Tamam.Modules.Integration.Helpers;
using ADS.Tamam.Modules.Integration.Repositories;

using RAWDetailCode = ADS.Tamam.Modules.Integration.Models.IDetailCodeSimilar;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class LeaveTypesIntegrator : DetailCodeIntegrator
    {
        #region Ctor

        public LeaveTypesIntegrator( IDetailCodeDataHandler detailCodesDataHandler, DetailCodeRepository detailCodesRepository ) : base( detailCodesDataHandler, detailCodesRepository ) { }

        #endregion

        public override ValidationResponse IsValidForEdit( DetailCode DetailCode, RAWDetailCode RAWDetailCode )
        {
            if ( !IsDeactivating( DetailCode, RAWDetailCode ) ) return new ValidationResponse( true, string.Empty );

            var nativeLeaveTypePolicies = TamamServiceBroker.OrganizationHandler.GetPolicies( new PolicyFilters( new Guid( PolicyTypes.LeavePolicyType ) ), SystemRequestContext.Instance ).Result;
            var specializedPolicies = nativeLeaveTypePolicies.Select( x => new LeavePolicy( x ) ).ToList();
            var query = specializedPolicies.Where( P => P.LeaveType.HasValue && ( int )P.LeaveType.Value == DetailCode.Id ).ToList();
            if ( query.Count == 0 ) return new ValidationResponse( true, string.Empty );

            var errors = new StringBuilder();
            errors.Append( "Can not deactivate this leave type, because it used with the following leave policies (" );
            errors.Append( String.Join( ", ", query.Select( p => p.Name ) ) );
            errors.Append( ")" );

            return new ValidationResponse( false, errors.ToString() );
        }

        private bool IsDeactivating( DetailCode DetailCode, RAWDetailCode RAWDetailCode )
        {
            if ( DetailCode.IsDeleted == false && RAWDetailCode.Activated == false ) return true;

            return false;
        }
    }
}