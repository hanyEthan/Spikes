using System.Collections.Generic;

namespace XCore.Framework.Framework.Unity.Models
{
    public class UnityConfig
    {
        #region props.

        public string Raw { get; set; }
        public IList<UnityService> ConfiguredServices { get; set; }

        #endregion
        #region cst.

        public UnityConfig()
        {
            this.ConfiguredServices = new List<UnityService>();
        }

        #endregion
    }
}
