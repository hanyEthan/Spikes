using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Organizations.Core.Models.Support
{
     public class OrganizationKey
    {
        public int DepartmentID { get; set; }
        public int ContactInfoID { get; set; }
        public int ContactPersonalID { get; set; }
        public int SettingsID { get; set; }
        public string Key { get; set; }

    }
}
