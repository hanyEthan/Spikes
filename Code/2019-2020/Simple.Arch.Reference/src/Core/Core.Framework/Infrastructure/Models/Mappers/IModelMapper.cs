namespace Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers
{
    public interface IModelMapper<TSource, TDestination>
    {
        TDestination Map(TSource from, object metadata = null);
        TDestinationAlt Map<TDestinationAlt>(TSource from, object metadata = null) where TDestinationAlt : TDestination;
    }
}
