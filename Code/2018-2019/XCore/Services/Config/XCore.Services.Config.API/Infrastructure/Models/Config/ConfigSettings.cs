namespace XCore.Services.Config.API.Infrastructure.Models.Config
{
    public class ConfigSettings
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }

        public int Port { get; set; }
        public string BasePath { get; set; }
    }
}
