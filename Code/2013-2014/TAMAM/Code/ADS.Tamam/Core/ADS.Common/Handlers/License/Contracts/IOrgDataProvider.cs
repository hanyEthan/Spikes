using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Context;

namespace ADS.Common.Handlers.License.Contracts
{
    public interface IOrgDataProvider
    {
        ExecutionResponse<object> getOrgInfo();
    }
}
