using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Execution.Models;

namespace XCore.Framework.Infrastructure.Entities.Validation.Models
{
    public class ValidationResponse
    {
        #region props.

        public bool IsValid { get; set; }
        public List<MetaPair> Errors { get; set; }

        #endregion
        #region statics.

        public readonly static ValidationResponse Error = new ValidationResponse();

        #endregion
    }
}
