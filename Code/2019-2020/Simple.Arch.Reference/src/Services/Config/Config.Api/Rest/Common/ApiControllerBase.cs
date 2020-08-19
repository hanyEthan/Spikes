using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        #region props.

        protected virtual string ApiVersion
        {
            get
            {
                try
                {
                    return HttpContext.GetRequestedApiVersion().ToString();
                }
                catch
                {
                    return null;
                }
            }
        }
        protected virtual IModelMapper<BaseRequestContext, BaseRequestContext> RequestContextMapper { get; set; }

        #endregion
        #region cst.

        public ApiControllerBase(IModelMapper<BaseRequestContext, BaseRequestContext> requestContextMapper)
        {
            this.RequestContextMapper = requestContextMapper;
        }

        #endregion
    }
}
