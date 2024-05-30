namespace Extended.System
{
    /// <summary>
    /// The enum extensions class.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the flag enum list using the specified value.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>An enumerable of string.</returns>
        public static IEnumerable<string?> GetFlagEnumList<T>(this T value) where T : Enum
        {
            var type = typeof(T);
            foreach (Enum flag in Enum.GetValues(type))
            {
                if (value.HasFlag(flag))
                    yield return Enum.GetName(type, flag);
            }
        }
    }
}
