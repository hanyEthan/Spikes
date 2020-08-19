using System.Threading.Tasks;

namespace XCore.Framework.Infrastructure.Config.Contracts
{
    public interface IConfigProvider<T>
    {
        bool Initialized { get; }
        Task<T> GetConfigAsync();
    }
    public interface IConfigProvider : IConfigProvider<IConfigData>
    {
    }
}
