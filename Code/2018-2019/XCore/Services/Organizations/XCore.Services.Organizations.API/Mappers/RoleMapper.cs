using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.ContactInfo;
using XCore.Services.Organizations.API.Models.ContactPersonal;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.Role;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.API.Models.Settings;
using XCore.Services.Organizations.Core.Models.Domain;


namespace XCore.Services.Organizations.API.Mappers
{
    public class RoleMapper : IModelMapper<RoleDTO, Role>,
                             IModelMapper<Role, RoleDTO>
    {
        #region props.

        public static RoleMapper Instance { get; } = new RoleMapper();
        public static DepartmentMapper DepartmentMapper { get; } = new DepartmentMapper();
       



        #endregion
        #region IModelMapper

        public Role Map(RoleDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Role()
            {   
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat) ,
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                Description =from.Description,
               
              
            };

            return to;
        }
        public RoleDTO Map(Role from, object metadata = null)
        {
            if (from == null) return null;

            var to = new RoleDTO() { };

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
            to.Description = from.Description;
            to.DepartmentsDTO = Map(from.Departments);

          

            return to;
        }

        #region Helpers.

        public List<DepartmentDTO> Map(IList<DepartmentRole> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<DepartmentDTO>();
            foreach (var item in from)
            {
                var department = DepartmentMapper.Map(item.Department);
                to.Add(department);
            }




            return to;
        }











        #endregion
        #endregion

    }
}
