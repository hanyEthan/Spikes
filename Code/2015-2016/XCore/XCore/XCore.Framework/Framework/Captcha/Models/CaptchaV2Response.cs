using Newtonsoft.Json;

namespace XCore.Framework.Framework.Captcha.Models
{
    public class CaptchaV2Response : JSON
    {
        [JsonProperty( "success" )]
        public bool IsSuccess { get; set; }

        [JsonProperty( "challenge_ts" )]
        public string ChallengeTs { get; set; }

        [JsonProperty( "error-codes" )]
        public string[] ErrorCodes { get; set; }

        [JsonProperty( "hostname" )]
        public string HostName { get; set; }

        [JsonProperty( "apk_package_name" )]
        public string ApkPackageName { get; set; }
    }
}
