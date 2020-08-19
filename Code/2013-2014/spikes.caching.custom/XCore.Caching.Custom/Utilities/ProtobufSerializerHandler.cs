using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using XCore.Caching.Custom.Contracts;

namespace XCore.Caching.Custom.Utilities
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

        internal static byte[] Serialize<T>(T instance)
        {
            byte[] data = null;
            using (var mem = new MemoryStream())
            {
                Serializer.Serialize<T>(mem, instance);
                data = mem.ToArray();
            }
            return data;
        }
        internal static T Deserialize<T>(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(mem);
            }
        }

        #endregion
        #region helpers

        private static void InitializeAssemblies()
        {
            try
            {
                Dictionary<Type, List<Type>> TypesMap = new Dictionary<Type, List<Type>>();

                #region Combine TypesMap

                var assembly = Assembly.GetExecutingAssembly();
                var XSerializableTypes = assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IXSerializable)));
                foreach (var type in XSerializableTypes) TypesMap.Add(type, null);

                var XIncludableTypes = assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IXIncludable)));
                foreach (var type in XIncludableTypes)
                {
                    var baseType = type.BaseType;
                    while (baseType != typeof(Object))
                    {
                        if (baseType.GetInterfaces().Contains(typeof(IXSerializable)))
                        {
                            if (TypesMap.ContainsKey(baseType))
                            {
                                var includes = TypesMap[baseType];
                                if (includes == null) includes = new List<Type>();
                                includes.Add(type);
                                TypesMap[baseType] = includes;
                            }
                        }

                        baseType = baseType.BaseType;
                    }
                }

                # endregion

                # region Build RuntimeTypeModel

                //int count = 1;

                foreach (var item in TypesMap)
                {
                    int count = 1;

                    // Add IXSerializable..
                    var type = item.Key;
                    var includes = item.Value;

                    var metaType = RuntimeTypeModel.Default.Add(type, false);

                    foreach (var prob in type.GetProperties())
                    {
                        var attrs = prob.GetCustomAttributes(true);
                        if (attrs != null && attrs.Count() > 0 && attrs[0] is XDontSerialize) continue;

                        metaType.AddField(count, prob.Name).AsReference = true;
                        count++;
                    }

                    // Add IXIncludable..
                    if (includes == null) continue;
                    foreach (var subType in includes)
                    {
                        metaType.AddSubType(count, subType);
                        count++;
                    }
                }

                # endregion

                Initialized = true;
            }
            catch (Exception ex)
            {
                XLogger.Error(ex.ToString());
                Initialized = false;
            }
        }

        #endregion
    }
}
