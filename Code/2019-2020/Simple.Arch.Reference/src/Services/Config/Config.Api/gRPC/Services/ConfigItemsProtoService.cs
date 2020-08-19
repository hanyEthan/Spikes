using System;
using System.Threading.Tasks;
using Grpc.Core;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Api.gRPC.Services
{
    public class ConfigItemsProtoService : ConfigItemsProtoAPI.ConfigItemsProtoAPIBase
    {
        #region props.

        private readonly ILogger<ConfigItemsProtoService> _logger;
        private readonly IMediator _mediator;
        protected virtual IModelMapper<BaseRequestContext, BaseRequestContext> _requestContextMapper { get; set; }

        private readonly IModelMapper<CreateConfigItemCommandProto, CreateConfigItemCommand> _createCommandMapper;
        private readonly IModelMapper<BaseResponseContext<int>, CreateConfigItemResponseProto> _createResponseMapper;

        #endregion
        #region cst.

        public ConfigItemsProtoService(ILogger<ConfigItemsProtoService> logger,
                                       IMediator mediator,
                                       IModelMapper<BaseRequestContext, BaseRequestContext> requestContextMapper,
                                       IModelMapper<CreateConfigItemCommandProto, CreateConfigItemCommand> createCommandMapper,
                                       IModelMapper<BaseResponseContext<int>, CreateConfigItemResponseProto> createResponseMapper)
        {
            this._logger = logger;
            this._mediator = mediator;
            this._requestContextMapper = requestContextMapper;
            this._createCommandMapper = createCommandMapper;
            this._createResponseMapper = createResponseMapper;
        }

        #endregion
        #region actions.

        [Authorize(Policy = "Internal")]
        public override async Task<CreateConfigItemResponseProto> Create(CreateConfigItemCommandProto request, ServerCallContext context)
        {
            try
            {
                #region request.

                var command = _createCommandMapper.Map(request);                              // data
                command = this._requestContextMapper.Map<CreateConfigItemCommand>(command);   // context

                #endregion
                #region processing.

                var result = await this._mediator.Send(command);

                #endregion
                #region response.

                // note : uncommenting the next line, will result in raising an exception in the client side (if we have any response that is not success). 
                //        if this is the desired case, then uncomment it.
                //ResponseContextMapper<int>.Map(result, context);      // context

                var response = this._createResponseMapper.Map(result);  // context + data

                return response;

                #endregion
            }
            catch (Exception x)
            {

                throw;
            }
        }

        #endregion
        #region helpers.

        #endregion
    }
}
