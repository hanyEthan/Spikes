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
    public class RoleMapper : IModelMapper<RoleDTO, Role>,
                              IModelMapper<Role, RoleDTO>
    {
        #region props.

        public static RoleMapper Instance { get; } = new RoleMapper();
        private PrivilegeMapper PrivilegeMapper = PrivilegeMapper.Instance;
        private ActorMapper ActorMapper = ActorMapper.Instance;


        #endregion
        #region IModelMapper

        public Role Map(RoleDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Role()
            {
                Id = from.Id,
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
                Code = from.Code,
                Privileges = Map(from, from.Privileges),
                //Actors = Map(from, from.Actors)
            };

            return to;
        }
        public RoleDTO Map(Role from, object metadata = null)
        {
            if (from == null) return null;

            var to = new RoleDTO()
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
                //Actors = Map(from.Actors)
            };

            return to;
        }



        #endregion
        #region helpers.
        
        

        public List<RolePrivilege> Map(RoleDTO fromRole, List<PrivilegeDTO> fromPrivileges)
        {
            if (fromRole == null || fromPrivileges == null) return null;

            var to = new List<RolePrivilege>();
            foreach (var fromPrivilegeItem in fromPrivileges)
            {
                var toItem = Map(fromRole, fromPrivilegeItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        public List<ActorRole> Map(RoleDTO fromRole, List<ActorDTO> fromActors)
        {
            if (fromRole == null || fromActors == null) return null;

            var to = new List<ActorRole>();
            foreach (var fromActorItem in fromActors)
            {
                var toItem = Map(fromRole, fromActorItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private RolePrivilege Map(RoleDTO fromRole, PrivilegeDTO from)
        {
            if (from == null) return null;

            var to = new RolePrivilege();
            to.PrivilegeId = from.Id;
            to.RoleId = fromRole.Id;

            return to;
        }
        private ActorRole Map(RoleDTO fromRole, ActorDTO from)
        {
            if (from == null) return null;

            var to = new ActorRole();
            to.ActorId = from.Id;
            to.RoleId = fromRole.Id;

            return to;
        }
        private List<ActorDTO> Map(IList<ActorRole> from)
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
        private List<PrivilegeDTO> Map(IList<RolePrivilege> from)
        {
            if (from == null ) return null;

            var to = new List<PrivilegeDTO>();
            foreach (var fromItem in from)
            {
                var toItem = Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private PrivilegeDTO Map(RolePrivilege from)
        {
            if (from == null) return null;

            var to = this.PrivilegeMapper.Map(from.Privilege) ?? new PrivilegeDTO() { Id = from.PrivilegeId };

            return to;
        }
        private ActorDTO Map(ActorRole from)
        {
            if (from == null) return null;

            var to = this.ActorMapper.Map(from.Actor) ?? new ActorDTO() { Id = from.ActorId };

            return to;
        }
        #endregion
    }
}
