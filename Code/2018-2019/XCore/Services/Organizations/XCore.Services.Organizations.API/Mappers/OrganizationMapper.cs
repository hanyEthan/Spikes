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
using XCore.Services.Organizations.API.Models.Organization;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.API.Models.Settings;
using XCore.Services.Organizations.Core.Models.Domain;


namespace XCore.Services.Organizations.API.Mappers
{
    public class OrganizationMapper : IModelMapper<OrganizationDTO, Organization>,
                             IModelMapper<Organization, OrganizationDTO>
    {
        #region props.

        public static OrganizationMapper Instance { get; } = new OrganizationMapper();
        public static OrganizationDelegationMapper OrganizationDelegationMapper { get; } = new OrganizationDelegationMapper();
        public static DepartmentMapper DepartmentMapper { get; } = new DepartmentMapper();
        public static SettingsMapper SettingsMapper { get; } = new SettingsMapper();
        public static ContactInfoMapper ContactInfoMapper { get; } = new ContactInfoMapper();
        public static ContactPersonalMapper ContactPersonalMapper { get; } = new ContactPersonalMapper();



        #endregion
        #region IModelMapper

        public Organization Map(OrganizationDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Organization()
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
                ParentOrganizationId=from.ParentOrganizationId,
                Settings = Map(from.Settings),
                SubOrganizations = Map(from.SubOrganizations),
                //OrganizationDelegates= MapDelegates(from, from.OrganizationDelegates),
                //OrganizationDelegators= MapDelegators(from,from.OrganizationDelegators),
               
                //OrganizationDelegates = Map(from.OrganizationDelegates),
                //OrganizationDelegators = Map(from.OrganizationDelegators),
                // ParentOrganization = Instance.Map(from.ParentOrganization),
                ContactInfo = Map(from.ContactInfo),
                ContactPersonnel = Map(from.ContactPersonnel),
                Departments = Map(from.Departments),
            };

            return to;
        }
        public OrganizationDTO Map(Organization from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationDTO() { };

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
            to.ParentOrganizationId = from.ParentOrganizationId;
            to.Settings = Map(from.Settings);
            to.SubOrganizations = Map(from.SubOrganizations);
           // to.OrganizationDelegates = Map(from.OrganizationDelegates);
           // to.OrganizationDelegators = Map(from.OrganizationDelegators);
           // to.ParentOrganization = Instance.Map(from.ParentOrganization);
            to.ContactInfo = Map(from.ContactInfo);
            to.ContactPersonnel = Map(from.ContactPersonnel);
           // to.Departments = Map(from.Departments);

            return to;
        }

        #region Helpers.

        public List<Organization> Map(List<OrganizationDTO> from)
        {
            if (from == null) return null;

            var to = new List<Organization>();

            foreach (var item in from)
            {
                var toItem = Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<OrganizationDTO> Map(List<Organization> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationDTO>();

            foreach (var item in from)
            {
                var toItem = Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }


        public List<OrganizationDelegation> Map(List<OrganizationDelegationDTO> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationDelegation>();

            foreach (var item in from)
            {
                var toItem = OrganizationDelegationMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<OrganizationDelegation> MapDelegates(OrganizationDTO FromOrganization,List<OrganizationDTO> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationDelegation>();

            foreach (var item in from)
            {
                var toItem = new OrganizationDelegation { DelegatorId = FromOrganization.Id, DelegateId = item.Id };
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<OrganizationDelegation> MapDelegators(OrganizationDTO FromOrganization,List<OrganizationDTO> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationDelegation>();

            foreach (var item in from)
            {
                var toItem = new OrganizationDelegation { DelegatorId = item.Id, DelegateId = FromOrganization.Id };
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<OrganizationDelegationDTO> Map(List<OrganizationDelegation> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationDelegationDTO>();

            foreach (var item in from)
            {
                var toItem = OrganizationDelegationMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }


        public List<Department> Map(List<DepartmentDTO> from)
        {
            if (from == null) return null;

            var to = new List<Department>();

            foreach (var item in from)
            {
                var toItem = DepartmentMapper.Map(item,item.Id);
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
                var toItem = DepartmentMapper.Map(item,item.Id);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }


        public List<Settings> Map(List<SettingsDTO> from)
        {
            if (from == null) return null;

            var to = new List<Settings>();

            foreach (var item in from)
            {
                var toItem = SettingsMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<SettingsDTO> Map(List<Settings> from)
        {
            if (from == null) return null;

            var to = new List<SettingsDTO>();

            foreach (var item in from)
            {
                var toItem = SettingsMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }


        public List<ContactInfo> Map(List<ContactInfoDTO> from)
        {
            if (from == null) return null;

            var to = new List<ContactInfo>();

            foreach (var item in from)
            {
                var toItem = ContactInfoMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<ContactInfoDTO> Map(List<ContactInfo> from)
        {
            if (from == null) return null;

            var to = new List<ContactInfoDTO>();

            foreach (var item in from)
            {
                var toItem = ContactInfoMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        public List<ContactPerson> Map(List<ContactPersonalDTO> from)
        {
            if (from == null) return null;

            var to = new List<ContactPerson>();

            foreach (var item in from)
            {
                var toItem = ContactPersonalMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        public List<ContactPersonalDTO> Map(List<ContactPerson> from)
        {
            if (from == null) return null;

            var to = new List<ContactPersonalDTO>();

            foreach (var item in from)
            {
                var toItem = ContactPersonalMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        #endregion
        #endregion

    }
}
