using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Docs.API.Models;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.API.Mappers
{
    public class DocumentMapper : IModelMapper<Document, DocumentDTO>,
                                  IModelMapper<DocumentDTO, Document>
    {
        #region props.

        public static DocumentMapper Instance { get; } = new DocumentMapper();

        #endregion
        #region cst.

        static DocumentMapper()
        {
        }
        public DocumentMapper()
        {
        }

        #endregion

        #region IModelMapper

        public DocumentDTO Map(Document from, object metadata = null)
        {
            if (from == null) return null;

            var to = new DocumentDTO();
            to.Id = from.Id;
            to.Action = from.Action;
            to.App = from.App;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Entity = from.Entity;
            to.MetaData = from.MetaData;
            to.AttachId = from.AttachId;
            to.Category = from.Category;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Module = from.Module;
            to.UserId = from.UserId;
            to.UserName = from.UserName;
            

            return to;
        }
        public Document Map(DocumentDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Document();
            to.Id = from.Id;
            to.Action = from.Action;
            to.App = from.App;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            
            to.Entity = from.Entity;
            to.MetaData = from.MetaData;
            to.AttachId = from.AttachId;
            to.Category = from.Category;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Module = from.Module;
           
            to.UserId = from.UserId;
            to.UserName = from.UserName;

            return to;
        }

        #endregion
    }
}
