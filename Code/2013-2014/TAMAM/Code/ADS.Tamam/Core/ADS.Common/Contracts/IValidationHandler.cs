
namespace ADS.Common.Contracts
{
    public interface IValidationHandler
    {
        bool IsValid( IValidationEnabledEntity entity );
    }
}
