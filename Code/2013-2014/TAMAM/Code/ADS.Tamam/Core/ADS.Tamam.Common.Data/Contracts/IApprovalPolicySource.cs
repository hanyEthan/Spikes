using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IApprovalPolicySource
    {
        ApprovalPolicy ApprovalPolicy { get; }
    }
}
