using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Docs.API.Models;
using XCore.Services.Docs.Core.Models;
using static XCore.Services.Docs.Core.Models.DocumentSearchCriteria;

namespace XCore.Services.Docs.API.Mappers
{
    public class DocumentGetRequestMapper : IModelMapper<DocumentSearchCriteria, DocumentSearchCriteriaDTO>
    {
        #region props.

        public static DocumentGetRequestMapper Instance { get; } = new DocumentGetRequestMapper();

        #endregion
        #region cst.

        static DocumentGetRequestMapper()
        {
        }
        public DocumentGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public DocumentSearchCriteria Map(DocumentSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new DocumentSearchCriteria();

            to.Id = from.Id;
            to.Actions = from.Actions;
            to.Apps = from.Apps;
            to.Modules = from.Modules;
            to.Entities = from.Entities;
            to.AttachId = from.AttachId;
            to.Category = from.Category;
            to.UserIds = from.UserIds;
            to.UserNames = from.UserNames;

           

            return to;
        }
        public DocumentSearchCriteriaDTO Map(DocumentSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
