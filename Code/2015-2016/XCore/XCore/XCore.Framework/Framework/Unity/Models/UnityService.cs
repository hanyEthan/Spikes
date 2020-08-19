using System.Collections.Generic;

namespace XCore.Framework.Framework.Unity.Models
{
    public class UnityService
    {
        #region props.

        public int Id { get; set; }

        public string Key { get; set; }
        public string ClusterKey { get; set; }

        public string ServiceContract { get; set; }
        public string ServiceImplementation { get; set; }

        public List<string> ClusterDependencies { get; set; }
        public List<UnityServiceParameter> Parameters { get; set; }

        public bool IsActive { get; set; }

        #endregion
        #region cst.

        public UnityService()
        {

        }

        #endregion
    }
}
