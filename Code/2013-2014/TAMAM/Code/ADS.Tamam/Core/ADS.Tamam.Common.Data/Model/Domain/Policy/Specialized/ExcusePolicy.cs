using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Common.Data.Model.Enums;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class ExcusePolicy : AbstractSpecialPolicy, IApprovalPolicySource
    {
        # region props ...

        public int? MaxExcusesPerDay
        {
            get;
            private set;
        }
        public int? MaxExcusesPerMonth
        {
            get;
            private set;
        }
        public double? AllowedHoursPerDay
        {
            get;
            private set;
        }
        public double? AllowedHoursPerMonth
        {
            get;
            private set;
        }
        public double? MinExcuseDuration { get; private set; }
        public bool HasCredit { get { return ExcuseType != ExcuseTypes.AwayExcuse; } }
        public ExcuseTypes ExcuseType
        {
            get;
            private set;
        }
        public ApprovalPolicy ApprovalPolicy { get; private set; }

        # endregion
        # region cst ...

        public ExcusePolicy(PolicyModel policy)
            : base(policy)
        {
            MaxExcusesPerDay = GetInt(PolicyFields.ExcusePolicy.MaxExcusesPerDay);
            MaxExcusesPerMonth = GetInt(PolicyFields.ExcusePolicy.MaxExcusesPerMonth);
            AllowedHoursPerDay = GetDouble(PolicyFields.ExcusePolicy.AllowedHoursPerDay);
            AllowedHoursPerMonth = GetDouble(PolicyFields.ExcusePolicy.AllowedHoursPerMonth);
            MinExcuseDuration = GetDouble(PolicyFields.ExcusePolicy.MinExcuseDuration);
            ExcuseType = (ExcuseTypes)GetInt(PolicyFields.ExcusePolicy.ExcuseType);
            InitializeApprovalPolicy();
            // InitializeExcuseType();
        }

        # endregion
        # region Helpers

        private void InitializeApprovalPolicy()
        {
            Guid? approvalPolicyId = GetGuid(PolicyFields.ExcusePolicy.ApprovalPolicy);
            if (approvalPolicyId.HasValue)
            {
                ApprovalPolicy = GetApprovlPolicy(approvalPolicyId.Value);
            }
            else
            {
                ApprovalPolicy = null;
            }
        }

        //private void InitializeExcuseType()
        //{
        //    Guid? excuseTypeId = GetGuid(PolicyFields.ExcusePolicy.ExcuseType);
        //    if (excuseTypeId.HasValue)
        //    {
        //        ExcuseType = GetExcuseType(excuseTypeId.Value);
        //    }
        //    else
        //    {
        //        ExcuseType = null;
        //    }
        //}

        private ApprovalPolicy GetApprovlPolicy(Guid id)
        {
            //var dataHandler = new OrganizationDataHandler();
            //var response = dataHandler.GetPolicy(id);
            //if (response.Type != ResponseState.Success) return null;
            //if (response.Result.PolicyTypeId != new Guid(PolicyTypes.ApprovalPolicyType)) return null;

            //return new ApprovalPolicy(response.Result);
            return null;
        }

        //private DetailCode GetExcuseType(Guid id)
        //{
        //    var dataHandler = new Handlers();
        //    var response = dataHandler.GetExcuseType(id);
        //    if (response.Type != ResponseState.Success) return null;

        //    return response.Result;
        //}

        # endregion
        #region statics

        public static List<ExcusePolicy> GetInstances( List<PolicyModel> policies )
        {
            var list = new List<ExcusePolicy>();

            foreach ( var policy in policies )
            {
                list.Add( new ExcusePolicy( policy ) );
            }

            return list;
        }
        public static List<ExcusePolicy> GetInstances( List<PolicyModel> policies , int excuseType )
        {
            var nativePolicies = policies.Where( p => p.Values.Any( pv => pv.PolicyFieldId == PolicyFields.ExcusePolicy.ExcuseType && pv.Value == excuseType.ToString() ) ).ToList();
            return GetInstances( nativePolicies );
        }

        public static ExcusePolicy GetExcusePolicy(List<PolicyModel> policies, ExcuseTypes excuseType)
        {
            var excuseTypeAsString = ((int)excuseType).ToString();
            var nativePolicy = policies.FirstOrDefault(p => p.Values.Any(pv => pv.PolicyFieldId == PolicyFields.ExcusePolicy.ExcuseType && pv.Value == excuseTypeAsString));

            return nativePolicy == null ? null : new ExcusePolicy(nativePolicy);
        }
        public static ExcusePolicy GetNormalPolicy(List<PolicyModel> policies)
        {
            return GetExcusePolicy(policies, ExcuseTypes.NormalExcuse);
        }
        public static ExcusePolicy GetAwayPolicy(List<PolicyModel> policies)
        {
            return GetExcusePolicy(policies, ExcuseTypes.AwayExcuse);
        }

        # endregion
    }
}
