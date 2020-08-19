using System;
using System.Threading.Tasks;
using Flurl.Http;
using XCore.Framework.Framework.Captcha.Models;
using XCore.Utilities.Logger;

namespace XCore.Framework.Framework.Captcha.Handlers
{
    public class CaptchaHandler
    {
        #region props.

        public bool Initialized { get; set; }
        private CaptchaSettings Settings { get; set; }

        #endregion
        #region cst.

        public CaptchaHandler() : this( CaptchaSettings.Defaults )
        {
        }
        public CaptchaHandler( string secretKey ) : this( CaptchaSettings.Defaults , secretKey )
        {
        }
        public CaptchaHandler( CaptchaSettings settings ) : this( settings , null )
        {
        }
        public CaptchaHandler( CaptchaSettings settings , string secretKey )
        {
            try
            {
                this.Settings = settings;
                this.Settings.SecretKey = secretKey ?? this.Settings.SecretKey;

                this.Initialized = Initialize();
            }
            catch ( Exception x )
            {
                this.Initialized = false;
            }
        }

        #endregion
        #region publics.

        public virtual async Task<bool> Verify( CaptchaV3Request request )
        {
            try
            {
                if ( Skip() ) return true;

                var response = await VerifyReCaptcha( request );
                var result = Validate( request , response );

                return result;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            return true;
        }

        private bool Skip()
        {
            return this.Settings?.IsEnabled == true;
        }
        private bool Validate( CaptchaSettings Settings )
        {
            bool isValid = true;

            isValid = isValid && Settings != null;
            isValid = isValid && !string.IsNullOrWhiteSpace( Settings?.VerifyURL );
            isValid = isValid && !string.IsNullOrWhiteSpace( Settings?.JavaScriptUrl );
            isValid = isValid && !string.IsNullOrWhiteSpace( Settings?.SecretKey );
            isValid = isValid && Settings?.MinimumAcceptableScore >  0;
            isValid = isValid && Settings?.MinimumAcceptableScore <= 1;

            return isValid;
        }
        private bool Validate( CaptchaV2Request request )
        {
            bool isValid = true;

            isValid = isValid && this.Initialized;
            isValid = isValid && Validate( this.Settings );

            isValid = isValid && request != null;
            isValid = isValid && !string.IsNullOrWhiteSpace( request?.ClientResponse );

            return isValid;
        }
        private bool Validate( CaptchaV3Request request )
        {
            bool isValid = true;

            isValid = isValid && this.Initialized;
            isValid = isValid && Validate( this.Settings );

            isValid = isValid && request != null;
            isValid = isValid && !string.IsNullOrWhiteSpace( request?.ClientToken );
            isValid = isValid && !string.IsNullOrWhiteSpace( request?.ActionTag );

            return isValid;
        }
        private bool Validate( CaptchaV3Request request , CaptchaV3Response response )
        {
            bool isValid = true;

            isValid = isValid && response != null;
            isValid = isValid && response.Score >= this.Settings.MinimumAcceptableScore;
            isValid = isValid && request.ActionTag == response.Action;

            return isValid;
        }

        private object Map( CaptchaV2Request from )
        {
            return new
            {
                response = from.ClientResponse ,
                remoteip = from.ClientRemoteIP ,
                secret = this.Settings.SecretKey ,
            };
        }
        private object Map( CaptchaV3Request from )
        {
            return new
            {
                response = from.ClientToken ,
                remoteip = from.ClientRemoteIP ,
                secret = this.Settings.SecretKey ,
            };
        }

        private async Task<T> RestCall<T>( string url , object request )
        {
            var postAction = url.PostUrlEncodedAsync( request );
            var postResponse = await postAction.ConfigureAwait( false );
            if ( !postResponse.IsSuccessStatusCode ) return default(T);

            var response = await postAction.ReceiveJson<T>().ConfigureAwait( false );
            return response;
        }
        private async Task<CaptchaV3Response> VerifyReCaptcha( CaptchaV3Request request )
        {
            try
            {
                bool isValid = Validate( request );
                if ( !isValid ) return CaptchaV3Response.Default;

                var response = await RestCall<CaptchaV3Response>( this.Settings.VerifyURL , Map( request ) );
                return response ?? CaptchaV3Response.Default;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return CaptchaV3Response.Default;
            }
        }

        #endregion
    }
}
