using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.API.Mappers
{
    public class MessageTemplateKeyMapper : IModelMapper<MessageTemplateKeyDTO, MessageTemplateKey>,
                                         IModelMapper<MessageTemplateKey, MessageTemplateKeyDTO>
    {
        public static MessageTemplateKeyMapper Instance { get; } = new MessageTemplateKeyMapper();

        public MessageTemplateKeyDTO Map(MessageTemplateKey from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateKeyDTO();

            to.Id = from.Id;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);

            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);
            
            to.Key = from.Key;
            to.Description = from.Description;
            to.MessageTemplateId = from.MessageTemplateId;
            //to.MessageTemplate = MessageTemplateMapper.Instance.Map(from.MessageTemplate, metadata = null),

            return to;
        }
        public MessageTemplateKey Map(MessageTemplateKeyDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateKey();

            to.Id = from.Id;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);
            to.IsActive = from.IsActive ?? true;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);
            to.Key = from.Key;
            to.Description = from.Description;
            to.MessageTemplateId = from.MessageTemplateId;
            //to.MessageTemplate= MessageTemplateMapper.Instance.Map(from.MessageTemplate, metadata = null),

            return to;
        }
    }
}
