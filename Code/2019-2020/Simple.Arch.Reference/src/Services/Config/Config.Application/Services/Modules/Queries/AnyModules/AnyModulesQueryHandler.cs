using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Application.Services.Modules.Queries.AnyModules
{
    public class AnyModulesQueryHandler : MediatR.IRequestHandler<AnyModulesQuery, bool>
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly IConfigurationDataUnity _dataHandler;
        private readonly IModelMapper<AnyModulesQuery, ModulesSearchCriteria> _modelMapper;

        #endregion
        #region cst.

        public AnyModulesQueryHandler(IConfigurationDataUnity dataHandler,
                                          IModelMapper<AnyModulesQuery, ModulesSearchCriteria> modelMapper)
        {
            this._dataHandler = dataHandler;
            this._modelMapper = modelMapper;

            this.Initialized = Initialize();
        }

        #endregion
        #region MediatR.IRequestHandler

        public async Task<bool> Handle(AnyModulesQuery request, CancellationToken cancellationToken)
        {
            var criteria = _modelMapper.Map(request);
            return await this._dataHandler.ModulesReadOnly.AnyAsync(criteria);
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (_dataHandler?.Initialized ?? false);
            isValid = isValid && (_modelMapper != null);

            return isValid;
        }

        #endregion
    }
}
