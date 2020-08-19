using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.API.Mappers
{
    public class DocumentMapper : IModelMapper<MessageTemplateAttachment, DocumentDTO>,
                                  IModelMapper<DocumentDTO, MessageTemplateAttachment>
    {
        #region props.

        public static DocumentMapper Instance { get; } = new DocumentMapper();

        #endregion
        #region IModelMapper
        public DocumentDTO Map(MessageTemplateAttachment from, object metadata = null)
        {
            if (from == null) return null;

            var to = new DocumentDTO();

            to.Id = from.Id;
            to.Action = from.Action;
            to.App = from.App;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.MetaData = from.MetaData;
            to.Category = from.Category;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Module = from.Module;
            to.UserId = from.UserId;
            to.UserName = from.UserName;

            to.DocumentReferenceId = from.DocumentReferenceId;
            to.attachId = from.AttachmentReferenceId;
            to.Entity = from.MessageTemplateId.ToString();

            return to;
        }
         public MessageTemplateAttachment Map(DocumentDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateAttachment();
            to.Id = from.Id;
            to.Action = from.Action;
            to.App = from.App;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.MetaData = from.MetaData;
            to.Category = from.Category;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Module = from.Module;
            to.UserId = from.UserId;
            to.UserName = from.UserName;
            
            to.DocumentReferenceId = from.DocumentReferenceId;
            to.AttachmentReferenceId = from.attachId;
            to.MessageTemplateId = int.TryParse(from.Entity , out int messageTemplateId) ? messageTemplateId : 0;

            return to;
        }

        #endregion
    }
}
