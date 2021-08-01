using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnofficialDevryIT.Architecture.Helpers;
using UnofficialDevryIT.Architecture.Models;

namespace UnofficialDevryIT.Architecture.Extensions
{
    /// <summary>
    /// Adds functionality for entities <see cref="IExtendableObject"/>
    /// </summary>
    public static class ExtendableObjectExtensions
    {

        /// <summary>
        /// Retrieve <paramref name="name"/> key from JSON structure
        /// </summary>
        /// <param name="extendableObject">Entity which contains data</param>
        /// <param name="name">Key to get data from</param>
        /// <param name="handleType"></param>
        /// <typeparam name="T">The type associated to the value at (key) <paramref name="name"/></typeparam>
        /// <returns>Value located at <paramref name="name"/> of type <typeparamref name="T"/></returns>
        public static T GetData<T>(this IExtendableObject extendableObject, string name, bool handleType = false)
        {
            CheckNotNull(extendableObject, name);

            return extendableObject.GetData<T>(name, handleType
                ? new JsonSerializer() {TypeNameHandling = TypeNameHandling.All}
                : JsonSerializer.CreateDefault());
        }
        
        /// <summary>
        /// Attempts to retrieve value located at key (<paramref name="name"/>)
        /// </summary>
        /// <param name="extendableObject">Entity which contains the data</param>
        /// <param name="name">Key from JSON structure to get data from</param>
        /// <param name="jsonSerializer"></param>
        /// <typeparam name="T">Type associated to the data attempting to be fetched</typeparam>
        /// <returns><typeparamref name="T"/> data</returns>
        public static T GetData<T>(this IExtendableObject extendableObject, string name, JsonSerializer jsonSerializer)
        {
            CheckNotNull(extendableObject, name);

            if (extendableObject.ExtensionData == null)
                return default;

            var json = JObject.Parse(extendableObject.ExtensionData);
            var prop = json[name];

            if (prop == null)
                return default;

            if (TypeHelper.IsPrimitiveExtendedIncludingNullable(typeof(T)))
                return prop.Value<T>();
            else
                return (T) prop.ToObject(typeof(T), jsonSerializer ?? JsonSerializer.CreateDefault());
        }

        
        
        /// <summary>
        /// Attempt to set the value of <paramref name="name"/> in JSON structure to <paramref name="value"/>
        /// </summary>
        /// <param name="extendableObject">Entity containing data</param>
        /// <param name="name">Key in JSON structure</param>
        /// <param name="value">Value that shall be set to <paramref name="name"/> key</param>
        /// <param name="handleType"></param>
        /// <typeparam name="T">Type of data that is being set</typeparam>
        public static void SetData<T>(this IExtendableObject extendableObject, string name, T value,
            bool handleType = false)
        {
            extendableObject.SetData(name, value, handleType ? new JsonSerializer { TypeNameHandling = TypeNameHandling.All }
                : JsonSerializer.CreateDefault());
        }
        
        /// <summary>
        /// Attempt to set value of <paramref name="name"/> in JSON structure to <paramref name="value"/>
        /// </summary>
        /// <param name="extendableObject">Entity containing data</param>
        /// <param name="name">Key in JSON structure which <paramref name="value"/> will be set to</param>
        /// <param name="value">Data which will be inserted into <paramref name="name"/> key</param>
        /// <param name="jsonSerializer"></param>
        /// <typeparam name="T">Type of data that shall be inserted</typeparam>
        public static void SetData<T>(this IExtendableObject extendableObject, string name, T value,
            JsonSerializer jsonSerializer = null)
        {
            CheckNotNull(extendableObject, name);

            if (jsonSerializer == null)
                jsonSerializer = JsonSerializer.CreateDefault();

            if (extendableObject.ExtensionData == null)
            {
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                    return;

                extendableObject.ExtensionData = "{}";
            }

            var json = JObject.Parse(extendableObject.ExtensionData);

            if (value == null | EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                if (json[name] != null)
                    json.Remove(name);
            }
            else if (TypeHelper.IsPrimitiveExtendedIncludingNullable(value.GetType()))
                json[name] = new JValue(value);
            else
                json[name] = JToken.FromObject(value, jsonSerializer);

            var data = json.ToString(Formatting.None);

            if (data == "{}")
                data = null;

            extendableObject.ExtensionData = data;
        }
        
        /// <summary>
        /// Deletes / Removes key from JSON structure
        /// </summary>
        /// <param name="extendableObject">Entity containing data</param>
        /// <param name="name">Delete the key associated with this name</param>
        /// <returns>True if successful, otherwise false (false may also indicate it didn't exist)</returns>
        public static bool RemoveData(this IExtendableObject extendableObject, string name)
        {
            CheckNotNull(extendableObject, name);

            if (extendableObject.ExtensionData == null)
                return false;

            var json = JObject.Parse(extendableObject.ExtensionData);

            var token = json[name];
            if (token == null)
                return false;

            json.Remove(name);

            var data = json.ToString(Formatting.None);

            if (data == "{}")
                data = null;

            extendableObject.ExtensionData = data;
            return true;
        }

        /// <summary>
        /// Ensure all values within <paramref name="values"/> are not null
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void CheckNotNull(params object[] values)
        {
            foreach(var value in values)
                if (value == null)
                    throw new ArgumentNullException();
        }
    }
}