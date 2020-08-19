using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.AnyConfigItems
{
    public class AnyConfigItemsQueryHandler : MediatR.IRequestHandler<AnyConfigItemsQuery, bool>
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly ILogger<AnyConfigItemsQueryHandler> _logger;
        private readonly IConfigurationDataUnity _dataHandler;
        private readonly IModelMapper<AnyConfigItemsQuery, ConfigItemsSearchCriteria> _modelMapper;

        #endregion
        #region cst.

        public AnyConfigItemsQueryHandler(IConfigurationDataUnity dataHandler,
                                          IModelMapper<AnyConfigItemsQuery, ConfigItemsSearchCriteria> modelMapper,
                                          ILogger<AnyConfigItemsQueryHandler> logger)
        {
            this._logger = logger;
            this._dataHandler = dataHandler;
            this._modelMapper = modelMapper;

            this.Initialized = Initialize();
        }

        #endregion
        #region MediatR.IRequestHandler

        public async Task<bool> Handle(AnyConfigItemsQuery request, CancellationToken cancellationToken)
        {
            var criteria = _modelMapper.Map(request);
            return await this._dataHandler.ConfigItemsReadOnly.AnyAsync(criteria);
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
