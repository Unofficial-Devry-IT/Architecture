using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace UnofficialDevryIT.Architecture.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Retrieves an array portion from the configuration file
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="valuePath">Path to value (Account:Options:Roles)</param>
        /// <returns></returns>
        public static IEnumerable<string> GetEnumerable(this IConfiguration configuration, string valuePath)
        {
            return configuration.GetSection(valuePath).Get<string[]>();
        }

        /// <summary>
        /// Retrieves an array of <typeparamref name="T"/> from configuration file
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="valuePath">PAth to value (Account:Options:RoleIds)</param>
        /// <typeparam name="T">Numeric type to retrieve from configuration file.</typeparam>
        /// <returns>Array containing numeric values</returns>
        public static IEnumerable<T> GetEnumerable<T>(this IConfiguration configuration, string valuePath)
            where T : IComparable, IConvertible, IFormattable, IComparable<T>, IEquatable<T>
        {
            return configuration.GetSection(valuePath).Get<T[]>();
        }
    }
}