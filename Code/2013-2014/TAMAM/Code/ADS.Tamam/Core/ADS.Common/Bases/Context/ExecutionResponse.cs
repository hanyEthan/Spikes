using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ADS.Common.Validation;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace ADS.Common.Context
{
    [Serializable]
    public class ExecutionResponse<T> where T : new()
    {
        #region props ...

        public T Result { get; set; }
        public virtual List<ModelMetaPair> MessageDetailed { get; set; }
        public virtual string Code { get; set; }
        public virtual Exception Exception { get; set; }
        public virtual ResponseState Type { get; set; }

        #endregion
        #region publics

        public virtual T Set( ExecutionResponse<T> source )
        {
            Type = source.Type;
            Result = source.Result;
            Code = source.Code;
            Exception = source.Exception;
            MessageDetailed = source.MessageDetailed;

            return Result;
        }
        public virtual T Set( ResponseState type , T result )
        {
            Type = type;
            Result = result;
            MessageDetailed = new List<ModelMetaPair>();

            return Result;
        }
        public virtual T Set( ResponseState type , T result , List<ModelMetaPair> detailedMessage )
        {
            Type = type;
            Result = result;
            MessageDetailed = detailedMessage ?? new List<ModelMetaPair>();

            return Result;
        }
        public virtual T Set( ResponseState type , T result , List<ModelMetaPair> detailedMessage , string code , Exception exception )
        {
            Set( type , result , detailedMessage );

            Code = code;
            Exception = exception;

            return Result;
        }

        #endregion
    }
}