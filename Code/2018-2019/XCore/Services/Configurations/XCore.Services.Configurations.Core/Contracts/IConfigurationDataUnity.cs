using System.Threading.Tasks;

namespace XCore.Services.Configurations.Core.DataLayer.Contracts
{
    public interface IConfigurationDataUnity
    {
        bool? Initialized { get; }


        IModulesRepository Modules { get; }
        IConfigsRepository Configs { get; }

        IAppsRepository Apps { get; }
        Task SaveAsync();
    }
}