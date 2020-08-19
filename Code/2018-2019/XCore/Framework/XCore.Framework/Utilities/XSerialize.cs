using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XCore.Framework.Utilities
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
            public static IDictionary<string, string> Parse(string value)
            {
                try
                {
                    return JsonParser.Parse(value);
                }
                catch
                {
                    throw;
                    //return default( T );
                }
            }

            #region nested.

            public class JsonParser
            {
                private JsonParser() { }

                private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                private readonly Stack<string> _context = new Stack<string>();
                private string _currentPath;

                public static IDictionary<string, string> Parse(string json) => new JsonParser().ParseJson(json);

                private IDictionary<string, string> ParseJson(string json)
                {
                    _data.Clear();

                    var jsonConfig = JObject.Parse(json);

                    VisitJObject(jsonConfig);

                    return _data;
                }

                private void VisitJObject(JObject jObject)
                {
                    foreach (var property in jObject.Properties())
                    {
                        EnterContext(property.Name);
                        VisitProperty(property);
                        ExitContext();
                    }
                }

                private void VisitProperty(JProperty property)
                {
                    VisitToken(property.Value);
                }

                private void VisitToken(JToken token)
                {
                    switch (token.Type)
                    {
                        case JTokenType.Object:
                            VisitJObject(token.Value<JObject>());
                            break;

                        case JTokenType.Array:
                            VisitArray(token.Value<JArray>());
                            break;

                        case JTokenType.Integer:
                        case JTokenType.Float:
                        case JTokenType.String:
                        case JTokenType.Boolean:
                        case JTokenType.Bytes:
                        case JTokenType.Raw:
                        case JTokenType.Null:
                            VisitPrimitive(token.Value<JValue>());
                            break;

                        default:
                            throw new FormatException("Unsupported JSON token");
                    }
                }

                private void VisitArray(JArray array)
                {
                    for (int index = 0; index < array.Count; index++)
                    {
                        EnterContext(index.ToString());
                        VisitToken(array[index]);
                        ExitContext();
                    }
                }

                private void VisitPrimitive(JValue data)
                {
                    var key = _currentPath;

                    if (_data.ContainsKey(key))
                    {
                        throw new FormatException("Duplicate Key");
                    }
                    _data[key] = data.ToString(CultureInfo.InvariantCulture);
                }

                private void EnterContext(string context)
                {
                    _context.Push(context);
                    _currentPath = ConfigurationPath.Combine(_context.Reverse());
                }

                private void ExitContext()
                {
                    _context.Pop();
                    _currentPath = ConfigurationPath.Combine(_context.Reverse());
                }
            }

            #endregion
        }

        #endregion
    }
}
