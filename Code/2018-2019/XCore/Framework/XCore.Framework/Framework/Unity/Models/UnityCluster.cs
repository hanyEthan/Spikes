using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;

namespace XCore.Framework.Framework.Unity.Models
{
    public class UnityCluster : IUnityCluster
    {
        #region props.

        public virtual bool Initialized { get; private set; }

        public virtual string Name { get; set; }
        public virtual Dictionary<string , IUnityService> Services { get; set; }

        #endregion
        #region cst.

        public UnityCluster()
        {
            this.Services = new Dictionary<string , IUnityService>();
        }

        #endregion
    }
}
