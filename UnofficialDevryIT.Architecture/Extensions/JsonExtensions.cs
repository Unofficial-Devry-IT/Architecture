using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UnofficialDevryIT.Architecture.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Converts given object to JSON string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerSettings();

            if (camelCase)
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();

            if (indented)
                options.Formatting = Formatting.Indented;

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}