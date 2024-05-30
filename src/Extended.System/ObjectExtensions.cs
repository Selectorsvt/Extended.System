using System.ComponentModel;
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
