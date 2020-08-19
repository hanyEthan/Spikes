using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.ListConfigItems
{
    public class ListConfigItemsQueryHandler : MediatR.IRequestHandler<ListConfigItemsQuery, BaseResponseContext<SearchResults<ConfigItem>>>
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly ILogger<ListConfigItemsQueryHandler> _logger;
        private readonly IConfigurationDataUnity _dataHandler;
        private readonly IModelMapper<ListConfigItemsQuery, ConfigItemsSearchCriteria> _modelMapper;
        private string _modelIncludes { get; set; }

        #endregion
        #region cst.

        public ListConfigItemsQueryHandler(IConfigurationDataUnity dataHandler,
                                           IModelMapper<ListConfigItemsQuery, ConfigItemsSearchCriteria> modelMapper,
                                           ILogger<ListConfigItemsQueryHandler> logger)
        {
            this._logger = logger;
            this._dataHandler = dataHandler;
            this._modelMapper = modelMapper;

            this.Initialized = Initialize();
        }

        #endregion
        #region MediatR.IRequestHandler

        public async Task<BaseResponseContext<SearchResults<ConfigItem>>> Handle(ListConfigItemsQuery request, CancellationToken cancellationToken)
        {
            var criteria = _modelMapper.Map(request);
            var result = await this._dataHandler.ConfigItemsReadOnly.GetAsync(criteria, this._modelIncludes);
            return request.SetResponse(result);
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (_dataHandler?.Initialized ?? false);
            isValid = isValid && (_modelMapper != null);
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            this._modelIncludes = string.Join(",", new List<string>()
            {
                "Module",
            });

            return true;
        }

        #endregion
    }
}
