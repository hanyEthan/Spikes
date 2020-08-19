using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Application.Services.Modules.Queries.AnyModules
{
    public class AnyModulesQueryMapper : IModelMapper<AnyModulesQuery, ModulesSearchCriteria>
    {
        #region IModelMapper

        public ModulesSearchCriteria Map(AnyModulesQuery from, object metadata = null)
        {
            return from;
        }
        public TDestinationAlt Map<TDestinationAlt>(AnyModulesQuery from, object metadata = null) where TDestinationAlt : ModulesSearchCriteria
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
