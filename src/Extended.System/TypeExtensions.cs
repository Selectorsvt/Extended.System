namespace Extended.System
{
    /// <summary>
    /// The type extensions class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the default value using the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The object.</returns>
        public static dynamic? GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return default;
        }

        /// <summary>
        /// Describes whether is inherited.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>The bool.</returns>
        public static bool IsInherited<T>(this Type type)
        {
            var inheritedType = typeof(T);
            return IsInherited(type, inheritedType);
        }

        /// <summary>
        /// Describes whether is inherited.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="inheritedType">The inherited type.</param>
        /// <returns>The bool.</returns>
        public static bool IsInherited(this Type type, Type inheritedType)
        {
            return (inheritedType.IsInterface && type.GetInterfaces().Contains(inheritedType)) || (inheritedType.IsClass && inheritedType.IsAssignableFrom(type));
        }
    }
}
