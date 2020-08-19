using ADS.Tamam.Common.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Services.DataAcquisition.Common
{
    public class Context
    {
        public static RequestContext RequestContext
        {
            get
            {
                return SystemRequestContext.Instance;
            }
        }
    }
}
