using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Configurations.SDK.Models.DTOs;
using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Clients.Common.Framework.Configurations
{
    public class RemoteConfigProvider<TConfigResult> : IConfigProvider<TConfigResult>
    {
        #region props.

        public bool Initialized { get; protected set; }

        protected virtual IConfigClient ConfigClient { get; set; }
        protected virtual ConfigKeyDTO ConfigKey { get; set; }

        #endregion
        #region cst.

        public RemoteConfigProvider(IConfigClient configClient, ConfigKeyDTO configKey)
        {
            this.ConfigClient = configClient;
            this.ConfigKey = configKey;

            this.Initialized = Initialize();
        }

        #endregion

        #region IConfigProvider

        public async Task<TConfigResult> GetConfigAsync()
        {
            var request = GetRequest();
            var responseDTO = await this.ConfigClient.Get(request);
            var response = Map(responseDTO);

            return response;
        }

        #endregion
        #region helpers.

        private ServiceExecutionRequestDTO<ConfigKeyDTO> GetRequest()
        {
            return new ServiceExecutionRequestDTO<ConfigKeyDTO>()
            {
                RequestClientToken = XCore.Services.Clients.Common.Constants.Constants.Requestoken,
                Content = this.ConfigKey,
            };
        }
        private TConfigResult Map(RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>> from)
        {
            return string.IsNullOrWhiteSpace(from?.Response?.Content?.Value)
                 ? default
                 : XSerialize.JSON.Deserialize<TConfigResult>(from.Response.Content.Value);
        }

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ConfigClient != null;
            isValid = isValid && this.ConfigClient.Initialized;
            isValid = isValid && this.ConfigKey != null;

            return isValid;
        }

        #endregion
    }
}
