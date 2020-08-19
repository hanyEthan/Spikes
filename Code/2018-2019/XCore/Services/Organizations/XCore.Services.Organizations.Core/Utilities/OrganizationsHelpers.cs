using System.Collections.Generic;
using System.Linq;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.Core.Utilities
{
    public class OrganizationsHelpers
    {
        internal static bool MapUpdate(Organization existing, Organization updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Description;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.ParentOrganizationId = updated.ParentOrganizationId;

            existing.ContactInfo = updated.ContactInfo;
            existing.ContactPersonnel = updated.ContactPersonnel;

            return true;
        }
        internal static bool MapUpdate(Role existing, Role updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.Departments = updated.Departments;



            return true;
           
        }
        internal static bool MapUpdate(Event existing, Event updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            return true;

        }
        internal static bool MapUpdate(City existing, City updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            return true;
        }
        internal static bool MapUpdate(Venue existing, Venue updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            existing.EventId = updated.EventId;
            existing.Cities = updated.Cities;
            existing.Departments = updated.Departments;
            existing.ParentVenueId = updated.ParentVenueId;
            //existing.OrganizationId = updated.OrganizationId;
            //existing.EventId = updated.EventId;

            return true;
        }
        internal static bool MapUpdate(Department existing, Department updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Description;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            existing.OrganizationId = updated.OrganizationId;
            existing.ParentDepartmentId = updated.ParentDepartmentId;
            existing.Venues = updated.Venues;
            existing.Roles = updated.Roles;

            bool state = true;

            //state = MapUpdate(existing.ParentOrganization, updated.ParentOrganization) && state;             // don't map
            //state = MapUpdate(existing.ParentDepartment, updated.ParentDepartment) && state;                 // don't map
            //state = MapUpdate(existing.SubDepartments, updated.SubDepartments) && state;                     // don't map

            return state;
        }
        internal static bool MapUpdate(Settings existing, Settings updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Description;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.OrganizationId = updated.OrganizationId;

            bool state = true;

            return state;
        }
        internal static bool MapUpdate(OrganizationDelegation existing, OrganizationDelegation updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.DelegateId = updated.DelegateId;
            existing.DelegatorId = updated.DelegatorId;



            return true;
        }
    }
}
