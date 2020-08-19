using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Audit.API.Models;
using XCore.Services.Audit.Core.Models;
using XCore.Services.Audit.Models;
using XCore.Services.Audit.Models.Enums;

namespace XCore.Services.Audit.API.Mappers
{
    public class AuditMapper : IModelMapper<AuditTrail, AuditTrailDTO>,
                               IModelMapper<AuditTrailDTO, AuditTrail>
    {
        #region props.

        public static AuditMapper Instance { get; } = new AuditMapper();

        #endregion
        #region cst.

        static AuditMapper()
        {
        }
        public AuditMapper()
        {
        }

        #endregion

        #region IModelMapper

        public AuditTrailDTO Map(AuditTrail from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AuditTrailDTO();

            to.Action = from.Action;
            to.App = from.App;
            to.Code = from.Code;
            to.ConnectionMethod = from.ConnectionMethod;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.DestinationAddress = from.DestinationAddress;
            to.DestinationIP = from.DestinationIP;
            to.DestinationPort = from.DestinationPort;
            to.Entity = from.Entity;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Module = from.Module;
            to.SourceClient = from.SourceClient;
            to.SourceIP = from.SourceIP;
            to.SourceOS = from.SourceOS;
            to.SourcePort = from.SourcePort;
            to.Text = from.Text;
            to.UserId = from.UserId;
            to.UserName = from.UserName;

            to.SyncStatus = from.SyncStatus;
            to.Level =  from.Level;
           

            return to;
        }
        public AuditTrail Map(AuditTrailDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AuditTrail();

            to.Action = from.Action;
            to.App = from.App;
            to.Code = from.Code;
            to.ConnectionMethod = from.ConnectionMethod;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.DestinationAddress = from.DestinationAddress;
            to.DestinationIP = from.DestinationIP;
            to.DestinationPort = from.DestinationPort;
            to.Entity = from.Entity;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Module = from.Module;
            to.SourceClient = from.SourceClient;
            to.SourceIP = from.SourceIP;
            to.SourceOS = from.SourceOS;
            to.SourcePort = from.SourcePort;
            to.Text = from.Text;
            to.UserId = from.UserId;
            to.UserName = from.UserName;
            to.SyncStatus =from.SyncStatus;
            to.Level =  from.Level;

            return to;
        }

        #endregion
    }
}
