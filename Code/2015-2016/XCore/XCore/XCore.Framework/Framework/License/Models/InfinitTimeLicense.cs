using System;
using System.Collections.Generic;
using XCore.Framework.Framework.License.Contracts;

namespace XCore.Framework.Framework.License.Models
{
    public class InfinitTimeLicense : ITimeLicense
    {
        #region props.

        private Guid _Id = new Guid("31C43388-2F49-4A38-8091-9ECB3F4146F4");
        public Guid Id { get { return _Id; } set {  } }

        public TimeSpan Period { get; set; }
        public DateTime StartDate { get; set; }
        public List<ILicenseValidator> LicenseValidators { get; set; }

        #endregion
        #region cst.

        public InfinitTimeLicense()
        {
            this.Period = new TimeSpan();
            this.StartDate = new DateTime();
            this.LicenseValidators = new List<ILicenseValidator>();
        }

        #endregion
        #region publics.

        public bool IsValid()
        {
            return true;
        }
        public bool IsValid(int count)
        {
            return true;
        }

        #endregion
    }
}
