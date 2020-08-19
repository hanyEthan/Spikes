using System;
using System.Threading.Tasks;
using XCore.Services.Attachments.Core.DataLayer.Context;
using XCore.Services.Attachments.Core.DataLayer.Contracts;

namespace XCore.Services.Attachments.Core.DataLayer.Unity
{
    public class AttachmentDataUnity : IAttachmentDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        public bool? IsContextRequired { get; protected set; }
        
        private readonly AttachmentDataContext _DataContext;

        public IAttachmentRepository Attachments { get; private set; }

        #endregion
        #region cst.

        public AttachmentDataUnity(IAttachmentRepository attachmentsRepository)
        {
            this.IsContextRequired = false;
            this.Attachments = attachmentsRepository;
            this.Initialized = Initialize();
        }
        public AttachmentDataUnity(AttachmentDataContext dataContext, IAttachmentRepository attachmentsRepository)
        {
            this.IsContextRequired = true;
            this._DataContext = dataContext;
            this.Attachments = attachmentsRepository;
            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public void Save()
        {
            this._DataContext.SaveChanges();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && ( !this.IsContextRequired.GetValueOrDefault() || this._DataContext != null);
            isValid = isValid && (this.Attachments?.Initialized.GetValueOrDefault() ?? false );

            return isValid;
        }

        public async Task SaveAsync()
        {
            if (!this.IsContextRequired.GetValueOrDefault())
                return;
            await this._DataContext.SaveChangesAsync();
        }

        #endregion
    }
}
