using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.GetConfigItem
{
    public class GetConfigItemQueryHandler : MediatR.IRequestHandler<GetConfigItemQuery, BaseResponseContext<ConfigItem>>
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly ILogger<GetConfigItemQueryHandler> _logger;
        private readonly IConfigurationDataUnity _dataHandler;
        private string _modelIncludes { get; set; }

        #endregion
        #region cst.

        public GetConfigItemQueryHandler(IConfigurationDataUnity dataHandler,
                                         ILogger<GetConfigItemQueryHandler> logger)
        {
            this._logger = logger;
            this._dataHandler = dataHandler;

            this.Initialized = Initialize();
        }

        #endregion
        #region MediatR.IRequestHandler

        public async Task<BaseResponseContext<ConfigItem>> Handle(GetConfigItemQuery request, CancellationToken cancellationToken)
        {
            #region DL.

            var result = await this._dataHandler.ConfigItemsReadOnly.GetAsync(request.ModuleId, request.Key, this._modelIncludes);

            #endregion
            #region response.

            if (result != null)
            {
                return request.SetResponse(result);
            }
            else
            {
                return request.SetResponseNative<BaseResponseContext<ConfigItem>>(ResponseCode.NotFound);
            }

            #endregion
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (_dataHandler?.Initialized ?? false);
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
