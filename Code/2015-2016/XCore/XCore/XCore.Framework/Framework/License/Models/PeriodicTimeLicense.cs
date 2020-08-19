using System;
using System.Collections.Generic;
using XCore.Framework.Framework.License.Contracts;

namespace XCore.Framework.Framework.License.Models
{
    public class PeriodicTimeLicense : ITimeLicense
    {
        #region props.

        private Guid _Id = new Guid("815B276D-71D7-4591-B737-1A932A6F159A");
        public Guid Id { get { return _Id; } set { } }

        public TimeSpan Period { get; set; }
        public DateTime StartDate { get; set; }
        public List<ILicenseValidator> LicenseValidators { get; set; }

        #endregion
        #region cst.

        public PeriodicTimeLicense()
        {
            this.Period = new TimeSpan();
            this.StartDate = new DateTime();
            this.LicenseValidators = new List<ILicenseValidator>();
        }

        #endregion
        #region publics.

        public bool IsValid()
        {
            return this.StartDate.Add(this.Period) >= DateTime.Now;
        }
        public bool IsValid(int count)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
