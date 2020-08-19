using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.ContactPersonal;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class ContactPersonalMapper : IModelMapper<ContactPersonalDTO, ContactPerson>,
                                         IModelMapper<ContactPerson, ContactPersonalDTO>
    {
        #region props.

        public static ContactPersonalMapper Instance { get; } = new ContactPersonalMapper();

        #endregion
        #region IModelMapper

        public ContactPerson Map(ContactPersonalDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ContactPerson()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                PersonMobile=from.PersonMobile,
                PersonReferenceId=from.PersonReferenceId,
                PersonEmail=from.PersonEmail,
                Description = from.Description,
                OrganizationId = from.OrganizationId,
                IsActive = from.IsActive,
            };

            return to;
        }
        public ContactPersonalDTO Map(ContactPerson from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ContactPersonalDTO()
            {

                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                PersonMobile = from.PersonMobile,
                PersonReferenceId = from.PersonReferenceId,
                PersonEmail = from.PersonEmail,
                Description = from.Description,
                OrganizationId = from.OrganizationId,
            };

            return to;
        }

        #endregion
    }
}
