using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.API.Mappers.Personnels;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class PersonnelAccountMapper : PersonnelAccountEssentialMapper<PersonnelAccount, PersonnelAccountDTO>
    {
        #region props.

        public static PersonnelAccountMapper Instance { get; } = new PersonnelAccountMapper();

        #endregion
        #region cst.

        static PersonnelAccountMapper()
        {
        }
        public PersonnelAccountMapper()
        {
        }

        #endregion
        #region IModelMapper

        public override PersonnelAccountDTO Map(PersonnelAccount from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
           // to.Person = PersonnelMapper.Instance.Map(from.Person, metadata);
            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }

        public override PersonnelAccount Map(PersonnelAccountDTO from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
           // to.Person = PersonnelMapper.Instance.Map(from.Person, metadata);
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
