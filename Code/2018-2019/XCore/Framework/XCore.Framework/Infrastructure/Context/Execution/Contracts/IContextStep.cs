using System.Threading.Tasks;

namespace Core.Components.Framework.Context.Contracts
{
    public interface IContextStep
    {
        Task<IResponse> Process( IActionContext context );
    }
}
