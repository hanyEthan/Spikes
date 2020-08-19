using System;
using System.Threading;
using ADS.Common.Handlers;
using ADS.Common.Utilities;

namespace ADS.Common.Context
{
    public class ExecutionContext<T> where T : new()
    {
        #region props

        private ExecutionResponse<T> _Response = new ExecutionResponse<T>();
        public ExecutionResponse<T> Response { get { return _Response; } private set { _Response = value; } }

        public ActionContext ActionContext { get; set; }

        #endregion
        #region publics

        public virtual T Execute( Func<T> func , RequestContextBase requestContex )
        {
            try
            {
                XLogger.Trace( "" , XLogger.Enums.LogSettings.TransientCaller );  // log

                if (!CheckLicense()) return Response.Result;  // expired

                SetCulture( requestContex );  // culture
                //ActionContext = actionContext;  // action

                return Response.Result = func();  // logic + response
            }
            catch ( Exception x )
            {
                SetFailure( x );  // exception
                return Response.Result;  // response
            }
        }
        public virtual T Execute( Action action , RequestContextBase requestContex )
        {
            try
            {
                XLogger.Trace( "" , XLogger.Enums.LogSettings.TransientCaller );  // log

                if (!CheckLicense()) return Response.Result;  // expired

                SetCulture( requestContex );  // culture
                //ActionContext = actionContext;  // action
                
                action();  // logic
                return Response.Result;  // response
            }
            catch ( Exception x )
            {
                SetFailure( x );  // exception
                return Response.Result;  // response
            }
        }

        public virtual T Execute( Action action )
        {
            try
            {
                XLogger.Trace( "" , XLogger.Enums.LogSettings.TransientCaller );  // log

                action();  // logic
                return Response.Result;  // response
            }
            catch ( Exception x )
            {
                SetFailure( x );  // exception
                return Response.Result;  // response
            }
        }

        #endregion
        #region helpers

        private void SetCulture( RequestContextBase requestContex )
        {
            if ( requestContex == null || string.IsNullOrEmpty( requestContex.CultureName ) ) return;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo( requestContex.CultureName );
        }
        private void SetFailure( Exception x )
        {
            Response.Exception = x;
            Response.Type = ResponseState.Failure;
            XLogger.Error( "Exception : " + x );

//#if DEBUG
//            System.Diagnostics.Debugger.Break();    // only for debugging, remove in case of deployment / production.
//#endif
        }
        private bool CheckLicense()
        {
            if (!Broker.Licensed)
            {
                Response.Type = ResponseState.LicenseError;
                XLogger.Error("License Expired.");
                return false;
            }

            return true;
        }

        #endregion
    }
}
