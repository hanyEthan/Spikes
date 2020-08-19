using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class PolicyFilters : IXSerializable
    {
        #region props.

        public Guid PolicyTypeId { get; set; }
        public bool? Active { get; set; }
        
        #endregion
        #region cst.

        public PolicyFilters()
        {
        }
        public PolicyFilters( Guid policyTypeId )
        {
            PolicyTypeId = policyTypeId;
        }
        public PolicyFilters( Guid policyTypeId , bool? active ) : this( policyTypeId )
        {
            Active = active;
        }

        #endregion
        #region publics.

        public override string ToString()
        {
            return string.Format( "{0}_{1}" , PolicyTypeId , Active );
        }

        #endregion
    }
}
