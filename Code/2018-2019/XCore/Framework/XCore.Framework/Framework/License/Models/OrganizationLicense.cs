using System;
using System.Collections.Generic;
using XCore.Framework.Framework.License.Contracts;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.License.Models
{
    public class OrganizationLicense : ILicenseValidator
    {
        #region props.

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ILicenseValidator> LicenseValidators { get; set; }

        public string OrganizationInfo { get; set; }
        public IOrgDataProvider DataProvider { get; set; }

        #endregion
        #region cst.

        public OrganizationLicense(IOrgDataProvider dataProvider)
        {
            this.DataProvider = dataProvider;
            this.LicenseValidators = new List<ILicenseValidator>();
        }

        #endregion
        #region publics.

        public bool IsValid()
        {
            try
            {
                var existingOrganization = this.DataProvider == null
                                         ? null
                                         : this.DataProvider.getOrgInfo().ToString();
                var licensedOrganization = this.OrganizationInfo;

                return existingOrganization == licensedOrganization;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        public bool IsValid(int count)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
