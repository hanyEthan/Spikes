using System.Collections.Generic;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.API.Mappers
{
    public class MessageTemplateMapper : IModelMapper<MessageTemplateDTO, MessageTemplate>,
                                         IModelMapper<MessageTemplate, MessageTemplateDTO>
    {
        public static MessageTemplateMapper Instance { get; } = new MessageTemplateMapper();

        #region IModelMapper

        public MessageTemplateDTO Map(MessageTemplate from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateDTO();

            to.Id = from.Id;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);

            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;

            to.AppId = from.AppId;
            to.ModuleId = from.ModuleId;
            to.Body = from.Body;
            to.Title = from.Title;
            to.Keys = Map(from.Keys);
            to.Attachments = Map(from.Attachments);

            return to;
        }
        public MessageTemplate Map(MessageTemplateDTO from, object metadata = null)
        {
            if (from == null) return null;
            var requestContext = metadata as ServiceExecutionRequest<MessageTemplate>;

            var to = new MessageTemplate();

            to.Id = from.Id;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);
            to.IsActive = from.IsActive ?? true;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.Code = from.Code;

            to.AppId = requestContext?.AppId ?? from?.AppId;
            to.ModuleId = requestContext?.ModuleId ?? from?.ModuleId;
            to.Body = from.Body;
            to.Title = from.Title;
            to.Keys = Map(from.Keys);
            to.Attachments = Map(from.Attachments);

            return to;
        }

        #endregion
        #region helplers.

        private List<MessageTemplateKeyDTO> Map(List<MessageTemplateKey> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<MessageTemplateKeyDTO>();

            foreach (var item in from)
            {
                var toItem = MessageTemplateKeyMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        private List<MessageTemplateKey> Map(List<MessageTemplateKeyDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<MessageTemplateKey>();

            foreach (var item in from)
            {
                var toItem = MessageTemplateKeyMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }


        private List<DocumentDTO> Map(List<MessageTemplateAttachment> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<DocumentDTO>();

            foreach (var item in from)
            {
                var toItem = DocumentMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        private List<MessageTemplateAttachment> Map(List<DocumentDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<MessageTemplateAttachment>();

            foreach (var item in from)
            {
                var toItem = DocumentMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
