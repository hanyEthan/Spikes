using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.Services.Rest
{
    public class RestHandler : IRestHandler
    {
        #region props.

        public bool Initialized { get; private set; }
       
        public string BaseUrl { get; set; }
        protected virtual RestClient RestsharpClient { get; set; }

        #endregion
        #region cst.

        public RestHandler(string baseUrl)
        {
            try
            {
                this.BaseUrl = baseUrl;
                this.RestsharpClient = new RestClient(baseUrl);
                this.Initialized = Initialize();
            }
            catch (Exception x)
            {
                //throw;
            }
        }

        #endregion

        #region IRestHandler.

        //public RestResponse<TResponse> Call<TRequest, TResponse>(HttpMethod action, TRequest request, string urlResource = null) where TResponse : new()
        //{
        //    var requestSerialized = XSerialize.JSON.Serialize(request);

        //    var restRequest = new RestRequest(urlResource, (Method)(int)action);
        //    restRequest.AddJsonBody(requestSerialized);

        //    var restResponse = this.RestsharpClient.Execute<TResponse>(restRequest);
        //    TResponse resData = JsonConvert.DeserializeObject<TResponse>(restResponse.Content);

        //    return new RestResponse<TResponse>(restResponse.StatusCode.ToString(), resData);
        //}
        //public RestResponse<TResponse> Call<TResponse>(HttpMethod action, string urlResource = null) where TResponse : new()
        //{
        //    var restRequest = new RestRequest(urlResource, (Method)(int)action);
        //    // restRequest.AddJsonBody(request);
        //    var restResponse = this.RestsharpClient.Execute<TResponse>(restRequest);
        //    TResponse resData = JsonConvert.DeserializeObject<TResponse>(restResponse.Content);
        //    return new RestResponse<TResponse>(restResponse.StatusCode.ToString(), resData);
        //}

        public async Task<RestResponse<TResponse>> CallAsync<TRequest, TResponse>(HttpMethod action, TRequest request, string urlResource = null) where TResponse : new()
        {
            var requestSerialized = XSerialize.JSON.Serialize(request);

            var restRequest = new RestRequest(urlResource, (Method)(int)action);
            restRequest.AddJsonBody(requestSerialized);

            var taskCompletionSource = new TaskCompletionSource<RestResponse<TResponse>>();
            this.RestsharpClient.ExecuteAsync<TResponse>(restRequest, (response) => taskCompletionSource.SetResult(new RestResponse<TResponse>()
            {
                Code = response.StatusCode.ToString(), 
                Response = JsonConvert.DeserializeObject<TResponse>(response.Content),
            }));

            return await taskCompletionSource.Task;
        }
        public async Task<RestResponse<TResponse>> CallAsync<TResponse>(HttpMethod action, string urlResource = null) where TResponse : new()
        {
            var restRequest = new RestRequest(urlResource, (Method)(int)action);

            var taskCompletionSource = new TaskCompletionSource<RestResponse<TResponse>>();
            this.RestsharpClient.ExecuteAsync<TResponse>(restRequest, (response) => taskCompletionSource.SetResult(new RestResponse<TResponse>()
            {
                Code = response.StatusCode.ToString(),
                Response = JsonConvert.DeserializeObject<TResponse>(response.Content),
            }));

            return await taskCompletionSource.Task;
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.RestsharpClient != null;
            isValid = isValid && !string.IsNullOrWhiteSpace(this.BaseUrl);

            return isValid;
        }

        #endregion
    }
    public class RestHandler<TConfigData> : RestHandler, IRestHandler<TConfigData> where TConfigData : IConfigData
    {
        #region cst.

        public RestHandler(IConfigProvider<TConfigData> configProvider) : base(configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.Endpoint)
        {
        }

        #endregion
    }
}
