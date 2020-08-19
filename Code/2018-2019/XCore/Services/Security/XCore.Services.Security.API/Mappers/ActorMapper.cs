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
    public class ActorMapper : IModelMapper<ActorDTO, Actor>,
                               IModelMapper<Actor, ActorDTO>
    {
        public static ActorMapper Instance { get; } = new ActorMapper();
        private PrivilegeMapper PrivilegeMapper = PrivilegeMapper.Instance;
        private RoleMapper RoleMapper = RoleMapper.Instance;
        #region IModelMapper

        public Actor Map(ActorDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Actor()
            {
                Id = from.Id,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateFormat),
                Description = from.Description,
                IsActive = from.IsActive ?? true,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                AppId = from.AppId,
                Privileges = Map(from, from.Privileges),
                Roles = Map(from, from.Roles)
            };

            return to;
        }
        public ActorDTO Map(Actor from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ActorDTO()
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
                Name = from.Name,
                NameCultured = from.NameCultured,
                AppId = from.AppId,
                Privileges = Map(from.Privileges),
                Roles = Map(from.Roles)


            };

            return to;
        }

        #endregion
        #region helpers.



        public List<ActorPrivilege> Map(ActorDTO fromActor, List<PrivilegeDTO> fromPrivileges)
        {
            if (fromActor == null || fromPrivileges == null) return null;

            var to = new List<ActorPrivilege>();
            foreach (var fromPrivilegeItem in fromPrivileges)
            {
                var toItem = Map(fromActor, fromPrivilegeItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        public List<ActorRole> Map(ActorDTO fromActor, List<RoleDTO> fromRoles)
        {
            if (fromActor == null || fromRoles == null) return null;

            var to = new List<ActorRole>();
            foreach (var fromActorItem in fromRoles)
            {
                var toItem = Map(fromActor, fromActorItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private ActorPrivilege Map(ActorDTO fromActor, PrivilegeDTO from)
        {
            if (from == null) return null;

            var to = new ActorPrivilege();
            to.PrivilegeId = from.Id;
            to.ActorId = fromActor.Id;

            return to;
        }
        private ActorRole Map(ActorDTO fromActor, RoleDTO from)
        {
            if (from == null) return null;

            var to = new ActorRole();
            to.RoleId = from.Id;
            to.ActorId = fromActor.Id;

            return to;
        }
        private List<RoleDTO> Map(IList<ActorRole> from)
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
        private List<PrivilegeDTO> Map(IList<ActorPrivilege> from)
        {
            if (from == null) return null;

            var to = new List<PrivilegeDTO>();
            foreach (var fromItem in from)
            {
                var toItem = Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private PrivilegeDTO Map(ActorPrivilege from)
        {
            if (from == null) return null;

            var to = this.PrivilegeMapper.Map(from.Privilege) ?? new PrivilegeDTO() { Id = from.PrivilegeId };

            return to;
        }
        private RoleDTO Map(ActorRole from)
        {
            if (from == null) return null;

            var to = this.RoleMapper.Map(from.Role) ?? new RoleDTO() { Id = from.RoleId };

            return to;
        }
        #endregion
    }
}
