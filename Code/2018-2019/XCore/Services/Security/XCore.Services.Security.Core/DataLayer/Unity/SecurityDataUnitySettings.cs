using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.DataLayer.Contracts;

namespace XCore.Services.Security.Core.DataLayer.Unity
{
   public class SecurityDataUnitySettings : ISecurityDataUnitySettings
    {
        #region IConfigDataUnitySettings

        public virtual string DBConnectionName { get; set; } = "XCore.Security";

        #endregion
    }
}
