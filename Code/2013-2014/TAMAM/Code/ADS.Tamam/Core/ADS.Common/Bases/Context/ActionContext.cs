using System;

namespace ADS.Common.Context
{
    [Serializable]
    public class ActionContext
    {
        #region props ...

        public string ModuleId { get; set; }
        public string ActionId { get; set; }
        public string ActionKey { get; set; }

        public string MessageForDenied { get; set; }
        public string MessageForFailure { get; set; }
        public string MessageForSuccess { get; set; }

        #endregion
        #region cst ...

        public ActionContext() { }
        public ActionContext( string moduleId , string actionId , string actionKey ) : this()
        {
            ModuleId = moduleId;
            ActionId = actionId;
            ActionKey = actionKey;
        }
        public ActionContext( string moduleId , string actionId , string actionKey , string messageForDenied , string messageForFailure , string messageForSuccess ) : this( moduleId , actionId , actionKey )
        {
            MessageForDenied = messageForDenied;
            MessageForFailure = messageForFailure;
            MessageForSuccess = messageForSuccess;
        }

        #endregion
    }
}
