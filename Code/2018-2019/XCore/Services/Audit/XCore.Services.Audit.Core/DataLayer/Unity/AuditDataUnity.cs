using System;
using System.Threading.Tasks;
using XCore.Services.Audit.Core.DataLayer.Context;
using XCore.Services.Audit.Core.DataLayer.Contracts;

namespace XCore.Services.Audit.Core.DataLayer.Unity
{
    public class AuditDataUnity : IAuditDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        private readonly AuditDataContext _DataContext;

        public IAuditRepository Audit { get; private set; }
        public IAuditReadRepository AuditRead { get; private set; }
        #endregion
        #region cst.

        public AuditDataUnity(AuditDataContext dataContext, 
                              IAuditRepository auditRepository , 
                              IAuditReadRepository auditReadRepository)
        {
            this._DataContext = dataContext;
            this.Audit = auditRepository;
            this.AuditRead = auditReadRepository;
            this.Initialized = Initialize();
        }

        #endregion

        #region publics

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
            isValid = isValid && (this.Audit?.Initialized.GetValueOrDefault() ?? false);
            isValid = isValid && (this.AuditRead?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
