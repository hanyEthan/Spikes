using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.Organizations;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Roles
{
    public class RoleMapper : IModelMapper<Role, RoleDTO>
    {
        #region props.

        public static RoleMapper Instance { get; } = new RoleMapper();

        #endregion

        #region IModelMapper

        public RoleDTO Map(Role from, object metadata = null)
        {
            if (from == null) return null;

            var to = new RoleDTO
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                 Name = from.Name,
                NameCultured = from.NameCultured,
                //Organization = OrganizationMapper.Instance.Map(from.Organization),
                OrganizationId = from.OrganizationId,
                
            };

            return to;
        }
        public Role Map(RoleDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Role {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                Name = from.Name,
                NameCultured = from.NameCultured,
                //Organization = OrganizationMapper.Instance.Map(from.Organization),
                OrganizationId = from.OrganizationId,
            };
            return to;
        }

        #endregion
    }
}
