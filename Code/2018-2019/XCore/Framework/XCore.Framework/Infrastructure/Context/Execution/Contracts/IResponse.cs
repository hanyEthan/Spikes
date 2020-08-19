using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Execution.Models;

namespace Core.Components.Framework.Context.Contracts
{
    public interface IResponse
    {
        #region props.

        List<MetaPair> DetailedMessages { get; set; }
        string Code { get; set; }
        Exception Exception { get; set; }
        ResponseState State { get; set; }
        string Message { get; }

        #endregion

        void Set( ResponseState state , List<MetaPair> detailedMessage , string code , Exception exception );
    }
}
