using System;
using ADS.Common.Utilities;
using ADS.Common.Handlers.License.Definition;

namespace ADS.Tamam.License.Definition
{
    public class LicenseDefinition : IFeaturesDefinition
    {
        #region props.

        private string HiddenDefinition
        {
            get
            {
                return @"<All>
	                <Organization>
			                <ID>363CDCC3-8D24-49DF-8C3C-0AB67C84B16D</ID>
			                <DataProvider>ADS.Tamam.Modules.Organization.Handlers.OrganizationHandler, ADS.Tamam.Modules.Organization</DataProvider>
	                </Organization>
	                <Features>
		                <Feature>
			                <ID>49673c6f-596c-48e5-97ea-84548d76ae96</ID>
			                <Name>Number Of Terminals</Name>
			                <DataProvider>ADS.Tamam.Modules.Organization.Handlers.OrganizationHandler, ADS.Tamam.Modules.Organization</DataProvider>
		                </Feature>
		                <Feature>
			                <ID>0136C3B9-272A-4428-8413-E114720C15DE</ID>
			                <Name>Number Of Personnels</Name>
			                <DataProvider>ADS.Tamam.Modules.Personnel.Handlers.PersonnelHandler, ADS.Tamam.Modules.Personnel</DataProvider>
		                </Feature>
	                </Features>
                </All>";
            }
        }

        #endregion

        #region IBaseHandler

        public bool Initialized { get { return true; } }
        public string Name { get { return "OnTimeLicenseDefinition"; } }

        #endregion
        #region IFeaturesDefinition

        public string Definition
        {
            get
            {
                return XCrypto.EncryptToAES(this.HiddenDefinition);
            }
        }

        #endregion
    }
}
