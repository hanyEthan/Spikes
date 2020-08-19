using System;
using System.IO;
using System.Linq;
using System.Reflection;

using ProtoBuf;
using ProtoBuf.Meta;

using ADS.Common.Utilities;
using ADS.Common.Contracts;
using System.Collections.Generic;

namespace ADS.Common.Handlers.Serialization
{
    internal class ProtobufSerializerHandler
    {
        #region props

        internal static bool Initialized { get; private set; }
        
        #endregion
        #region cst
        
        static ProtobufSerializerHandler()
        {
            InitializeAssemblies();
        }
        
        #endregion

        #region internals

        internal static byte[] Serialize<T>( T instance )
        {
            byte[] data = null;
            using ( var mem = new MemoryStream() )
            {
                Serializer.Serialize<T>( mem , instance );
                data = mem.ToArray();
            }
            return data;
        }
        internal static T Deserialize<T>( byte[] data )
        {
            using ( var mem = new MemoryStream( data ) )
            {
                return Serializer.Deserialize<T>( mem );
            }
        }
        
        #endregion
        #region helpers
        
        private static void InitializeAssemblies()
        {
            try
            {
                var assembliesStr = Broker.ConfigurationHandler.GetValue( Constants.ProtobufSection , Constants.ProtobufAssemblies );
                var assembliesArray = assembliesStr.Split( ';' ).Where( s => !string.IsNullOrWhiteSpace( s ) ).ToList();
                var loadedAssemblies = new List<Assembly>();
                Dictionary<Type, List<Type>> TypesMap = new Dictionary<Type, List<Type>>();

                # region Load Assemblies

                foreach ( var str in assembliesArray )
                {
                    try
                    {
                        Assembly assembly = Assembly.Load( str );
                        loadedAssemblies.Add( assembly );
                    }
                    catch ( Exception x )
                    {
                        XLogger.Warning( "Cannot Load Assembly : " + str + ". Exception : " + x );
                        continue;
                    }
                }

                # endregion

                # region Combine TypesMap

                foreach ( var assembly in loadedAssemblies )
                {
                    var XSerializableTypes = assembly.GetTypes().Where( type => type.GetInterfaces().Contains( typeof( IXSerializable ) ) );
                    foreach ( var type in XSerializableTypes ) TypesMap.Add( type, null );
                }

                foreach ( var assembly in loadedAssemblies )
                {
                    var XIncludableTypes = assembly.GetTypes().Where( type => type.GetInterfaces().Contains( typeof( IXIncludable ) ) );
                    foreach ( var type in XIncludableTypes )
                    {
                        var baseType = type.BaseType;
                        while ( baseType != typeof( Object ) )
                        {
                            if ( baseType.GetInterfaces().Contains( typeof( IXSerializable ) ) )
                            {
                                if ( TypesMap.ContainsKey( baseType ) )
                                {
                                    var includes = TypesMap[baseType];
                                    if ( includes == null ) includes = new List<Type>();
                                    includes.Add( type );
                                    TypesMap[baseType] = includes;
                                }
                            }

                            baseType = baseType.BaseType;
                        }
                    }
                }

                # endregion

                # region Build RuntimeTypeModel

                //int count = 1;

                foreach ( var item in TypesMap )
                {
                    int count = 1;

                    // Add IXSerializable..
                    var type = item.Key;
                    var includes = item.Value;

                    var metaType = RuntimeTypeModel.Default.Add( type, false );

                    foreach ( var prob in type.GetProperties() )
                    {
                        var attrs = prob.GetCustomAttributes( true );
                        if ( attrs != null && attrs.Count() > 0 && attrs[0] is XDontSerialize ) continue;

                        metaType.AddField( count, prob.Name ).AsReference = true;
                        count++;
                    }

                    // Add IXIncludable..
                    if ( includes == null ) continue;
                    foreach ( var subType in includes )
                    {
                        metaType.AddSubType( count, subType );
                        count++;
                    }
                }

                # endregion

                Initialized = true;
            }
            catch ( Exception ex )
            {
                XLogger.Error( ex.ToString() );
                Initialized = false;
            }
        }
        
        #endregion
    }
}