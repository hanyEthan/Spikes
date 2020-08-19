using Newtonsoft.Json;

namespace XCore.Framework.Framework.Captcha.Models
{
    public class CaptchaV3Response : JSON
    {
        [JsonProperty( "success" )]
        public bool IsSuccess { get; set; }

        [JsonProperty( "score" )]
        public float Score { get; set; }

        [JsonProperty( "action" )]
        public string Action { get; set; }

        [JsonProperty( "challenge_ts" )]
        public string ChallengeTs { get; set; }

        [JsonProperty( "error-codes" )]
        public string[] ErrorCodes { get; set; }

        [JsonProperty( "hostname" )]
        public string HostName { get; set; }

        public static readonly CaptchaV3Response Default = new CaptchaV3Response() { IsSuccess = false };
    }
}
