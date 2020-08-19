using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XCore.Utilities.Utilities
{
    public static class XSerialize
    {
        #region JSON

        public static class JSON
        {
            public static string Serialize<T>( T value )
            {
                try
                {
                    return value != null ? JsonConvert.SerializeObject( value ,
                           new JsonSerializerSettings
                           {
                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore ,
                           } ) : null;
                }
                catch
                {
                    throw;
                    //return null;
                }
            }
            public static T Deserialize<T>( string value )
            {
                try
                {
                    return value != null ? JsonConvert.DeserializeObject<T>( value ) : default( T );
                }
                catch
                {
                    throw;
                    //return default( T );
                }
            }
        }

        #endregion
    }
}
