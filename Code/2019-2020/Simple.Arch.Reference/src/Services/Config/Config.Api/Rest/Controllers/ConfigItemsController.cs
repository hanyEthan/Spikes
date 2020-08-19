using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Api.Rest.Common;
using Mcs.Invoicing.Services.Config.Api.Rest.Models;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.GetConfigItem;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.ListConfigItems;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/configurations")]
    public class ConfigItemsController : ApiControllerBase
    {
        #region props.

        public bool Initialized { get; protected set; }

        private readonly ILogger<ConfigItemsController> _logger;
        private readonly IMediator _mediator;
        private readonly IModelMapper<CreateConfigItemCommandDTO, CreateConfigItemCommand> _createCommandMapper;
        private readonly IModelMapper<SearchResults<ConfigItem>, SearchResults<ConfigItemDTO>> _configItemListMapper;
        private readonly IModelMapper<ConfigItem, ConfigItemDTO> _configItemMapper;

        #endregion
        #region cst.

        public ConfigItemsController(ILogger<ConfigItemsController> logger,
                                     IMediator mediator,
                                     IModelMapper<ConfigItem, ConfigItemDTO> configItemMapper,
                                     IModelMapper<SearchResults<ConfigItem>, SearchResults<ConfigItemDTO>> configItemListMapper,
                                     IModelMapper<CreateConfigItemCommandDTO, CreateConfigItemCommand> createConfigItemCommandMapper,
                                     IModelMapper<BaseRequestContext, BaseRequestContext> requestContextMapper)
                                     : base(requestContextMapper)
        {
            this._logger = logger;
            this._mediator = mediator;
            this._configItemMapper = configItemMapper;
            this._configItemListMapper = configItemListMapper;
            this._createCommandMapper = createConfigItemCommandMapper;

            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        [HttpPost]
        public async Task<int> Create(CreateConfigItemCommandDTO commandDTO)
        {
            #region request.

            var command = _createCommandMapper.Map(commandDTO);                          // data
            command = base.RequestContextMapper.Map<CreateConfigItemCommand>(command);   // context

            #endregion
            #region processing.

            var result = await this._mediator.Send(command);

            #endregion
            #region response.

            return await ResponseContextMapper<int>.MapContent(result, result.Content, base.Response);

            #endregion
        }

        [HttpGet]
        public async Task<ConfigItemDTO> Get(int moduleId, string key)
        {
            #region request.

            var query = new GetConfigItemQuery() { ModuleId = moduleId, Key = key };  // data
            query = base.RequestContextMapper.Map<GetConfigItemQuery>(query);         // context

            #endregion
            #region processing.

            var result = await this._mediator.Send(query);

            #endregion
            #region response.

            return await ResponseContextMapper<ConfigItemDTO>.MapContent(result, this._configItemMapper.Map(result.Content), base.Response);

            #endregion
        }

        [HttpGet]
        [Route("search")]
        public async Task<SearchResults<ConfigItemDTO>> Get(int? moduleId, string partialKey, bool? isActive, int ps, int pn, ConfigItemsSearchCriteria.OrderDirection? ordDir, ConfigItemsSearchCriteria.OrderByExpression? ordExp, ConfigItemsSearchCriteria.OrderByCulture? ordCult = SearchCriteria.OrderByCulture.Default)
        {
            #region request.

            var query = Map(moduleId, partialKey, isActive, ps, pn, ordDir, ordExp, ordCult);  // data
            query = base.RequestContextMapper.Map<ListConfigItemsQuery>(query);                // context

            #endregion
            #region processing.

            var result = await this._mediator.Send(query);

            #endregion
            #region response.

            return await ResponseContextMapper<SearchResults<ConfigItemDTO>>.MapContent(result, this._configItemListMapper.Map(result.Content), base.Response);

            #endregion
        }

        [HttpGet]
        [Route("healthy")]
        [Authorize(Policy = "Internal")]
        public bool IsHealthy()
        {
            return this.Initialized;
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this._mediator != null);
            isValid = isValid && (this._createCommandMapper != null);
            isValid = isValid && (this._configItemListMapper != null);
            isValid = isValid && (this._configItemMapper != null);

            return isValid;
        }

        private ListConfigItemsQuery Map(int? moduleId, string partialKey, bool? isActive, int ps, int pn, ConfigItemsSearchCriteria.OrderDirection? ordDir, ConfigItemsSearchCriteria.OrderByExpression? ordExp, ConfigItemsSearchCriteria.OrderByCulture? ordCult = SearchCriteria.OrderByCulture.Default)
        {
            var to = new ListConfigItemsQuery();

            to.ModuleId = moduleId;
            to.Key = partialKey;
            to.IsActive = isActive;
            to.PageNumber = pn;
            to.PageSize = ps;
            to.OrderByDirection = ordDir;
            to.Order = ordExp;
            to.OrderByCultureMode = ordCult ?? to.OrderByCultureMode;

            return to;
        }

        #endregion
    }
}
