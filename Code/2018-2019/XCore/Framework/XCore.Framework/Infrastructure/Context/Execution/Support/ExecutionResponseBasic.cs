using System;
using System.Collections.Generic;
using System.Text;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Models;

namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    [Serializable]
    public class ExecutionResponseBasic : IResponse
    {
        #region props.

        public virtual List<MetaPair> DetailedMessages { get; set; }
        public virtual string Code { get; set; }
        public virtual Exception Exception { get; set; }
        public virtual ResponseState State { get; set; }
        public virtual string Message
        {
            get
            {
                try
                {
                    var message = new StringBuilder();
                    message.Append(string.Format("( Status : {0} )", State.ToString()));
                    foreach (var DetailedMessage in DetailedMessages)
                    {
                        message.Append(string.Format(" ( {0} : {1} )", DetailedMessage.Property, DetailedMessage.Meta));
                    }

                    return message.ToString();
                }
                catch (Exception)
                {
                    return "!!";
                    //throw;
                }
            }
        }

        #endregion
        #region publics.

        public void Set( ResponseState state , List<MetaPair> detailedMessage , string code , Exception exception )
        {
            this.State = state;
            this.DetailedMessages = detailedMessage ?? new List<MetaPair>();
            this.Code = code;
            this.Exception = exception;
        }

        #endregion
    }
}
