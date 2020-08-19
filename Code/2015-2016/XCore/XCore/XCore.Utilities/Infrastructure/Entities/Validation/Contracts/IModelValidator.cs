using System.Collections.Generic;
using XCore.Utilities.Infrastructure.Context.Execution.Models;
using XCore.Utilities.Infrastructure.Entities.Validation.Models;

namespace XCore.Utilities.Infrastructure.Entities.Validation.Contracts
{
    public interface IModelValidator<T>
    {
        bool? IsValid { get; }
        List<MetaPair> Errors { get; }

        bool Validate( T model , ValidationMode mode );
    }
}
