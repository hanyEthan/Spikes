using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.Role;
using XCore.Services.Organizations.API.Models.Venue;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class DepartmentMapper : IModelMapper<DepartmentDTO, Department>,
                                    IModelMapper<Department, DepartmentDTO>
    {
        #region props.

        public static DepartmentMapper Instance { get; } = new DepartmentMapper();
        public static RoleMapper RoleMapper { get; } = new RoleMapper();
        public static VenueMapper VenueMapper { get; } = new VenueMapper();


        #endregion
        #region IModelMapper

        public Department Map(DepartmentDTO from, object metadata = null)
        {
            if (from == null) return null;
            var to = new Department();

            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.Id = from.Id;
            to.ParentDepartmentId = from.ParentDepartmentId;
            to.SubDepartments = Map(from.SubDepartments);
            to.Description = from.Description;
            to.OrganizationId = from.OrganizationId;

            return to;
        }
        public DepartmentDTO Map(Department from, object metadata = null)
        {
            if (from == null) return null;

            var to = new DepartmentDTO();
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.Id = from.Id;
            to.ParentDepartmentId = from.ParentDepartmentId;
            to.SubDepartments = Map(from.SubDepartments);
            to.VenuesDTO = Map(from.Venues);
            to.RolesDTO = Map(from.Roles);
            to.Description = from.Description;
            to.OrganizationId = from.OrganizationId;


            return to;
        }

        #endregion
        #region Helpers
        public List<Department> Map(List<DepartmentDTO> from)
        {
            if (from == null) return null;

            var to = new List<Department>();

            foreach (var item in from)
            {

                var toItem = Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        public List<DepartmentDTO> Map(List<Department> from)
        {
            if (from == null) return null;

            var to = new List<DepartmentDTO>();

            foreach (var item in from)
            {

                var toItem = Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<RoleDTO> Map(IList<DepartmentRole> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<RoleDTO>();
            foreach (var item in from)
            {
                var role = RoleMapper.Map(item.Role);
                to.Add(role);
            }

            return to;
        }
        public List<VenueDTO> Map(IList<VenueDepartment> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<VenueDTO>();
            foreach (var item in from)
            {
                var venue = VenueMapper.Map(item.Venue);
                to.Add(venue);
            }

            return to;
        }

        #endregion
    }
}
