using System;

namespace ADS.Tamam.Common.Data.Model.Domain.PersonnelPrivileges
{
    public class PersonnelPrivilege
    {
        public Guid PersonId { set; get; }
        public string FullName { get; set; }
        public string FullNameCultureInvariant { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string PrivilegeName { get; set; }
        public string PrivilegeDescription { get; set; }
    }
}
