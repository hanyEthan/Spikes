using System.Threading.Tasks;
using domain.Support;

namespace application.Contracts
{
    public interface IRulesValidator
    {
        Task<BaseResponse> Validate(params string[] jsons);
    }
}
