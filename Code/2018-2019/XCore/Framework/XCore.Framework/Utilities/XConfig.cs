using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace XCore.Framework.Utilities
{
    public class XConfig
    {
        #region props.

        public virtual bool IsInitialized { get; protected set; }
        public virtual IConfiguration Configuration { get; set; }

        #endregion
        #region cst.

        public XConfig() : this("appsettings.json")
        {
        }
        public XConfig(string configFile) : this(configFile, null)
        {
        }
        public XConfig(string configFile, string jsonContent)
        {
            this.IsInitialized = Initialize(configFile, jsonContent);
        }

        #endregion
        #region helpers.

        protected virtual bool Initialize(string configFile, string jsonContent)
        {
            this.Configuration = string.IsNullOrWhiteSpace(jsonContent)
                         ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                     .AddJsonFile(configFile, optional: true, reloadOnChange: true)
                                                     .Build()
                         : new ConfigurationBuilder().AddJsonFile(new InMemoryFileProvider(jsonContent), configFile, false, false)
                                                     .Build();

            return true;
        }

        #endregion
        #region publics.

        public T GetSection<T>(string sectionName) where T : new()
        {
            if (!this.IsInitialized) return default;

            var instance = (T)Activator.CreateInstance(typeof(T));
            this.Configuration.GetSection(sectionName)
                        .Bind(instance);

            return instance;
        }
        public bool Exists(string sectionName)
        {
            if (!this.IsInitialized) return false;
            return this.Configuration.GetSection(sectionName).Exists();
        }
        public string GetConnectionString(string key)
        {
            return this.Configuration.GetConnectionString(key);
        }
        public T GetValue<T>(string key)
        {
            return this.Configuration.GetValue<T>(key);
        }

        #endregion
        #region nested.

        public class InMemoryFileProvider : IFileProvider
        {
            private class InMemoryFile : IFileInfo
            {
                private readonly byte[] _data;
                public InMemoryFile(string json) => _data = Encoding.UTF8.GetBytes(json);
                public Stream CreateReadStream() => new MemoryStream(_data);
                public bool Exists { get; } = true;
                public long Length => _data.Length;
                public string PhysicalPath { get; } = string.Empty;
                public string Name { get; } = string.Empty;
                public DateTimeOffset LastModified { get; } = DateTimeOffset.UtcNow;
                public bool IsDirectory { get; } = false;
            }

            private readonly IFileInfo _fileInfo;
            public InMemoryFileProvider(string json) => _fileInfo = new InMemoryFile(json);
            public IFileInfo GetFileInfo(string _) => _fileInfo;
            public IDirectoryContents GetDirectoryContents(string _) => null;
            public IChangeToken Watch(string _) => NullChangeToken.Singleton;
        }

        #endregion
    }
}
