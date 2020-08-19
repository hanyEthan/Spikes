using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.Models
{
    public class ErrorResponse
    {
        public virtual BaseResponseContext.HeaderContent error { get; set; }
    }
}
