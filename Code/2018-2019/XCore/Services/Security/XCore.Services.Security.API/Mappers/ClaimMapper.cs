using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Relations;

namespace XCore.Services.Security.API.Mappers
{
    public class ClaimMapper : IModelMapper<ClaimDTO, Claim>,
                              IModelMapper<Claim, ClaimDTO>
    {
        #region props.

        public static ClaimMapper Instance { get; } = new ClaimMapper();
        private RoleMapper RoleMapper = RoleMapper.Instance;
        private ActorMapper ActorMapper = ActorMapper.Instance;


        #endregion
        #region IModelMapper

        public Claim Map(ClaimDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Claim()
            {
                Id = from.Id,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateFormat),
                Description = from.Description,
                IsActive = from.IsActive ?? true,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateFormat),
                Key = from.Key,
                Value = from.Value,
                Map = from.Map,
                AppId = from.AppId,
                Code = from.Code,
            };

            return to;
        }
        public ClaimDTO Map(Claim from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ClaimDTO()
            {
                Id = from.Id,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat),
                Description = from.Description,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat),
                Key = from.Key,
                Value = from.Value,
                Map = from.Map,
                AppId = from.AppId
            };

            return to;
        }



        #endregion
        #region helpers.



        public List<RoleClaim> Map(ClaimDTO fromClaim, List<RoleDTO> fromRoles)
        {
            if (fromClaim == null || fromRoles == null) return null;

            var to = new List<RoleClaim>();
            foreach (var fromRoleItem in fromRoles)
            {
                var toItem = Map(fromClaim, fromRoleItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        public List<ActorClaim> Map(ClaimDTO fromClaim, List<ActorDTO> fromActors)
        {
            if (fromClaim == null || fromActors == null) return null;

            var to = new List<ActorClaim>();
            foreach (var fromActorItem in fromActors)
            {
                var toItem = Map(fromClaim, fromActorItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private RoleClaim Map(ClaimDTO fromClaim, RoleDTO from)
        {
            if (from == null) return null;

            var to = new RoleClaim();
            to.RoleId = from.Id;
            to.ClaimId = fromClaim.Id;

            return to;
        }
        private ActorClaim Map(ClaimDTO fromClaim, ActorDTO from)
        {
            if (from == null) return null;

            var to = new ActorClaim();
            to.ActorId = from.Id;
            to.ClaimId = fromClaim.Id;

            return to;
        }
        private List<ActorDTO> Map(IList<ActorClaim> from)
        {
            if (from == null) return null;

            var to = new List<ActorDTO>();
            foreach (var fromItem in from)
            {
                var toItem = Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private List<RoleDTO> Map(IList<RoleClaim> from)
        {
            if (from == null) return null;

            var to = new List<RoleDTO>();
            foreach (var fromItem in from)
            {
                var toItem = Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private RoleDTO Map(RoleClaim from)
        {
            if (from == null) return null;

            var to = this.RoleMapper.Map(from.Role) ?? new RoleDTO() { Id = from.RoleId };

            return to;
        }
        private ActorDTO Map(ActorClaim from)
        {
            if (from == null) return null;

            var to = this.ActorMapper.Map(from.Actor) ?? new ActorDTO() { Id = from.ActorId };

            return to;
        }
        #endregion
    }
}
