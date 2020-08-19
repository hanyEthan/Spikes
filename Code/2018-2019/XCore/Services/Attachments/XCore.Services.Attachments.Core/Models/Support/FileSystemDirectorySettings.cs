namespace XCore.Services.Attachments.Core.Models.Support
{
    public class FileSystemDirectorySettings
    {
        public bool CreateDirectoryIfNotFound { get; set; } = false;
        public string BaseDirectoryPath { get; set; }
        public string TempDirectoryPath { get; set; }
    }
}
