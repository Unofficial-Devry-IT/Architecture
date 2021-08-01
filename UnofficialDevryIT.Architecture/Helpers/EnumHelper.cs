using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UnofficialDevryIT.Architecture.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Convert an enum into a useful menu
        /// </summary>
        /// <param name="type">Enum type</param>
        /// <param name="zeroBased">Should the menu start at zero, or be 1 based</param>
        /// <returns>Dictionary mapping position --> enum value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDictionary<string, string> ToMenuDictionary(Type type, bool zeroBased = true)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var dics = new Dictionary<string, string>();
            var enumValues = Enum.GetValues(type);

            int index = 0;
            foreach (Enum value in enumValues)
            {
                string position = (index + (zeroBased ? 0 : 1)).ToString();
                dics.Add(position, value.GetDisplayName());

                index++;
            }
            
            return dics;
        }

        /// <summary>
        /// Convert an enum type into a dictionary
        /// </summary>
        /// <param name="type">Enum to get dictionary mapping for</param>
        /// <returns>Dictionary where key: enum value | value: textual representation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDictionary<Enum, string> ToDictionary(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var dics = new Dictionary<Enum, string>();
            var enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
                dics.Add(value, value.GetDisplayName());

            return dics;
        }

        /// <summary>
        /// Get textual representation for an enumerated value
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns><see cref="string"/> representation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetDisplayName(this Enum value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var displayName = value.ToString();
            var fieldInfo = value.GetType().GetField(displayName);
            var attributes = (DisplayAttribute[]) fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            
            if (attributes.Length > 0)
                return attributes.First().Name;

            var nameAttributes = (DisplayNameAttribute[]) fieldInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (nameAttributes.Length > 0)
                return nameAttributes.First().DisplayName;

            return value.ToString();
        }
    }
}