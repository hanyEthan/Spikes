using System;
using System.Collections.Generic;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Audit.API.Models;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.API.Mappers
{
    public class AuditGetResponseMapper : IModelMapper<SearchResults<AuditTrail>, SearchResultsDTO<AuditTrailDTO>>
    {
        #region props.

        public static AuditGetResponseMapper Instance { get; } = new AuditGetResponseMapper();

        #endregion
        #region cst.

        static AuditGetResponseMapper()
        {
        }
        public AuditGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<AuditTrail> Map(SearchResultsDTO<AuditTrailDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<AuditTrailDTO> Map(SearchResults<AuditTrail> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<AuditTrailDTO>();

            to.PageIndex = from.PageIndex;
            to.TotalCount = from.TotalCount;
            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<AuditTrailDTO> Map(List<AuditTrail> from)
        {
            if (from == null) return null;

            var to = new List<AuditTrailDTO>();

            foreach (var fromItem in from)
            {
                var toItem = Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private AuditTrailDTO Map(AuditTrail from)
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

        #endregion
    }
}
