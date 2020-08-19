namespace XCore.Services.Config.API.Infrastructure.Models.Config
{
    public class JWTSettings
    {
        public string SigningSecret { get; set; }
        public int ExpiryDuration { get; set; }
    }
}
