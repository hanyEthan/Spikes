using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.API.Mappers
{
    public class MessageTemplateKeyValueMapper : IModelMapper<MessageTemplateKeyValueDTO, MessageTemplateKeyValue>,
                                         IModelMapper<MessageTemplateKeyValue, MessageTemplateKeyValueDTO>
    {

        public static MessageTemplateKeyValueMapper Instance { get; } = new MessageTemplateKeyValueMapper();




        public MessageTemplateKeyValue Map(MessageTemplateKeyValueDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateKeyValue();

            to.Key = from.Key;
            to.Value = from.Value;
            return to;
        }

        public MessageTemplateKeyValueDTO Map(MessageTemplateKeyValue from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateKeyValueDTO();

            to.Key = from.Key;
            to.Value = from.Value;
            return to;
        }
    }
    }

