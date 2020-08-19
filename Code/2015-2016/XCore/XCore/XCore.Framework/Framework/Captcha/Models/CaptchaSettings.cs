namespace XCore.Framework.Framework.Captcha.Models
{
    public class CaptchaSettings
    {
        #region props.

        public bool IsEnabled { get; set; }

        public string VerifyURL { get; set; }
        public string JavaScriptUrl { get; set; }
        public string SecretKey { get; set; }
        public double MinimumAcceptableScore { get; set; }

        #endregion
        #region statics.

        private static CaptchaSettings defaults;
        public static CaptchaSettings Defaults
        {
            get
            {
                return defaults = defaults ?? new CaptchaSettings()
                {
                    IsEnabled = false ,

                    VerifyURL = "https://www.google.com/recaptcha/api/siteverify" ,
                    JavaScriptUrl = "https://www.google.com/recaptcha/api.js" ,
                    SecretKey = null ,
                    MinimumAcceptableScore = 0.5 ,
                };
            }
        }

        #endregion
    }
}
