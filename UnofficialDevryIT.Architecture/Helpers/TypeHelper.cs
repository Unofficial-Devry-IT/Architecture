using System;
using System.Reflection;

namespace UnofficialDevryIT.Architecture.Helpers
{
   /// <summary>
    /// Provides some helper functions regarding types
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Is <paramref name="obj"/> a generic function?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsFunc(object obj)
        {
            if (obj == null)
                return false;

            var type = obj.GetType();

            if (!type.GetTypeInfo().IsGenericType)
                return false;

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        /// <summary>
        /// Is <paramref name="obj"/> a function which returns <typeparamref name="TReturn"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="TReturn">Type of value <paramref name="obj"/> should be return</typeparam>
        /// <returns></returns>
        public static bool IsFunc<TReturn>(object obj)
        {
            return obj != null && obj.GetType() == typeof(Func<TReturn>);
        }

        /// <summary>
        /// is <paramref name="type"/> a primitive?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeEnums"></param>
        /// <returns></returns>
        public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
        {
            if (IsPrimitiveExtended(type, includeEnums))
                return true;

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);
            
            return false;
        }

        static bool IsPrimitiveExtended(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive)
                return true;

            if (includeEnums && type.GetTypeInfo().IsEnum)
                return true;

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(DateTime) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }
}