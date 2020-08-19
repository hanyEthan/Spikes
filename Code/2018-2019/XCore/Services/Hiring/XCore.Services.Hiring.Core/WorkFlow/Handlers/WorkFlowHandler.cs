using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Utilities;

namespace XCore.Services.Hiring.Core.WorkFlow.Handlers
{
    public abstract class WorkFlowHandler<TStatus, TAction, TActionInfo, TRequest, TResponse>
                    where TStatus : Enum
                    where TAction : Enum
                    where TActionInfo : class, new()
                    where TRequest : class, new()
                    where TResponse : class, new()
    {
        #region props.

        public bool Initialized { get; private set; }

        #endregion
        #region cst.

        public WorkFlowHandler()
        {
            this.Initialized = Initialize();
        }

        #endregion


        #region publics.

        #endregion

        #region helpers.

        private bool Initialize()
        {
            try
            {
                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }

        #endregion
    }
}
