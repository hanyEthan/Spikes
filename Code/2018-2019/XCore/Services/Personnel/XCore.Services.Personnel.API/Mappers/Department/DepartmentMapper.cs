using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;

namespace XCore.Services.Personnel.API.Mappers.Departments
{
    public class DepartmentMapper : DepartmentEssentialMapper<Department, DepartmentDTO>
     
    {
        #region props.

        public static DepartmentMapper Instance { get; } = new DepartmentMapper();

        #endregion
        #region cst.

        static DepartmentMapper()
        {
        }
        public DepartmentMapper()
        {
        }

        #endregion

        #region IModelMapper

        public override DepartmentDTO Map(Department from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.HeadDepartment = DepartmentMapper.Instance.Map(from.HeadDepartment, metadata);
            to.AppId = from.AppId;
            to.ModuleId = from.ModuleId;

            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }
        public override Department Map(DepartmentDTO from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.HeadDepartment = DepartmentMapper.Instance.Map(from.HeadDepartment, metadata);
            to.AppId = from.AppId;
            to.ModuleId = from.ModuleId;
            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }

        #endregion
    }
}
