using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Utilities;

namespace ADS.Common.Handlers.License.Model
{
    [Serializable]
    public class LicenseKey
    {
        #region props.

        public string CompanyName { get; set; }
        public int TimeLicenseType { get; set; }
        public DateTime StartDate { get; set; }
        public int Period { get; set; }
        public List<KeyFeature> Features { get; set; }

        #endregion
        #region Nested.

        [Serializable]
        public class KeyFeature
        {
            #region props.

            public Guid Id { get; set; }
            public int Quota { get; set; }

            #endregion
            #region cst.

            public KeyFeature()
            {
                this.Id = new Guid();
            }

            #endregion
        }

        #endregion
        #region cst.

        public LicenseKey()
        {

        }
        public LicenseKey(string companyName, int timeLicenseType, DateTime startDate, int peroid , List<KeyFeature> features) : this()
        {
            this.CompanyName = companyName;
            this.TimeLicenseType = timeLicenseType;
            this.StartDate = startDate;
            this.Period = peroid;
            this.Features = features;
        }

        #endregion
        #region statics.

        public static string Encode(LicenseKey key)
        {
            try
            {
                var json = XSerialize.Serialize<LicenseKey>(XSerialize.Mode.Json, key);
                var encoded =  XCrypto.EncryptToAES(json);

                return encoded;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }
        public static LicenseKey Decode(string key)
        {
            try
            {
                var decrypted = XCrypto.DecryptFromAES(key);
                var model = XSerialize.Deserialize<LicenseKey>(XSerialize.Mode.Json, decrypted);

                return model;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }

        #endregion
        #region publics.

        public bool IsValid()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CompanyName)) return false;
                if (TimeLicenseType <= 0 || TimeLicenseType > 3) return false;
                if (StartDate == DateTime.MinValue) return false;
                if (Period <= 0) return false;
                if (Features.Count == 0) return false;
                else
                {
                    foreach (var feature in Features)
                    {
                        if (feature == null) return false;
                        if (feature.Id == Guid.Empty) return false;
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
        public bool IsFeatureIncluded(Guid id)
        {
            return Features.Exists(x => x.Id == id);
        }
        public int GetQuota( Guid featureId )
        {
            var feature = Features.FirstOrDefault(x => x.Id == featureId);
            var quota = feature == null ? 0 : feature.Quota;

            return quota;
        }

        #endregion
    }
}
