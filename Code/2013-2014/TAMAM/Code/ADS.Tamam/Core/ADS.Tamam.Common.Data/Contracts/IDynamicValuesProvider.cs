using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using ADS.Tamam.Common.Data.Model.Domain.Policy;

namespace ADS.Tamam.Common.Data.Contracts
{
    public  interface IDynamicValuesProvider
    {
        IList<PolicyFieldValue> Values { get;  }
    }
}
