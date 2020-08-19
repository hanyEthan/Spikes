using System;
using System.Collections.Generic;
using System.Xml;
using XCore.Framework.Framework.License.Contracts;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.License.Models
{
    public class LicenseModel
    {
        #region props.

        private List<ILicenseValidator> AvailableDefinitions { get; set; }
        private Dictionary<Guid, ILicenseValidator> RegisteredDefinitions { get; set; }

        #endregion
        #region cst.

        public LicenseModel(string key, XmlDocument definition)
        {
            try
            {
                var status = true;

                LicenseKey keyData = LicenseKey.Decode(key);
                status = status && keyData.IsValid();
                status = status && Prepare();
                status = status && Build(definition , keyData);

                if (!status) throw new Exception();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        #endregion
        #region publics.

        public bool ValidateAll()
        {
            try
            {
                foreach (var feature in RegisteredDefinitions.Values)
                {
                    if (!feature.IsValid())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        public bool Validate(Guid featureId)
        {
            try
            {
                return RegisteredDefinitions.ContainsKey(featureId) 
                    && RegisteredDefinitions[featureId].IsValid();
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        public bool Validate(Guid featureId, int count)
        {
            try
            {
                return RegisteredDefinitions.ContainsKey(featureId)
                    && RegisteredDefinitions[featureId].IsValid(count);
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }

        #endregion
        #region helpers.

        private bool Prepare()
        {
            this.AvailableDefinitions = new List<ILicenseValidator>();
            this.RegisteredDefinitions = new Dictionary<Guid, ILicenseValidator>();

            return true;
        }
        private bool Build(XmlDocument featuresDefinitions , LicenseKey keyData)
        {
            try
            {
                XmlNode xmlNode = featuresDefinitions.SelectSingleNode("All");

                // organization ...
                var organizationLicense = BuildDefinition(DefinitionType.Organization, xmlNode, keyData);
                if ( organizationLicense != null )
                {
                    if (!RegisterFeature(organizationLicense, keyData)) return false;
                }

                // time ...
                var timeLicense = BuildDefinition(DefinitionType.Time, null, keyData);
                if (!RegisterFeature(timeLicense, keyData)) return false;

                // features ...
                XmlNode xmlNodeFeatures = xmlNode.SelectSingleNode("Features");
                if ( xmlNodeFeatures != null )
                {
                    foreach ( XmlNode xmlNodeChild in xmlNodeFeatures )
                    {
                        var feature = BuildDefinition( DefinitionType.Feature , xmlNodeChild , keyData );
                        if ( feature == null ) return false;

                        if ( !RegisterFeature( feature , keyData ) ) return false;
                    }
                }

                // ...
                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }

        private ILicenseValidator BuildDefinition( DefinitionType type , XmlNode definition, LicenseKey keyData)
        {
            switch (type)
            {
                case DefinitionType.Time: return BuildDefinitionTime(keyData);
                case DefinitionType.Organization: return BuildDefinitionOrg(definition, keyData);
                case DefinitionType.Feature: return BuildDefinitionFeature(definition, keyData);
                default: return null;
            }
        }
        private ILicenseValidator BuildDefinitionTime(LicenseKey keyData)
        {
            ITimeLicense timeLicense = keyData.TimeLicenseType == 1 ? (ITimeLicense)new InfinitTimeLicense() :
                                       keyData.TimeLicenseType == 2 ? (ITimeLicense)new PeriodicTimeLicense() :
                                       keyData.TimeLicenseType == 3 ? (ITimeLicense)new TrialTimeLicense() : null;

            timeLicense.StartDate = keyData.StartDate;
            timeLicense.Period = keyData.StartDate.AddMonths(keyData.Period).Subtract(keyData.StartDate);

            return timeLicense;
        }
        private ILicenseValidator BuildDefinitionOrg(XmlNode definition, LicenseKey keyData)
        {
            IOrgDataProvider OrganizationValidator = XReflector.GetInstance<IOrgDataProvider>(definition.SelectSingleNode("//Organization/DataProvider")?.InnerText);
            if ( OrganizationValidator == null ) return null;

            OrganizationLicense organizationLicense = new OrganizationLicense(OrganizationValidator);
            organizationLicense.Id = new Guid(definition.SelectSingleNode("//Organization/ID").InnerText);
            organizationLicense.OrganizationInfo = keyData.CompanyName;

            return organizationLicense;
        }
        private ILicenseValidator BuildDefinitionFeature(XmlNode definition, LicenseKey keyData)
        {
            try
            {
                var feature = new Feature();
                feature.Id = new Guid(definition.SelectSingleNode("ID").InnerText);
                feature.Name = definition.SelectSingleNode("Name").InnerText;
                feature.Quota = keyData.GetQuota(feature.Id);

                // data provider ...
                string dataProvider = definition.SelectSingleNode("DataProvider") != null ? definition.SelectSingleNode("DataProvider").InnerText : null;
                feature.DataProvider = dataProvider != null ? XReflector.GetInstance<IDataProvider>(dataProvider) : null;
                if (dataProvider != null && feature.DataProvider == null) return null;

                XmlNode xmlNodeChilds = definition.SelectSingleNode("Features");
                if (xmlNodeChilds != null)
                {
                    foreach (XmlNode xmlNodeChild in xmlNodeChilds)
                    {
                        feature.LicenseValidators.Add((Feature)BuildDefinitionFeature(xmlNodeChild, keyData));
                    }
                }
                return feature;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }

        private bool RegisterFeature(ILicenseValidator feature , LicenseKey keyData)
        {
            return RegisterFeature(feature, keyData, false);
        }
        private bool RegisterFeature(ILicenseValidator feature, LicenseKey keyData, bool isSubFeature)
        {
            if (!isSubFeature) this.AvailableDefinitions.Add(feature);
            if (feature is OrganizationLicense || feature is ITimeLicense || keyData.IsFeatureIncluded(feature.Id))
            {
                this.RegisteredDefinitions.Add(feature.Id, feature);
            }

            foreach (var subLicenseValidator in feature.LicenseValidators)
            {
                RegisterFeature(subLicenseValidator, keyData, true);
            }

            return true;
        }

        #endregion
        #region nested

        private enum DefinitionType { Time , Organization , Feature , }

        #endregion
    }
}
