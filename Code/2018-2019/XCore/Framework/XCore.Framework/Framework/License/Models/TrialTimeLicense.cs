using System;
using System.Collections.Generic;
using XCore.Framework.Framework.License.Contracts;

namespace XCore.Framework.Framework.License.Models
{
    public class TrialTimeLicense : ITimeLicense
    {
        #region props.

        private Guid _Id = new Guid("CC2054C6-B452-4D04-8775-FD658AB302F5");
        public Guid Id { get { return _Id; } set { } }

        public TimeSpan Period { get; set; }
        public DateTime StartDate { get; set; }
        public List<ILicenseValidator> LicenseValidators { get; set; }

        #endregion
        #region cst.

        public TrialTimeLicense()
        {
            this.Period = new TimeSpan();
            this.StartDate = new DateTime();
            this.LicenseValidators = new List<ILicenseValidator>();
        }

        #endregion
        #region publics.

        public bool IsValid()
        {
            return this.StartDate.Add(this.Period) >= DateTime.UtcNow;
        }
        public bool IsValid(int count)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
