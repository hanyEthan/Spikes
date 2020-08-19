using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.Models.DTO.Essential.Personnels;
using XCore.Services.Personnel.API.Mappers.Departments;
using XCore.Services.Personnel.API.Mappers.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Personnels
{
    public class PersonnelMapper : PersonnelEssentialMapper<Person, PersonnelDTO>
    {
        #region props.

        public static PersonnelMapper Instance { get; } = new PersonnelMapper();

        #endregion
        #region cst.

        static PersonnelMapper()
        {
        }
        public PersonnelMapper()
        {
        }

        #endregion

        #region IModelMapper

        public override PersonnelDTO Map(Person from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.Manager = Map(from.Manager, metadata);
            to.Department = DepartmentMapper.Instance.Map(from.Department, metadata);
            to.Account = PersonnelAccountMapper.Instance.Map(from.Account, metadata);
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
        public override Person Map(PersonnelDTO from, object metadata = null)
        {
            if (from == null) return null;
            
            var to = base.Map(from, metadata);
            to.Manager = Map(from.Manager, metadata);
            to.Department = DepartmentMapper.Instance.Map(from.Department, metadata);
            to.Account = PersonnelAccountMapper.Instance.Map(from.Account, metadata);
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
