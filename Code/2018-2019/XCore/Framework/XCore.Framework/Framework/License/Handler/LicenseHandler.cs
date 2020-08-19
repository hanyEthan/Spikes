using System;
using System.Threading.Tasks;
using System.Xml;
using XCore.Framework.Framework.License.Contracts;
using XCore.Framework.Framework.License.Models;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.License.Handler
{
    public class LicenseHandler : ILicenseHandler
    {
        #region props.

        public bool Initialized { get; private set; }
        public string Name { get { return "LicenseHandler"; } }

        public bool IsValid { get; private set; }
        public event EventHandler<EventArgs> LicenseExpired;
        public bool RealtimeStatusChecking { get; protected set; }

        private string Key { get; set; }
        private string Definition { get; set; }
        private LicenseModel LicenseModel { get; set; }
        private const int INTEGRITYCHECKINTERVAL = 1;   // in hours ...

        #endregion
        #region cst.

        public LicenseHandler( string definition ) : this( definition , LicenseKeyContext.Default )
        {
        }
        public LicenseHandler( string definition , LicenseKeyContext licenseContext )
        {
            try
            {
                var status = true;

                status = status && ExtractKey( licenseContext );
                status = status && Prepare( definition );
                status = status && ValidateStatus();
                status = status && InitializePeriodicCheck();

                this.RealtimeStatusChecking = true;
                this.Initialized = status;
                this.LicenseExpired += LicenseHandler_LicenseExpired;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                this.Initialized = false;
            }
        }

        #endregion
        #region publics.

        public bool Validate( Guid featureId )
        {
            return this.LicenseModel.Validate( featureId );
        }
        public bool Validate( Guid featureId , int count )
        {
            return this.LicenseModel.Validate( featureId , count );
        }

        #endregion
        #region helpers.

        private bool ExtractKey( LicenseKeyContext licenseContext )
        {
            try
            {
                switch ( licenseContext.Mode )
                {
                    case LicenseKeyContextMode.Local:
                        {
                            this.Key = licenseContext.LicenseKey;
                            return !string.IsNullOrWhiteSpace( this.Key );
                        }
                    case LicenseKeyContextMode.Configurations:
                    case LicenseKeyContextMode.Registery:
                    case LicenseKeyContextMode.FileSystsem:
                    case LicenseKeyContextMode.Database:
                    default:
                        {
                            this.Key = null;
                            return false;
                        }
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        private bool Prepare( string definition )
        {
            try
            {
                this.Definition = XCrypto.DecryptFromAES( definition );

                var XMLDefinition = new XmlDocument();
                XMLDefinition.LoadXml( this.Definition );

                this.LicenseModel = new LicenseModel( this.Key , XMLDefinition );

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        private bool ValidateStatus()
        {
            this.IsValid = this.LicenseModel.ValidateAll();

            #region LOG

            if ( this.IsValid )
            {
                //XLogger.Info("License is Valid.");
            }
            else
            {
                XLogger.Error( "EXP.LCN. is Corrupt." );
            }

            #endregion

            return this.IsValid;
        }

        private bool InitializePeriodicCheck()
        {
            try
            {
                CheckStatusIntegrityAsync();
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        private async void CheckStatusIntegrityAsync()
        {
            await Task.Run( () => CheckStatusIntegrity() );
        }
        private async Task CheckStatusIntegrity()
        {
            try
            {
                while ( true )
                {
                    await Task.Delay( new TimeSpan( INTEGRITYCHECKINTERVAL , 0 , 0 ) );
                    if ( RealtimeStatusChecking )
                    {
                        if ( !ValidateStatus() )
                        {
                            if ( LicenseExpired != null ) LicenseExpired( this , null );
                        }
                    }
                    //XLogger.Trace( "Checking ... ... ..." );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        private void LicenseHandler_LicenseExpired( object sender , EventArgs e )
        {
            this.Initialized = false;
            this.IsValid = false;
        }

        #endregion
    }
}
