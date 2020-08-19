namespace XCore.Framework.Framework.Captcha.Models
{
    public class CaptchaV3Request
    {
        public string ClientToken { get; set; }
        public string ClientRemoteIP { get; set; }
        public string ActionTag { get; set; }
    }
}
