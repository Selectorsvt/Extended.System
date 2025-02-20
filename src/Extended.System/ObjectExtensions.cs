using System.ComponentModel;
#if NETCOREAPP3_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Extended.System
{
    /// <summary>
    /// The object extensions class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks the is null using the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="memberName">The member name.</param>
        /// <exception cref="ArgumentNullException">The ArgumentNullException.</exception>
        public static void CheckIsNull(
#if NETCOREAPP3_0_OR_GREATER
            [NotNull]
#endif
            this object? obj,
#if NETCOREAPP3_0_OR_GREATER
            [CallerArgumentExpression(nameof(obj))]
#endif
            string? memberName = null)
        {
#if NETCOREAPP3_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(obj, memberName);
#else
            if (obj == null)
                 throw new ArgumentNullException(memberName);
#endif
        }

        /// <summary>
        /// Gets the property type using the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="memberName">The member name.</param>
        /// <returns>The type.</returns>
        public static Type? GetPropertyType(this object obj, [CallerMemberName] string? memberName = null)
        {
            if (obj is null)
                return null;

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                if (property.Name == memberName)
                    return property.PropertyType;
            }

            return null;
        }

        /// <summary>
        /// Gets the json using the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The string.</returns>
        public static string SerializeJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Returns the dynamic using the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The result.</returns>
        public static IDictionary<string, object?> ToDynamic(this object obj)
        {
            if (obj is IDictionary<string, object?> dictionary)
            {
                return dictionary;
            }

            var properties = TypeDescriptor.GetProperties(obj);
            var result = new Dictionary<string, object?>();
            foreach (PropertyDescriptor property in properties)
            {
                result.Add(property.Name, property.GetValue(obj));
            }

            return result;
        }
    }
}
