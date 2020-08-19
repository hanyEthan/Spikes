using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Validation.Models;

namespace XCore.Framework.Infrastructure.Entities.Validation.Contracts
{
    public interface IModelValidator<T>
    {
        Task<ValidationResponse> ValidateAsync(T model, ValidationMode? mode = null);
    }
}
