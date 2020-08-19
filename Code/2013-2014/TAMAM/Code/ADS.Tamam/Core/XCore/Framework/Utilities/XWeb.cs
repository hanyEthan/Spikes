using System.Web;

namespace XCore.Framework.Utilities
{
    public static class XWeb
    {
        #region Nested

        public static class Session
        {
            #region publics.

            public static T Get<T>( string name )
            {
                if ( !IsValid() ) return default( T );
                if ( !IsValid( name ) ) return default( T );

                return ( T ) HttpContext.Current.Session[name];
            }
            public static object Get( string name )
            {
                if ( !IsValid() ) return null;
                if ( !IsValid( name ) ) return null;

                return HttpContext.Current.Session[name];
            }

            public static void Set( string name , object value )
            {
                if ( !IsValid() ) return;
                if ( !IsValid( name ) ) return;

                HttpContext.Current.Session[name] = value;
            }
            public static void Add( string name , object value )
            {
                if ( !IsValid() ) return;
                if ( !IsValid( name ) ) return;

                HttpContext.Current.Session.Add( name , value );
            }

            public static void Clear()
            {
                if ( !IsValid() ) return;

                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
            }
            public static void Clear( string name )
            {
                if ( !IsValid() ) return;
                if ( !IsValid( name ) ) return;

                HttpContext.Current.Session.Remove( name );
            }

            #endregion
            #region helpers.

            private static bool IsValid()
            {
                return HttpContext.Current != null;
            }
            private static bool IsValid( string value )
            {
                return !string.IsNullOrWhiteSpace( value );
            }

            #endregion
        }

        #endregion
    }
}
