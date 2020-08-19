using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mcs.Invoicing.Core.Framework.Utilities
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
        public static async Task<List<SchemaValidationError>> ValidateSchema(string json, string schemaJson)
        {
            var schema = await NJsonSchema.JsonSchema.FromJsonAsync(schemaJson);
            var validator = new NJsonSchema.Validation.JsonSchemaValidator();
            var result = validator.Validate(json, schema);

            return result?.Select(x=>new SchemaValidationError(x)).ToList();
        }

        #endregion
        #region nested.

        public class SchemaValidationError
        {
            #region props.

            public string Error { get; internal set; }
            public string PropertyName { get; internal set; }
            public string PropertyPath { get; internal set; }
            public bool HasLineInfo { get; internal set; }
            public int LineNumber { get; internal set; }
            public int LinePosition { get; internal set; }

            #endregion
            #region cst.

            public SchemaValidationError()
            {

            }
            public SchemaValidationError(NJsonSchema.Validation.ValidationError validationError)
                 : this(validationError?.Kind.ToString(),
                        validationError?.Property,
                        validationError?.Path,
                        validationError?.HasLineInfo,
                        validationError?.LineNumber,
                        validationError?.LinePosition)
            {

            }
            public SchemaValidationError(string error, 
                                         string propertyName, 
                                         string propertyPath,
                                         bool? hasLineInfo,
                                         int? lineNumber,
                                         int? linePosition) : this()
            {
                this.Error = error;
                this.PropertyName = propertyName;
                this.PropertyPath = propertyPath;
                this.HasLineInfo = hasLineInfo.GetValueOrDefault();
                this.LineNumber = lineNumber.GetValueOrDefault();
                this.LinePosition = linePosition.GetValueOrDefault();
            }

            #endregion
        }

        #endregion
    }
}
