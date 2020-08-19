using System.Collections.Generic;

namespace XCore.Framework.Framework.Unity.Contracts
{
    public interface IUnityCluster
    {
        bool Initialized { get; }
        string Name { get; set; }

        Dictionary<string , IUnityService> Services { get; set; }
    }
}
