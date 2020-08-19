using System;
using System.Linq;
using System.Globalization;

using ADS.Common.Utilities;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public abstract class AbstractSpecialPolicy
    {
        #region props ...

        public PolicyModel Policy { get; protected set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string NameCultureVarient { get; set; }

        #endregion
        #region cst ...

        public AbstractSpecialPolicy( PolicyModel policy )
        {
            if ( policy == null ) return;

            Policy = policy;
            Name = policy.Name;
            NameCultureVarient = policy.NameCultureVarient;

            Code = policy.Code;
        }

        #endregion
        #region Helpers

        private string GetFieldValue( Guid fieldId )
        {
            try
            {
                if ( Policy == null ) return null;

                var value = Policy.Values.Where( x => x.PolicyFieldId ==  fieldId ).FirstOrDefault();
                return value != null ? value.Value : null;
            }
            catch
            {
                return null;
            }
        }

        protected int? GetInt( Guid fieldId )
        {
            try
            {
                int value;
                return int.TryParse ( GetFieldValue ( fieldId ) , out value ) ? ( int? ) value : null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        protected bool? GetBool( Guid fieldId )
        {
            try
            {
                bool value;
                return bool.TryParse ( GetFieldValue ( fieldId ) , out value ) ? ( bool? ) value : null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        protected float? GetFloat( Guid fieldId )
        {
            try
            {
                float value;
                return float.TryParse ( GetFieldValue ( fieldId ) , out value ) ? ( float? ) value : null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        protected double? GetDouble(Guid fieldId)
        {
            try
            {
                double value;
                return double.TryParse(GetFieldValue(fieldId), out value) ? (double?)value : null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        protected Guid? GetGuid( Guid fieldId )
        {
            try
            {
                Guid value;
                return Guid.TryParse ( GetFieldValue ( fieldId ) , out value ) ? ( Guid? ) value : null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        protected DateTime? GetDate( Guid fieldId )
        {
            try
            {
                DateTime value;
                return DateTime.TryParseExact ( GetFieldValue ( fieldId ) , "dd/MM/yyyy" , CultureInfo.InvariantCulture , DateTimeStyles.None , out value ) ? ( DateTime? ) value : null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        protected TimeSpan? GetTimeSpan( Guid fieldId )
        {
            try
            {
                TimeSpan value = TimeSpan.Parse( GetFieldValue( fieldId ) );
                return value;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        protected string GetString(Guid fieldId)
        {
             try
            {
                return GetFieldValue( fieldId );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }

        #endregion
    }
}
