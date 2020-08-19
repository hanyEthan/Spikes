using System.Collections.Generic;
using XCore.Framework.Framework.Unity.Models;

namespace XCore.Framework.Framework.Unity.Contracts
{
    public interface IUnityConfigurationsProvider
    {
        bool Initialized { get; }
        List<string> InitializaionMessages { get; }

        UnityConfig Get();
    }
}
