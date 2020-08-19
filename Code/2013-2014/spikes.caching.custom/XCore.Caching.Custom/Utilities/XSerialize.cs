using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace XCore.Caching.Custom.Utilities
{
    public static class XSerialize
    {
        public enum Mode { Xml, XMLLegacy, Json, Binary, BinaryProtobuf }

        public static string Serialize<T>(Mode mode, T value)
        {
            switch (mode)
            {
                case Mode.Binary: return SerializeToBinaryString(value);
                case Mode.BinaryProtobuf: return SerializeToBinaryStringProtobuf(value);
                case Mode.Json: return SerializeToJsonString(value);
                case Mode.Xml: return SerializeToXmlString(value, new string[] { });
                case Mode.XMLLegacy:
                default: return SerializeToXmlStringLegacy(value, new string[] { });
            }
        }
        //public static bool Serialize<T>( Mode mode , T value , string file )
        //{
        //    return Serialize<T>( mode , value , null , file );
        //}
        public static bool Serialize<T>(Mode mode, T value, out Stream stream)
        {
            return Serialize<T>(mode, value, new string[] { }, out stream);
        }
        public static bool Serialize<T>(Mode mode, T value, out byte[] data)
        {
            return Serialize<T>(mode, value, new List<Type>() { }, out data);
        }

        public static string Serialize<T>(Mode mode, T value, IList<Type> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary: return SerializeToBinaryString(value);
                case Mode.BinaryProtobuf: return SerializeToBinaryStringProtobuf(value);
                case Mode.Json: return SerializeToJsonString(value);
                case Mode.Xml: return SerializeToXmlString(value, supportingTypes);
                case Mode.XMLLegacy:
                default: return SerializeToXmlStringLegacy(value, supportingTypes);
            }
        }
        //public static bool Serialize<T>( Mode mode , T value , IList<Type> supportingTypes , string file )
        //{
        //    switch ( mode )
        //    {
        //        case Mode.Binary: return SerializeToBinaryFile( value , file );
        //        case Mode.BinaryProtobuf: return SerializeToBinaryProtobufFile( value , file );
        //        case Mode.Json: return SerializeToJsonFile( value , file );
        //        case Mode.Xml: return SerializeToXmlFile( value , supportingTypes , file );
        //        case Mode.XMLLegacy:
        //        default: return SerializeToXmlFileLegacy( value , supportingTypes , file );
        //    }
        //}
        public static bool Serialize<T>(Mode mode, T value, IList<Type> supportingTypes, out Stream stream)
        {
            switch (mode)
            {
                case Mode.Binary: return (stream = SerializeToBinaryStream(value)) != null;
                case Mode.BinaryProtobuf: return (stream = SerializeToBinaryProtobufStream(value)) != null;
                case Mode.Json: return (stream = SerializeToJsonStream(value)) != null;
                case Mode.Xml: return (stream = SerializeToXmlStream(value, supportingTypes)) != null;
                case Mode.XMLLegacy:
                default: return (stream = SerializeToXmlStreamLegacy(value, supportingTypes)) != null;
            }
        }
        public static bool Serialize<T>(Mode mode, T value, IList<Type> supportingTypes, out byte[] data)
        {
            switch (mode)
            {
                case Mode.Binary: return (data = SerializeToBinaryArrayNative(value)) != null;
                case Mode.BinaryProtobuf: return (data = SerializeToBinaryArrayProtobuf(value)) != null;
                case Mode.Json: return (data = SerializeToJsonArray(value)) != null;
                case Mode.Xml: return (data = SerializeToXmlArray(value, supportingTypes)) != null;
                case Mode.XMLLegacy:
                default: return (data = SerializeToXmlArrayLegacy(value, supportingTypes)) != null;
            }
        }

        public static string Serialize<T>(Mode mode, T value, IList<string> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Xml: return SerializeToXmlString(value, supportingTypes);
                case Mode.XMLLegacy: return SerializeToXmlStringLegacy(value, supportingTypes);
                case Mode.Binary:
                case Mode.BinaryProtobuf:
                case Mode.Json:
                default: throw new NotImplementedException();
            }
        }
        //public static bool Serialize<T>( Mode mode , T value , IList<string> supportingTypes , string file )
        //{
        //    switch ( mode )
        //    {
        //        case Mode.Binary: return SerializeToBinaryFile( value , file );
        //        case Mode.BinaryProtobuf: return SerializeToBinaryProtobufFile( value , file );
        //        case Mode.Json: return SerializeToJsonFile( value , file );
        //        case Mode.Xml: return SerializeToXmlFile( value , supportingTypes , file );
        //        case Mode.XMLLegacy:
        //        default: return SerializeToXmlFileLegacy( value , supportingTypes , file );
        //    }
        //}
        public static bool Serialize<T>(Mode mode, T value, IList<string> supportingTypes, out Stream stream)
        {
            switch (mode)
            {
                case Mode.Xml:
                case Mode.Binary:
                case Mode.BinaryProtobuf:
                case Mode.Json:
                case Mode.XMLLegacy:
                default: throw new NotImplementedException();
            }
        }
        public static bool Serialize<T>(Mode mode, T value, IList<string> supportingTypes, out byte[] data)
        {
            switch (mode)
            {
                case Mode.BinaryProtobuf:
                case Mode.Xml:
                case Mode.Binary:
                case Mode.Json:
                case Mode.XMLLegacy:
                default: throw new NotImplementedException();
            }
        }

        public static T Deserialize<T>(Mode mode, string value)
        {
            return Deserialize<T>(mode, value, new string[] { });
        }
        public static T Deserialize<T>(Mode mode, FileInfo file)
        {
            return Deserialize<T>(mode, file, new List<Type>());
        }
        public static T Deserialize<T>(Mode mode, Stream stream)
        {
            return Deserialize<T>(mode, stream, new string[] { });
        }
        public static T Deserialize<T>(Mode mode, byte[] data)
        {
            return Deserialize<T>(mode, data, new List<Type>() { });
        }

        public static T Deserialize<T>(Mode mode, string value, IList<Type> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary: return DeserializeFromBinaryString<T>(value);
                case Mode.BinaryProtobuf: return DeserializeFromBinaryStringProtobuf<T>(value);
                case Mode.Json: return DeserializeFromJsonString<T>(value);
                case Mode.Xml: return DeserializeFromXmlString<T>(value, supportingTypes);
                case Mode.XMLLegacy:
                default: return DeserializeFromXmlStringLegacy<T>(value, supportingTypes);
            }
        }
        public static T Deserialize<T>(Mode mode, FileInfo file, IList<Type> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary: return DeserializeFromBinaryFile<T>(file);
                case Mode.BinaryProtobuf: return DeserializeFromBinaryFileProtobuf<T>(file);
                case Mode.Json: return DeserializeFromJsonFile<T>(file);
                case Mode.Xml: return DeserializeFromXmlFile<T>(file, supportingTypes);
                case Mode.XMLLegacy:
                default: return DeserializeFromXmlFileLegacy<T>(file, supportingTypes);
            }
        }
        public static T Deserialize<T>(Mode mode, Stream stream, IList<Type> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary: return DeserializeFromBinaryStream<T>(stream);
                case Mode.BinaryProtobuf: return DeserializeFromBinaryStreamProtobuf<T>(stream);
                case Mode.Json: return DeserializeFromJsonStream<T>(stream);
                case Mode.Xml: return DeserializeFromXmlStream<T>(stream, supportingTypes);
                case Mode.XMLLegacy:
                default: return DeserializeFromXmlStreamLegacy<T>(stream, supportingTypes);
            }
        }
        public static T Deserialize<T>(Mode mode, byte[] data, IList<Type> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary: return DeserializeFromBinaryArrayNative<T>(data);
                case Mode.BinaryProtobuf: return DeserializeFromBinaryArrayProtobuf<T>(data);
                case Mode.Json: return DeserializeFromJsonArray<T>(data);
                case Mode.Xml: return DeserializeFromXmlArray<T>(data, supportingTypes);
                case Mode.XMLLegacy:
                default: return DeserializeFromXmlArrayLegacy<T>(data, supportingTypes);
            }
        }

        public static T Deserialize<T>(Mode mode, string value, IList<string> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Xml: return DeserializeFromXmlString<T>(value, supportingTypes);
                case Mode.XMLLegacy: return DeserializeFromXmlStringLegacy<T>(value, supportingTypes);
                case Mode.Json: return DeserializeFromJsonString<T>(value);
                case Mode.Binary:
                case Mode.BinaryProtobuf:
                default: throw new NotImplementedException();
            }
        }
        public static T Deserialize<T>(Mode mode, FileInfo file, IList<string> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary:
                case Mode.BinaryProtobuf:
                case Mode.Json:
                case Mode.Xml:
                case Mode.XMLLegacy:
                default: throw new NotImplementedException();
            }
        }
        public static T Deserialize<T>(Mode mode, Stream stream, IList<string> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary:
                case Mode.BinaryProtobuf:
                case Mode.Json:
                case Mode.Xml:
                case Mode.XMLLegacy:
                default: throw new NotImplementedException();
            }
        }
        public static T Deserialize<T>(Mode mode, byte[] data, IList<string> supportingTypes)
        {
            switch (mode)
            {
                case Mode.Binary:
                case Mode.BinaryProtobuf:
                case Mode.Json:
                case Mode.Xml:
                case Mode.XMLLegacy:
                default: throw new NotImplementedException();
            }
        }

        #region Helpers

        private static string SerializeToBinaryString<T>(T value)
        {
            try
            {
                var formatter = new BinaryFormatter();

                using (var stream = new MemoryStream())
                {
                    formatter.Serialize(stream, value);
                    stream.Seek(0, SeekOrigin.Begin);

                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);

                    return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static string SerializeToBinaryStringProtobuf<T>(T value)
        {
            throw new NotImplementedException();
        }
        private static string SerializeToXmlStringLegacy<T>(T value, IList<Type> supportingTypes)
        {
            if (value == null) return null;

            try
            {
                var stream = new MemoryStream();
                var writer = new XmlTextWriter(stream, Encoding.UTF8);
                var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);

                serializer.Serialize(writer, value);
                stream = (MemoryStream)writer.BaseStream;

                return XString.ConvertFromUtf8(stream.ToArray());
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static string SerializeToXmlStringLegacy<T>(T value, IList<string> supportingTypes)
        {
            return SerializeToXmlStringLegacy(value, supportingTypes.Select(x => Type.GetType(x)).ToList());
        }
        private static string SerializeToXmlString<T>(T value, IList<Type> supportingTypes)
        {
            if (value == null) return null;

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(value.GetType(), supportingTypes);
                    serializer.WriteObject(stream, value);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static string SerializeToXmlString<T>(T value, IList<string> supportingTypes)
        {
            return SerializeToXmlString(value, supportingTypes.Select(x => Type.GetType(x)).ToList());
        }
        private static string SerializeToJsonString<T>(T value)
        {
            if (value == null) return null;

            try
            {
                return new JavaScriptSerializer().Serialize(value);
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }

        private static bool SerializeToBinaryFile<T>(T value, string file)
        {
            throw new NotImplementedException();
        }
        private static bool SerializeToBinaryProtobufFile<T>(T value, string file)
        {
            throw new NotImplementedException();
        }
        private static bool SerializeToXmlFileLegacy<T>(T value, IList<Type> supportingTypes, string file)
        {
            try
            {
                using (var stream = new FileStream(file, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);
                    serializer.Serialize(stream, value);
                }

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return false;
            }
        }
        private static bool SerializeToXmlFile<T>(T value, IList<Type> supportingTypes, string file)
        {
            throw new NotImplementedException();
        }
        private static bool SerializeToJsonFile<T>(T value, string file)
        {
            throw new NotImplementedException();
        }

        private static Stream SerializeToBinaryStream<T>(T value)
        {
            throw new NotImplementedException();
        }
        private static Stream SerializeToBinaryProtobufStream<T>(T value)
        {
            throw new NotImplementedException();
        }
        private static Stream SerializeToXmlStreamLegacy<T>(T value, IList<Type> supportingTypes)
        {
            try
            {
                string text;

                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);
                    serializer.Serialize(stream, value);

                    byte[] bytes = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    int readByteCount = stream.Read(bytes, 0, bytes.Length);
                    text = Encoding.UTF8.GetString(bytes, 0, readByteCount);
                }

                return new MemoryStream(Encoding.UTF8.GetBytes(text));
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static Stream SerializeToXmlStream<T>(T value, IList<Type> supportingTypes)
        {
            throw new NotImplementedException();
        }
        private static Stream SerializeToJsonStream<T>(T value)
        {
            throw new NotImplementedException();
        }

        private static byte[] SerializeToBinaryArrayNative<T>(T value)
        {
            try
            {
                var formatter = new BinaryFormatter();
                formatter.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                //var formatter = new NetDataContractSerializer ();
                //BinaryFormatter bf = new BinaryFormatter ();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, value);
                    return ms.ToArray();
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static byte[] SerializeToBinaryArrayProtobuf<T>(T value)
        {
            try
            {
                if (!ProtobufSerializerHandler.Initialized) return null;
                return ProtobufSerializerHandler.Serialize<T>(value);
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static byte[] SerializeToXmlArrayLegacy<T>(T value, IList<Type> supportingTypes)
        {
            try
            {

                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);
                    serializer.Serialize(stream, value);

                    byte[] bytes = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    int readByteCount = stream.Read(bytes, 0, bytes.Length);
                    return bytes;
                }

            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return null;
            }
        }
        private static byte[] SerializeToXmlArray<T>(T value, IList<Type> supportingTypes)
        {
            throw new NotImplementedException();
        }
        private static byte[] SerializeToJsonArray<T>(T value)
        {
            throw new NotImplementedException();
        }

        private static T DeserializeFromBinaryString<T>(string value)
        {
            try
            {
                var formatter = new BinaryFormatter();

                using (Stream stream = new MemoryStream(Convert.FromBase64String(value)))
                {
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromBinaryStringProtobuf<T>(string value)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromXmlStringLegacy<T>(string value, IList<Type> supportingTypes)
        {
            if (string.IsNullOrEmpty(value)) return default(T);

            try
            {
                var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);
                var stream = new MemoryStream(XString.ConvertToUtf8(value));

                return (T)serializer.Deserialize(stream);
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromXmlStringLegacy<T>(string value, IList<string> supportingTypes)
        {
            return DeserializeFromXmlStringLegacy<T>(value, supportingTypes.Select(x => Type.GetType(x)).ToList());
        }
        private static T DeserializeFromXmlString<T>(string value, IList<Type> supportingTypes)
        {
            if (string.IsNullOrEmpty(value)) return default(T);

            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    var reader = XmlDictionaryReader.CreateTextReader(stream, Encoding.UTF8, new XmlDictionaryReaderQuotas() { MaxArrayLength = int.MaxValue, MaxDepth = 2000, }, null);
                    var serializer = new DataContractSerializer(typeof(T), supportingTypes);

                    return (T)serializer.ReadObject(reader);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromXmlString<T>(string value, IList<string> supportingTypes)
        {
            return DeserializeFromXmlString<T>(value, supportingTypes.Select(x => Type.GetType(x)).ToList());
        }
        private static T DeserializeFromJsonString<T>(string value)
        {
            try
            {
                return (T)(new JavaScriptSerializer().Deserialize(value, typeof(T)));
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }

        private static T DeserializeFromBinaryFile<T>(FileInfo file)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromBinaryFileProtobuf<T>(FileInfo file)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromXmlFileLegacy<T>(FileInfo file, IList<Type> supportingTypes)
        {
            try
            {
                using (var stream = new FileStream(file.FullName, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromXmlFile<T>(FileInfo file, IList<Type> supportingTypes)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromJsonFile<T>(FileInfo file)
        {
            throw new NotImplementedException();
        }

        private static T DeserializeFromBinaryStream<T>(Stream stream)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromBinaryStreamProtobuf<T>(Stream stream)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromXmlStreamLegacy<T>(Stream stream, IList<Type> supportingTypes)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T), supportingTypes != null ? supportingTypes.ToArray() : null);
                return (T)serializer.Deserialize(stream);
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromXmlStream<T>(Stream stream, IList<Type> supportingTypes)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromJsonStream<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

        private static T DeserializeFromBinaryArrayNative<T>(byte[] data)
        {
            try
            {
                var formatter = new BinaryFormatter();
                formatter.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                // var formatter = new NetDataContractSerializer ();
                using (Stream stream = new MemoryStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromBinaryArrayProtobuf<T>(byte[] data)
        {
            try
            {
                if (!ProtobufSerializerHandler.Initialized || data == null) return default(T);
                return ProtobufSerializerHandler.Deserialize<T>(data);
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                throw;
                //return default( T );
            }
        }
        private static T DeserializeFromJsonArray<T>(byte[] data)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromXmlArrayLegacy<T>(byte[] data, IList<Type> supportingTypes)
        {
            throw new NotImplementedException();
        }
        private static T DeserializeFromXmlArray<T>(byte[] data, IList<Type> supportingTypes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
