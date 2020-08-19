namespace XCore.Framework.Framework.Geo.Models
{
    public class GeoLocation
    {
        #region props.

        public bool? IsValid { get { return this.Validate(); } }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        #endregion
        #region cst.

        public GeoLocation()
        {
        }
        public GeoLocation(double? longitude , double? latitude) : this()
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
        public GeoLocation(string longitude, string latitude) : this(ToDouble(longitude), ToDouble(latitude))
        {

        }

        #endregion
        #region helpers.

        private bool Validate()
        {
            try
            {
                bool state = true;

                state = state && this.Longitude != null;
                state = state && this.Latitude != null;
                state = state && this.Latitude >= -90 && this.Latitude <= 90;
                state = state && this.Longitude >= -180 && this.Longitude <= 180;

                return state;
            }
            catch
            {
                return false;
            }
        }
        private static double? ToDouble(string from)
        {
            return double.TryParse(from, out double to) ? (double?) to : null;
        }

        #endregion
    }
}
