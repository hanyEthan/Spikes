using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Audit.API.Models;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.API.Mappers
{
    public class AuditCreateResponseMapper : IModelMapper<AuditTrail, bool>
    {
        #region props.

        public static AuditCreateResponseMapper Instance { get; } = new AuditCreateResponseMapper();

        #endregion
        #region cst.

        static AuditCreateResponseMapper()
        {
        }
        public AuditCreateResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public bool Map(AuditTrail from, object metadata = null)
        {
            return from != null;
        }
        public AuditTrail Map(bool from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
