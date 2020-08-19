using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ServiceStack;
using ServiceStack.Text;

namespace core.Utilities
{
    public static class JsonUtilities
    {
        #region statics.

        public static string Serialize<T>(T value)
        {
            try
            {
                return value != null
                     ? Newtonsoft.Json.JsonConvert.SerializeObject(value, new Newtonsoft.Json.JsonSerializerSettings
                     {
                         ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                     })
                     : null;
            }
            catch
            {
                throw;
                //return null;
            }
        }
        public static T Deserialize<T>(string json)
        {
            try
            {
                return json != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json) : default;
            }
            catch
            {
                throw;
                //return default( T );
            }
        }
        public static dynamic[] DeserializeExpando(params string[] jsons)
        {
            try
            {
                var converter = new Newtonsoft.Json.Converters.ExpandoObjectConverter();
                var list = new List<dynamic>();
                foreach (var json in jsons)
                {
                    dynamic expando = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(json, converter);
                    list.Add(expando);
                }

                return list.ToArray();
            }
            catch
            {
                throw;
            }
        }
        public static Dictionary<string, object> DeserializeDictionary(string json)
        {
            try
            {
                JsConfig.ConvertObjectTypesIntoStringDictionary = true;
                return (Dictionary<string, object>) json.FromJson<object>();
            }
            catch
            {
                throw;
            }
        }

        public static TResult ParseJson<TResult>(this string json, string jsonPath)
        {
            var jsonObject = JObject.Parse(json);
            JToken token = jsonObject.SelectToken(jsonPath);

            return token.Value<TResult>();
        }
        public static string ParseJsonString(this string json, string jsonPath)
        {
            return ParseJson<string>(json, jsonPath);
        }
        public static int ParseJsonInt(this string json, string jsonPath)
        {
            return ParseJson<int>(json, jsonPath);
        }
        public static double ParseJsonDouble(this string json, string jsonPath)
        {
            return ParseJson<double>(json, jsonPath);
        }

        public static IEnumerable<TResult> ParseJsonList<TResult>(this string json, string jsonPath)
        {
            var jsonObject = JObject.Parse(json);
            IEnumerable<JToken> tokens = jsonObject.SelectTokens(jsonPath);

            return tokens.Select(x => x.Value<TResult>()).ToList();
        }
        public static IEnumerable<string> ParseJsonStringList(this string json, string jsonPath)
        {
            return ParseJsonList<string>(json, jsonPath);
        }
        public static IEnumerable<int> ParseJsonIntList(this string json, string jsonPath)
        {
            return ParseJsonList<int>(json, jsonPath);
        }
        public static IEnumerable<double> ParseJsonDoubleList(this string json, string jsonPath)
        {
            return ParseJsonList<double>(json, jsonPath);
        }

        #endregion
    }
}
