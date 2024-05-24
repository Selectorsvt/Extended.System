using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    }
}
