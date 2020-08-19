using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XCore.Framework.Framework.Captcha.Models
{
    public class JSON
    {
        /// <summary>
        /// Extra data for/from the JSON serializer/deserializer to included with the object model.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string , JToken> ExtraJson { get; } = new Dictionary<string , JToken>();
    }
}
