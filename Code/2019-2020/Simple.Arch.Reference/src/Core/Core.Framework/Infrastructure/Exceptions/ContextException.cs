using System;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Exceptions
{
    public class ContextException : Exception
    {
        public BaseResponseContext BaseResponseContext { get; }
        public ContextException(BaseResponseContext baseResponseContext) : base()
        {
            this.BaseResponseContext = baseResponseContext;
        }
    }
}
