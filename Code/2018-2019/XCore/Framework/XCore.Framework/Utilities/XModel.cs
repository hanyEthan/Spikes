using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XCore.Framework.Utilities
{
    public static class XModel
    {
        /// <summary>
        /// Compare between two objects of the same type on public primitive properties
        /// return a list with properties names that have different values
        /// </summary>
        public static List<string> GetDifferences<T>( T self , T to , params string[] ignore ) where T : class
        {
            var differences = new List<string>();
            if ( self == null && to == null ) return differences;

            var type = typeof( T );
            var ignoreList = new List<string>( ignore );
            var properties = type.GetProperties( BindingFlags.Public | BindingFlags.Instance ).ToList();
            // filter : get primitives only
            properties = properties.Where( p => p.PropertyType.IsValueType || p.PropertyType == typeof( string ) ).ToList();

            foreach ( var prop in properties )
            {
                if ( ignoreList.Contains( prop.Name ) ) continue;

                var selfValue = type.GetProperty( prop.Name ).GetValue( self , null );
                var toValue = type.GetProperty( prop.Name ).GetValue( to , null );

                if ( selfValue == null && toValue == null ) continue;

                if ( selfValue == null ^ toValue == null )
                {
                    differences.Add( prop.Name );
                    continue;
                }

                if ( !selfValue.Equals( toValue ) )
                {
                    differences.Add( prop.Name );
                }
            }

            return differences;
        }
        public static T Clone<T>( T model ) where T : class, new()
        {
            if ( model == null ) return default( T );

            T cloned = new T();

            var type = typeof( T );
            var properties = type.GetProperties( BindingFlags.Public | BindingFlags.Instance ).ToList();

            // filter : get primitives only
            //properties = properties.Where( p => p.PropertyType.IsValueType || p.PropertyType == typeof( string ) ).ToList();

            foreach ( var prop in properties )
            {
                var selfValue = type.GetProperty( prop.Name ).GetValue( model , null );

                if ( type.GetProperty( prop.Name ).CanWrite )
                {
                    type.GetProperty( prop.Name ).SetValue( cloned , selfValue );
                }
            }

            return cloned;
        }
    }
}
