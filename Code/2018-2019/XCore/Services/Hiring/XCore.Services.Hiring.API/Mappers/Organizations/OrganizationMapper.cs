using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.HiringProcesses;
using XCore.Services.Hiring.API.Mappers.Roles;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Organizations
{
    public class OrganizationMapper : IModelMapper<Organization, OrganizationDTO>
    {
        #region props.

        public static OrganizationMapper Instance { get; } = new OrganizationMapper();

        #endregion

        #region IModelMapper

        public OrganizationDTO Map(Organization from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationDTO
            {
                AppId = from.AppId,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                ModuleId = from.ModuleId,
                 Name = from.Name,
                OrganizationReferenceId = from.OrganizationReferenceId,
                NameCultured = from.NameCultured,

                HiringProcesses = Map(from.HiringProcesses),
                Roles = Map(from.Roles),                
                
            };

            return to;
        }
        public Organization Map(OrganizationDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Organization {
                AppId = from.AppId,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                ModuleId = from.ModuleId,
                Name = from.Name,
                OrganizationReferenceId = from.OrganizationReferenceId,
                NameCultured = from.NameCultured,

                HiringProcesses = Map(from.HiringProcesses),
                Roles = Map(from.Roles),        
                
            };
            return to;
        }

        #endregion

        #region HiringProcess
        private IList<HiringProcessDTO> Map(IList<HiringProcess> from)
        {
            if (from == null) return null;

            var to = new List<HiringProcessDTO>();

            foreach (var fromItem in from)
            {
                var toItem = HiringProcessMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }        
        private IList<HiringProcess> Map(IList<HiringProcessDTO> from)
        {
            if (from == null) return null;

            var to = new List<HiringProcess>();

            foreach (var fromItem in from)
            {
                var toItem = HiringProcessMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        #endregion
        #region Role
        private IList<Role> Map(IList<RoleDTO> from)
        {
            if (from == null) return null;

            var to = new List<Role>();

            foreach (var fromItem in from)
            {
                var toItem = RoleMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private IList<RoleDTO> Map(IList<Role> from)
        {
            if (from == null) return null;

            var to = new List<RoleDTO>();

            foreach (var fromItem in from)
            {
                var toItem = RoleMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        } 
        #endregion
        
    }
}
