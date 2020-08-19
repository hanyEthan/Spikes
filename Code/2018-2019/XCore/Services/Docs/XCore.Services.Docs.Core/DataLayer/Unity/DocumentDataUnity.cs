using System;
using System.Threading.Tasks;
using XCore.Services.Docs.Core.DataLayer.Context;
using XCore.Services.Docs.Core.DataLayer.Contracts;

namespace XCore.Services.Docs.Core.DataLayer.Unity
{
    public class DocumentDataUnity : IDocumentDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        private readonly DocumentDataContext _DataContext;

        public IDocumentRepository Document { get; private set; }

        #endregion
        #region cst.

        public DocumentDataUnity(DocumentDataContext dataContext, IDocumentRepository documentRepository)
        {
            this._DataContext = dataContext;
            this.Document = documentRepository;
            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public void Save()
        {
            this._DataContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await this._DataContext.SaveChangesAsync();
        }
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Document?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
