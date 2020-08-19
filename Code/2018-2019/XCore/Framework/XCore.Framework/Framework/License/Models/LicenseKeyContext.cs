namespace XCore.Framework.Framework.License.Models
{
    public class LicenseKeyContext
    {
        #region props.

        public LicenseKeyContextMode Mode { get; set; }
        public string LicenseKey { get; set; }

        #endregion
        #region statics.

        public static LicenseKeyContext Default = new LicenseKeyContext()
        {
            Mode = LicenseKeyContextMode.Configurations ,
        };

        #endregion
    }
}
