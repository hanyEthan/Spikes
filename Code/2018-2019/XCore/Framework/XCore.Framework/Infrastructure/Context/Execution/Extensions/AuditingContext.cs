using System;
using System.Threading.Tasks;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Models;

namespace XCore.Framework.Infrastructure.Context.Execution.Extensions
{
    [Serializable]
    public class AuditingContext : IContextStep
    {
        #region props.

        public string ModuleId { get; set; }
        public string ActionId { get; set; }

        public string MessageForDenied { get; set; }
        public string MessageForFailure { get; set; }
        public string MessageForSuccess { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        private IActionContext Context;

        #endregion
        #region cst.

        public AuditingContext() { }
        public AuditingContext( string moduleId , string actionId , string messageForDenied , string messageForFailure , string messageForSuccess ) : this()
        {
            this.ModuleId = moduleId;
            this.ActionId = actionId;
            this.MessageForDenied = messageForDenied;
            this.MessageForFailure = messageForFailure;
            this.MessageForSuccess = messageForSuccess;
        }

        public AuditingContext(string moduleId, string actionId, string messageForDenied, string messageForFailure, string messageForSuccess, string userName, string userId) : this()
        {
            this.ModuleId = moduleId;
            this.ActionId = actionId;
            this.MessageForDenied = messageForDenied;
            this.MessageForFailure = messageForFailure;
            this.MessageForSuccess = messageForSuccess;
            this.UserId = userId;
            this.UserName = userName;
        }

        #endregion
        #region IContextStep

        public async Task<IResponse> Process( IActionContext context )
        {
            this.Context = context;

            switch ( context.Response.State )
            {
                case ResponseState.Success:
                    {
                        //if ( Settings.Audit.AuditOnlyInCaseOfFailure ) return new ExecutionResponseBasic() { State = ResponseState.Success };
                        return await Audit( this.MessageForSuccess );
                    }
                case ResponseState.AccessDenied:
                    {
                        return await Audit( this.MessageForDenied );
                    }
                default:
                    {
                        return await Audit( this.MessageForFailure );
                    }
            }
        }
        public async Task<IResponse> Process( IActionContext context , ResponseState mode )
        {
            this.Context = context;
            switch ( mode )
            {
                case ResponseState.Success:
                    {
                        //if ( Settings.Audit.AuditOnlyInCaseOfFailure ) return new ExecutionResponseBasic() { State = ResponseState.Success };
                        return await Audit( this.MessageForSuccess );
                    }
                case ResponseState.AccessDenied:
                    {
                        return await Audit( this.MessageForDenied );
                    }
                default:
                    {
                        return await Audit( this.MessageForFailure );
                    }
            }
        }

        #endregion

        #region helpers

        private async Task<IResponse> Audit( string message )
        {
            return await Audit( message , null );
        }
        private async Task<IResponse> Audit( string message , string reference )
        {
            throw new NotImplementedException();

            //int moduleId, actionId;
            //if ( !int.TryParse( this.ModuleId , out moduleId ) || !int.TryParse( this.ActionId , out actionId ) ) return new ExecutionResponseBasic() { State = ResponseState.Error , };

            //var entry = new AuditTrailLog()
            //{
            //    //UserId = this.Context.Request.SecurityToken.ToString(),
            //    UserId = this.Context.Request.UserId != null? this.Context.Request.UserId : UserId,
            //    Date = DateTime.UtcNow ,
            //    Username = this.Context.Request.Username != null? this.Context.Request.Username : UserName,
            //    ModuleId = moduleId ,
            //    ActionId = actionId ,
            //    IPAddress = this.Context.Request.SourceIP ,
            //    MachineName = this.Context.Request.SourceMachine ,
            //    RefKey = reference ,
            //    Details = message ,
            //};

            //return SysUnity.Audit.Log( entry );
        }

        #endregion
    }
}
