using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Enums
{
    public enum LeaveStatus
    {
        Planned = 449,
        Pending = 450,
        Approved = 451,
        Denied = 452,
        Cancelled = 453,
        Taken = 454
    }
    public enum WorkFlowStepStatus
    {
        Pending = 1,
        Approved = 2,
        Denied = 3,
        Cancelled = 4
    }

    public enum LeaveCreditStatus
    {
        Current = 560,
        Previous = 561,
        Next = 562
    }
}
