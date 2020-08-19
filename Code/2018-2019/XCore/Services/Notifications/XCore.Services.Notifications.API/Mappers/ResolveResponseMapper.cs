using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.API.Mappers
{
    public class ResolveResponseMapper : IModelMapper<ResolveResponseDTO, ResolveResponse>,
                                         IModelMapper<ResolveResponse, ResolveResponseDTO>
    {

        public static ResolveResponseMapper Instance { get; } = new ResolveResponseMapper();

        public ResolveResponseDTO Map(ResolveResponse from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ResolveResponseDTO();

            to.RequestId = from.RequestId;
            to.MessageTemplateId = from.MessageTemplateId;
            to.Result = from.Result;
            
            return to;
        }

        public ResolveResponse Map(ResolveResponseDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ResolveResponse();

            to.RequestId = from.RequestId;
            to.MessageTemplateId = from.MessageTemplateId;
            to.Result = from.Result;


            return to;
        }
    }
}
