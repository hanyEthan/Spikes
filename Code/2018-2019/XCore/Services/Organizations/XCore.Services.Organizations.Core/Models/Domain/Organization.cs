
using System.Collections.Generic;
using System.Text.Json.Serialization;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class Organization : Entity<int>
    {
        #region Props.

        public string Description { get; set; }

        #endregion
        #region Conventions

        public virtual List<Department> Departments { get; set; }
        public virtual List<Settings> Settings { get; set; }
        public virtual List<ContactInfo> ContactInfo { get; set; }
        public virtual List<ContactPerson> ContactPersonnel { get; set; }
        public virtual List<Event> Events { get; set; }
        public virtual List<Venue> Venues { get; set; }
        public virtual List<OrganizationDelegation> OrganizationDelegates { get; set; }
        public virtual List<OrganizationDelegation> OrganizationDelegators { get; set; }
        public virtual int? ParentOrganizationId { get; set; }
        public virtual Organization ParentOrganization { get; set; }
        public virtual List<Organization> SubOrganizations { get; set; }

        #endregion
        #region cst.

        public Organization()
        {
            this.Departments = new List<Department>();
            this.Settings = new List<Settings>();
            this.ContactInfo = new List<ContactInfo>();
            this.ContactPersonnel = new List<ContactPerson>();
            this.Events = new List<Event>();
            this.Venues = new List<Venue>();
            this.OrganizationDelegates = new List<OrganizationDelegation>();
            this.OrganizationDelegators = new List<OrganizationDelegation>();
            this.SubOrganizations = new List<Organization>();
        }

        #endregion
    }
}
