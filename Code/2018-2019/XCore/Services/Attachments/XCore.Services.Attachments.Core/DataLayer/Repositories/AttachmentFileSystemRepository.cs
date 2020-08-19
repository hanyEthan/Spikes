using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Enums;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Config.Core.DataLayer.Repositories
{
    public class AttachmentFileSystemRepository : IAttachmentRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private FileSystemDirectorySettings Settings { get; set; }
        private DirectoryInfo BaseDirectory { get; set; }
        private DirectoryInfo TempDirectory { get; set; }

        #endregion
        #region cst.

        public AttachmentFileSystemRepository(FileSystemDirectorySettings configuration)
        {
            this.Settings = configuration;
            this.Initialized = Initialize();
        }

        #endregion
        #region IAttachmentsRepository
        public async Task<SearchResults<Attachment>> GetAsync(AttachmentSearchCriteria criteria)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            List<Attachment> attachments = await GetByIdAsync(criteria?.Id, false);
            return attachments != null
                 ? new SearchResults<Attachment>() { Results = attachments }
                 : null;

        }
        public async Task<List<Attachment>> GetByIdAsync(List<string> id, bool detached = false, string includeProperties = null)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            if (id == null || !id.Any()) return null;
            List<string> pathes = await GetAllFile(id) ?? await GetAllFile(id, isTemp: true);
            if (pathes == null || !pathes.Any()) return null;

            List<Attachment> attachments = new List<Attachment>();

            foreach (var path in pathes)
            {
                var fileProvider = new FileExtensionContentTypeProvider();
                if (!fileProvider.TryGetContentType(Path.GetFileName(path), out string contentType))
                {
                    throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {Path.GetFileName(path)}.");
                }
                attachments.Add(new Attachment()
                {
                    Id = Path.GetFileName(path) ,
                    Name = Path.GetFileName(path),
                    Extension = Path.GetExtension(path),
                    MimeType = GetContentType(path),
                    Body = await File.ReadAllBytesAsync(path),
                });
            }

            return attachments;

        }   
        public async Task<Attachment> GetByIdAsync(object id, bool detached = false, string includeProperties = null)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            if (id == null || string.IsNullOrWhiteSpace(id?.ToString())) return null;

            string path = await GetFile(id.ToString()) ?? await GetFile(id.ToString(), isTemp: true);
            if (string.IsNullOrWhiteSpace(path)) return null;

            return new Attachment()
            {
                Id = Path.GetFileName(path),
                Name = Path.GetFileName(path),
                Extension = Path.GetExtension(path),
                MimeType = GetContentType(path),
                Body = await File.ReadAllBytesAsync(path),

            };
        }
        public SearchResults<Attachment> Get(AttachmentSearchCriteria criteria)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            var attachment = GetById(criteria?.Id);
            return attachment != null
                 ? new SearchResults<Attachment>() { Results = new List<Attachment>() { attachment }, TotalCount = 1 }
                 : null;
        }
        public Attachment GetById(object id, bool detached = false, string includeProperties = null)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            return GetByIdAsync(id, detached).GetAwaiter().GetResult();
        }
        public async Task CreateAsync(Attachment entity, string createdBy = null)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            var date = DateTime.Now.Date;
            bool IsTemporary = CheckIfTemp(entity);
            var directory = GetAttechmentDirectory(date, IsTemporary);
            CreateDirectory(directory);
            var fileName = GetFileName(date, entity, out string prefix);
            if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("attachment name is not valid.");

            await File.WriteAllBytesAsync($@"{directory}{fileName}", entity.Body);

            entity.Id = prefix;

            // return entity;
        }
        public async Task Update(Attachment entity, string modifiedBy = null)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            if (string.IsNullOrWhiteSpace(entity?.Id) || entity?.Body == null || entity?.Body?.Length == 0)
            {
                throw new Exception("invalid input.");
            }

            string path = await GetFile(entity.Id.ToString()) ?? await GetFile(entity.Id.ToString(), isTemp: true);
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("file couldn't be found.");
            }

            await File.WriteAllBytesAsync(path, entity.Body);
        }
        void IRepository<Attachment>.Update(Attachment entity, string modifiedBy)
        {
            Update(entity, modifiedBy).GetAwaiter().GetResult();
        }
        public async Task Delete(object id)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            if (string.IsNullOrWhiteSpace(id?.ToString()))
            {
                throw new Exception("invalid input.");
            }

            string path = await GetFile(id.ToString()) ?? await GetFile(id.ToString(), isTemp: true);
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("file couldn't be found.");
            }

            File.Delete(path);
        }
        public async Task DeleteAsync(object id)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            await Delete(id);
        }
        void IRepository<Attachment>.Delete(Attachment entity)
        {
            Delete(entity).GetAwaiter().GetResult();
        }
        public async Task<bool> AnyAsync(AttachmentSearchCriteria criteria)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion

            List<string> path = await GetAllFile(criteria.Id) ?? await GetAllFile(criteria.Id, isTemp: true);
            return path.Any();
        }
        public void CreateConfirm(List<Attachment> attachments)
        {
            #region validate.

            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("AttachmentFileSystemRepository is not initialized correctly");
            }

            #endregion
            if (attachments.Any(x => string.IsNullOrWhiteSpace(x.Name?.ToString())))
            {
                throw new Exception("invalid input.");
            }
            foreach (var attachment in attachments)
            {
                var BaseDirectory = GetAttechmentDirectory(attachment.Name);
                CreateDirectory(BaseDirectory);
                var TempDirectory = GetAttechmentDirectory(attachment.Name, true);

                if (!File.Exists($@"{TempDirectory}{attachment.Name}"))
                {
                    throw new Exception("file couldn't be found.");
                }

                File.Move($@"{TempDirectory}{attachment.Name}", $@"{BaseDirectory}{attachment.Name}");
            }
        }
        public void DeleteSoft(Attachment attachment)
        {
            throw new NotImplementedException();
        }
        public async Task CreateAsync(Attachment[] attachments, string createdBy = null)
        {
            foreach (var attachment in attachments)
            {
                await CreateAsync(attachment, null);
            }

        }
        public void DeleteList(List<Attachment> ids)
        {
            foreach (var id in ids)
            {
               DeleteAsync(id);
            }
        }
        public void Dispose()
        { }
        public Task SetActivationAsync(object id, bool isActive)
        {
            throw new NotImplementedException();
        }
        Task<int> IRepository<Attachment>.SaveAsync()
        {
            throw new NotImplementedException();
        }
        public Task<int> CountAsync(Expression<Func<Attachment, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AnyAsync(Expression<Func<Attachment, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
        public void MarkAs(Attachment entity, string modifiedBy = null)
        {
            throw new NotImplementedException();
        }
        public void Save()
        {
            throw new NotImplementedException();
        }
        public void SetActivation(object id, bool isActive)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Attachment> GetAll(Func<IQueryable<Attachment>, IOrderedQueryable<Attachment>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Attachment> Get(Expression<Func<Attachment, bool>> filter = null, Func<IQueryable<Attachment>, IOrderedQueryable<Attachment>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public Attachment GetOne(Expression<Func<Attachment, bool>> filter = null, string includeProperties = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public Attachment GetFirst(Expression<Func<Attachment, bool>> filter = null, Func<IQueryable<Attachment>, IOrderedQueryable<Attachment>> orderBy = null, string includeProperties = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public int Count(Expression<Func<Attachment, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
        public bool Any(Expression<Func<Attachment, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Attachment>> GetAllAsync(Func<IQueryable<Attachment>, IOrderedQueryable<Attachment>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Attachment>> GetAsync(Expression<Func<Attachment, bool>> filter = null, Func<IQueryable<Attachment>, IOrderedQueryable<Attachment>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public Task<Attachment> GetOneAsync(Expression<Func<Attachment, bool>> filter = null, string includeProperties = null, bool detached = false)
        {
            throw new NotImplementedException();
        }
        public Task<Attachment> GetFirstAsync(Expression<Func<Attachment, bool>> filter = null, Func<IQueryable<Attachment>, IOrderedQueryable<Attachment>> orderBy = null, string includeProperties = null, bool detached = false)
        {
            throw new NotImplementedException();
        } 
        #endregion

        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.Settings != null;
            isValid = isValid && !string.IsNullOrWhiteSpace(this.Settings?.BaseDirectoryPath);
            isValid = isValid && !string.IsNullOrWhiteSpace(this.Settings?.TempDirectoryPath);
            isValid = isValid && InitializeFileSystem();

            return isValid;
        }
        private bool InitializeFileSystem()
        {
            var BaseDirectoryExists = Directory.Exists(this.Settings?.BaseDirectoryPath);
            var TempDirectoryExists = Directory.Exists(this.Settings?.TempDirectoryPath);

            if (!(BaseDirectoryExists || TempDirectoryExists) && !this.Settings.CreateDirectoryIfNotFound) return false;

            this.BaseDirectory = Directory.CreateDirectory(this.Settings?.BaseDirectoryPath);
            this.TempDirectory = Directory.CreateDirectory(this.Settings?.TempDirectoryPath);

            return this.BaseDirectory.Exists && this.TempDirectory.Exists;
        }
        private string GetAttechmentDirectory(DateTime date, bool isTempFile = false)
        {
            string Directory = isTempFile ? this.TempDirectory.FullName : BaseDirectory.FullName;
            return $"{Directory}/{date.Year}/{date.Month.ToString().PadLeft(2, '0')}/{date.Day.ToString().PadLeft(2, '0')}/";
        }
        private string GetAttechmentDirectory(string id, bool isTemp = false)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            //int length = id.IndexOf(".", id.IndexOf(".") + 1);
            int length = id.IndexOf(".", 0);
            string prefix = id.Substring(0, length);
            string year = prefix.Substring(0, 4);
            string month = prefix.Substring(4, 2);
            string day = prefix.Substring(6, 2);

            string Directory = isTemp ? this.TempDirectory.FullName : this.BaseDirectory.FullName;
            return $"{Directory}/{year}/{month}/{day}/";
        }
        private async Task<string> GetFile(string id, bool isTemp = false)
        {
            string directory = GetAttechmentDirectory(id, isTemp);
            if (string.IsNullOrWhiteSpace(directory)) return null;

            var files = await Task.Run(() => Directory.GetFiles(directory, $"{id}*"));
            if (files.Length != 1)
            {
                //throw new Exception("more than one file found matching that id");
                return null;
            }
            if (files.Length == 0)
            {
                //throw new Exception("File Not Found");
                return null;
            }

            return files[0];
        }
        private async Task<List<string>> GetAllFile(List<string> ids, bool isTemp = false)
        {
            List<string> files = new List<string>();
            foreach (var id in ids)
            {
                string directory = GetAttechmentDirectory(id, isTemp);
                if (string.IsNullOrWhiteSpace(directory)) return null;
                try
                {
                    var file = await Task.Run(() => Directory.GetFiles(directory, $"{id}*"));
                    if (file.Length != 1)
                    {
                        //throw new Exception("more than one file found matching that id");
                        return null;
                    }
                    if (file.Length == 0)
                    {
                        //throw new Exception("File Not Found");
                        return null;
                    }
                    files.Add(file[0]);
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            return files;
        }
        private bool CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
            return true;
        }
        private string GetFileName(DateTime date, Attachment entity, out string prefix)
        {
            // prep.
            prefix = null;
            string fileName = entity?.Name?.Trim();
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrWhiteSpace(fileNameWithoutExt)) return null;

            // compose.
            prefix = $"{date.Year}{date.Month.ToString().PadLeft(2, '0')}{date.Day.ToString().PadLeft(2, '0')}.{DateTime.Now.Ticks}";
            fileName = $"{prefix}.{fileName}";

            // trim.
            Path.GetInvalidFileNameChars().ToList().ForEach(x => fileName = fileName.Replace(x, '_'));

            // ...
            return fileName;
        }
        private bool CheckIfTemp(Attachment Atachment)
        {
            if (Atachment.Status == AttachmentStatus.MarkedForAddtion)
                return true;
            else
                return false;
        }
        public string GetContentType(string path)
        {
            var fileProvider = new FileExtensionContentTypeProvider();
            if (!fileProvider.TryGetContentType(Path.GetFileName(path), out string contentType))
            {
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {Path.GetFileName(path)}.");
            }
            return contentType;
        }
        #endregion
    }
}
