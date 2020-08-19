using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Handlers;

namespace XCore.Services.Security.Core.Unity
{
    public static class SecurityUnity
    {
        #region props.

        public static ISecurityHandler Security { get; set; }

        #endregion
        #region cst.

        static SecurityUnity()
        {
        }

      

        #endregion
    }
}
