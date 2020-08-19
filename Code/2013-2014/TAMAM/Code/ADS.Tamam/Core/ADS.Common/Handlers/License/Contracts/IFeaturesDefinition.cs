using ADS.Common.Contracts;

namespace ADS.Common.Handlers.License.Definition
{
    public interface IFeaturesDefinition : IBaseHandler
    {
        string Definition { get; }
    }
}
