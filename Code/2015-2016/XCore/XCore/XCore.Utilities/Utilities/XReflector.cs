using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XCore.Utilities.Utilities
{
    public static class XReflector
    {
        /// <summary>
        /// return an instance of the given type name
        /// </summary>
        public static T GetInstance<T>( string typeName ) where T : class
        {
            return GetInstance<T>( typeName , null );
        }

        /// <summary>
        /// return an instance of the given type name, and use the constructor of the given arguments types
        /// </summary>
        public static T GetInstance<T>( string typeName , params object[] args ) where T : class
        {
            try
            {
                if ( string.IsNullOrWhiteSpace( typeName ) ) return default( T );

                Type type = Type.GetType( typeName );
                return Activator.CreateInstance( type , args ) as T;
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception (Type Name : " + typeName + ") : " + x );
                return null;
            }
        }

        /// <summary>
        /// return the type of given type name
        /// </summary>
        public static Type GetType( string typeName )
        {
            try
            {
                return Type.GetType( typeName );
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : Type Name : " + typeName );
                NLogger.Error( "Exception : " + x );
                return null;
            }
        }

        /// <summary>
        /// get the current class.method names of the calling methods ...
        /// </summary>
        public static string GetCurrentMethodName( string className = null , [CallerMemberName] string methodName = null )
        {
            className = className ?? new StackTrace().GetFrame( 1 ).GetMethod().ReflectedType.FullName;
            return string.Format( "{0}.{1}" , className.Substring( className.LastIndexOf( '.' ) + 1 ) , methodName );
        }

        /// <summary>
        /// get the current class.method names of the sent method delegate ...
        /// </summary>
        public static string GetCurrentMethodName( Action action )
        {
            string className = action.Method.ReflectedType.FullName;
            return string.Format( "{0}.{1}" , className.Substring( className.LastIndexOf( '.' ) + 1 ) , action.Method.Name );
        }
    }
}
