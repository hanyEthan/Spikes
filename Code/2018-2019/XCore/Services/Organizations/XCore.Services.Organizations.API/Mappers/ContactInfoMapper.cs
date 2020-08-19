using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.ContactInfo;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class ContactInfoMapper : IModelMapper<ContactInfoDTO, ContactInfo>,
                                     IModelMapper<ContactInfo, ContactInfoDTO>
    {
        #region props.

        public static ContactInfoMapper Instance { get; } = new ContactInfoMapper();

        #endregion
        #region IModelMapper

        public ContactInfo Map(ContactInfoDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ContactInfo()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                Id = from.Id,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Phone = from.Phone,
                PostalCode = from.PostalCode,
                Description = from.Description,
                Email = from.Email,
                Fax = from.Fax,
                Address = from.Address,
                OrganizationId = from.OrganizationId,
            };

            return to;
        }
        public ContactInfoDTO Map(ContactInfo from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ContactInfoDTO()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Phone = from.Phone,
                Mobile = from.Mobile,
                PostalCode = from.PostalCode,
                Address = from.Address,
                OrganizationId = from.OrganizationId,
                Description = from.Description,
                Email = from.Email,
                Fax = from.Fax,
            };

            return to;
        }

        #endregion
    }
}
