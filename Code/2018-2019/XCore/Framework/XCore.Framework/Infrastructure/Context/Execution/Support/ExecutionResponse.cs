using System;
using System.Collections.Generic;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Models;

namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    [Serializable]
    public class ExecutionResponse<T> : ExecutionResponseBasic
    {
        #region props.

        public T Result { get; set; }

        #endregion
        #region publics.

        public virtual T Set( IResponse source )
        {
            return Set( source.State , default( T ) , source.DetailedMessages , source.Code , source.Exception );
        }
        public virtual T Set( ExecutionResponse<T> source )
        {
            return Set( source.State , source.Result , source.DetailedMessages , source.Code , source.Exception );
        }

        public bool Set(object responseStatus)
        {
            throw new NotImplementedException();
        }

        public virtual T Set<D>( ExecutionResponse<D> source , T result )
        {
            return Set( source.State , result , source.DetailedMessages , source.Code , source.Exception );
        }
        public virtual T Set( ResponseState state , T result )
        {
            return Set( state , result , new List<MetaPair>() );
        }
        public virtual T Set( ResponseState state , T result , List<MetaPair> detailedMessage )
        {
            this.State = state;
            this.Result = result;
            this.DetailedMessages = detailedMessage ?? new List<MetaPair>();

            return Result;
        }
        public virtual T Set( ResponseState state , T result , List<MetaPair> detailedMessage , string code , Exception exception )
        {
            Set( state , result , detailedMessage );

            this.Code = code;
            this.Exception = exception;

            return Result;
        }
        public virtual void Set( ResponseState state , List<MetaPair> detailedMessage , string code , Exception exception )
        {
            Set( state , default( T ) , detailedMessage , code , exception );
        }  
        
        #endregion
    }
}
