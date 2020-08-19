using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.API.Mappers
{
    public class ResolveRequestMapper : IModelMapper<ResolveRequestDTO, ResolveRequest>,
                                         IModelMapper<ResolveRequest, ResolveRequestDTO>
    {

        public static ResolveRequestMapper Instance { get; } = new ResolveRequestMapper();



        #region IModelMapper
        public ResolveRequest Map(ResolveRequestDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ResolveRequest();

            to.RequestId = from.RequestId;
            to.MessageTemplateId = from.MessageTemplateId;
            to.RequestId = from.RequestId;
            to.Values = Map(from.Values);
            return to;
        }

        public ResolveRequestDTO Map(ResolveRequest from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ResolveRequestDTO();

            to.RequestId = from.RequestId;
            to.MessageTemplateId = from.MessageTemplateId;
            to.RequestId = from.RequestId;
            to.Values = Map(from.Values);
            return to;
        }

        #endregion


        #region helplers.

        private List<MessageTemplateKeyValueDTO> Map(List<MessageTemplateKeyValue> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<MessageTemplateKeyValueDTO>();

            foreach (var item in from)
            {
                var toItem = MessageTemplateKeyValueMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        private List<MessageTemplateKeyValue> Map(List<MessageTemplateKeyValueDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<MessageTemplateKeyValue>();

            foreach (var item in from)
            {
                var toItem = MessageTemplateKeyValueMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
