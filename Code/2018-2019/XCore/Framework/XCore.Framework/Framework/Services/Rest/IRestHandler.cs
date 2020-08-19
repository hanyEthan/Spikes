using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Config.Contracts;

namespace XCore.Framework.Framework.Services.Rest
{
    public interface IRestHandler
    {
        bool Initialized { get; }

        //RestResponse<TResponse> Call<TRequest, TResponse>(HttpMethod action, TRequest request, string urlResource = null) where TResponse : new();
        //RestResponse<TResponse> Call<TResponse>(HttpMethod action, string urlResource = null) where TResponse : new();

        Task<RestResponse<TResponse>> CallAsync<TRequest, TResponse>(HttpMethod action, TRequest request, string urlResource = null) where TResponse : new();
        Task<RestResponse<TResponse>> CallAsync<TResponse>(HttpMethod action, string urlResource = null) where TResponse : new();
    }
    public interface IRestHandler<TConfigData> : IRestHandler where TConfigData : IConfigData
    {
    }
}