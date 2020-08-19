using System.Collections.Generic;

namespace XCore.Framework.Framework.Unity.Contracts
{
    public interface IUnityConfigurationsBuilder
    {
        bool Initialized { get; }
        List<string> InitializaionMessages { get; }

        IUnityConfigurationsProvider ConfigurationsProvider { get; set; }
        Dictionary<string , IUnityCluster> Build();
    }
}
