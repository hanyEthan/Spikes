using System.Linq;
using System.Collections.Generic;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Policy;

namespace ADS.Tamam.Common.Data.Validation
{
    public class PolicyGroupRulesValidator
    {
        #region props ...

        protected static OrganizationDataHandler DataHandler = new OrganizationDataHandler();
        
        #endregion

        /// <summary>
        /// Validates that policy type uniqueness inside the policy group
        /// Also, check for other pre defined policy rules ...
        /// </summary>
        public static bool Validate( PolicyGroup policyGroup , List<Policy> policies )
        {
            foreach ( var policy in policies )
            {
                if ( !IsPolicyUnique( policyGroup , policy ) ) return false;
                if ( !IsRulesCompliant( policyGroup , policy ) ) return false;
                if ( !IsRulesCompliant( policies , policy ) ) return false;
            }

            return true;
        }

        #region Helpers ...

        private static bool IsPolicyUnique( PolicyGroup policyGroup , Policy policy )
        {
            if ( policy.PolicyType.SupportMultiAssociation ) return true;

            var policyWithTheSameType = policyGroup.Policies.Where( x => x.PolicyTypeId == policy.PolicyTypeId ).FirstOrDefault();
            return policyWithTheSameType == null ? true : false;
        }
        private static bool IsRulesCompliant( PolicyGroup policyGroup , Policy policy )
        {
            foreach ( var rule in policy.PolicyType.Rules )
            {
                // get affected value ...
                var value = policy.Values.Where( x => x.PolicyFieldId == rule.FieldId ).FirstOrDefault();
                if ( value == null ) continue;

                // check rule ...
                switch ( rule.Condition )
                {
                    case PolicyRule.PolicyRulesConditions.Exclusive:
                        {
                            bool valueExist = policyGroup.Policies.Any( x => x.PolicyTypeId == rule.PolicyTypeId && x.Values.Any( y => y.PolicyFieldId == rule.FieldId && y.Value == value.Value ) );
                            if ( valueExist ) return false;

                            break;
                        }
                }
            }

            return true;
        }
        private static bool IsRulesCompliant( List<Policy> policies , Policy policy )
        {
            foreach ( var rule in policy.PolicyType.Rules )
            {
                // get affected value ...
                var value = policy.Values.Where( x => x.PolicyFieldId == rule.FieldId ).FirstOrDefault();
                if ( value == null ) continue;

                // check rule ...
                switch ( rule.Condition )
                {
                    case PolicyRule.PolicyRulesConditions.Exclusive:
                        {
                            bool valueExist = policies.Any( x => x.Id != policy.Id && x.PolicyTypeId == rule.PolicyTypeId && x.Values.Any( y => y.PolicyFieldId == rule.FieldId && y.Value == value.Value ) );
                            if ( valueExist ) return false;

                            break;
                        }
                }
            }

            return true;
        }

        #endregion
    }
}
