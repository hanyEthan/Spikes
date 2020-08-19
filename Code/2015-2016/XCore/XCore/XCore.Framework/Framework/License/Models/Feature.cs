using System;
using System.Collections.Generic;
using XCore.Framework.Framework.License.Contracts;
using XCore.Utilities.Logger;

namespace XCore.Framework.Framework.License.Models
{
    public class Feature : ILicenseValidator
    {
        #region props.

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ILicenseValidator> LicenseValidators { get; set; }

        public int? Quota { get; set; }
        public IDataProvider DataProvider { get; set; }

        #endregion
        #region cst.

        public Feature()
        {
            this.LicenseValidators = new List<ILicenseValidator>();
        }

        #endregion
        #region publics.

        public bool IsValid()
        {
            return IsValid(0);
        }
        public bool IsValid(int count)
        {
            try
            {
                var existingCount = this.DataProvider == null
                                  ? this.Quota.HasValue ? this.Quota.Value : 0
                                  : this.Quota.HasValue ? int.Parse(this.DataProvider.getInfo().ToString()) : 0;
                var extraCount = count;
                var quota = this.Quota.HasValue ? this.Quota.Value : existingCount + count;

                return existingCount + extraCount <= quota;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        
        #endregion
    }
}
