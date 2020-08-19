using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Common.Data.Model.Enums;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Common.Context;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class LeavePolicy : AbstractSpecialPolicy, IApprovalPolicySource
    {
        #region props ...

        public int? AllowedAmount { get; private set; }
        public bool? AllowRequests { get; private set; }
        public int? DaysBeforeRequest { get; private set; }
        public bool? SupportCarryOver { get; private set; }
        public int? MaxCarryOverDays { get; private set; }
        public ApprovalPolicy ApprovalPolicy { get; private set; }
        public LeaveTypes? LeaveType { get; private set; }
        public bool? AllowHalfDays { get; private set; }
        public bool? RequireAttachments { get; private set; }
        public bool? IncludeWeekEndsAndHolidays { get; private set; }
        public int? DaysLimitForOldLeaves { get; private set; }
        public int? MaxDaysPerRequest { get; private set; }
        public bool? EssentialCredit { get; private set; }
        public bool? DisablePlannedLeaves { get; private set; }
        public bool? UnlimitedCredit { get; private set; }
        public bool ExceedsProgressiveCredit { get; private set; }

        #endregion
        #region cst ...

        public LeavePolicy( PolicyModel policy )
            : base( policy )
        {
            int? leaveType = GetInt( PolicyFields.LeavePolicy.LeaveType );
            this.LeaveType = leaveType.HasValue ? ( LeaveTypes? )leaveType.Value : null;
            this.AllowedAmount = GetInt( PolicyFields.LeavePolicy.AllowedAmount );
            this.AllowRequests = GetBool( PolicyFields.LeavePolicy.AllowRequests );
            this.DaysBeforeRequest = GetInt( PolicyFields.LeavePolicy.DaysBeforeRequest );
            this.SupportCarryOver = GetBool( PolicyFields.LeavePolicy.CarryOver );
            this.MaxCarryOverDays = GetInt( PolicyFields.LeavePolicy.MaxCarryOverDays );
            this.AllowHalfDays = GetBool( PolicyFields.LeavePolicy.AllowHalfDays );
            this.RequireAttachments = GetBool( PolicyFields.LeavePolicy.RequireAttachments );
            this.IncludeWeekEndsAndHolidays = GetBool( PolicyFields.LeavePolicy.IncludeWeekEndsAndHolidays );
            this.DaysLimitForOldLeaves = GetInt( PolicyFields.LeavePolicy.DaysLimitForOldLeaves );
            this.MaxDaysPerRequest = GetInt( PolicyFields.LeavePolicy.MaxDaysPerRequest );
            this.EssentialCredit = GetBool( PolicyFields.LeavePolicy.EssentialCredit );
            this.DisablePlannedLeaves = GetBool( PolicyFields.LeavePolicy.DisablePlannedLeaves );
            this.UnlimitedCredit = GetBool( PolicyFields.LeavePolicy.UnlimitedCredit );
            this.ExceedsProgressiveCredit = GetBool( PolicyFields.LeavePolicy.ExceedsProgressiveCredit ) ?? true;

            InitializeApprovalPolicy();
        }

        #endregion
        #region Helpers

        private void InitializeApprovalPolicy()
        {
            Guid? approvalPolicyId = GetGuid( PolicyFields.LeavePolicy.ApprovalPolicy );
            if ( approvalPolicyId.HasValue )
            {
                ApprovalPolicy = GetApprovlPolicy( approvalPolicyId.Value );
            }
            else
                ApprovalPolicy = null;
        }
        private ApprovalPolicy GetApprovlPolicy( Guid id )
        {
            //var dataHandler = new OrganizationDataHandler();
            //var response = dataHandler.GetPolicy( id );
            //if ( response.Type != ResponseState.Success ) return null;
            //if ( response.Result.PolicyTypeId != new Guid( PolicyTypes.ApprovalPolicyType ) ) return null;

            //return new ApprovalPolicy( response.Result );
            return null;
        }

        #endregion

        #region statics

        public static List<LeavePolicy> GetInstances( List<PolicyModel> policies )
        {
            var list = new List<LeavePolicy>();

            foreach ( var policy in policies )
            {
                list.Add( new LeavePolicy( policy ) );
            }

            return list;
        }
        public static List<LeavePolicy> GetInstances( List<PolicyModel> policies, int leaveType )
        {
            var nativePolicies = policies.Where( x => x.Values.FirstOrDefault( y => y.PolicyFieldId == PolicyFields.LeavePolicy.LeaveType && y.Value == leaveType.ToString() ) != null ).ToList();
            return GetInstances( nativePolicies );
        }

        #endregion
    }
}
