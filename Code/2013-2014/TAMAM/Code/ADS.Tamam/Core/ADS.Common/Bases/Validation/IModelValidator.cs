using System.Collections.Generic;

namespace ADS.Common.Validation
{
    public interface IModelValidator
    {
        bool? IsValid { get; }
        List<string> Errors { get; }
        List<ModelMetaPair> ErrorsDetailed { get; }
    }
}
