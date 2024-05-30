using System.Collections.Specialized;

namespace Extended.System
{
    /// <summary>
    /// The dictionary extensions class.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns the name value collection using the specified dict.
        /// </summary>
        /// <param name="dict">The dict.</param>
        /// <returns>The name value collection.</returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, object> dict)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in dict)
            {
                string? value = null;
                if (kvp.Value != null)
                {
                    var type = kvp.Value.GetType();
                    if (type.IsFlaggedEnum())
                    {
                        var enumeration = EnumExtensions.GetFlagEnumList((Enum)kvp.Value);
                        nameValueCollection.Add(kvp.Key, enumeration);
                        continue;
                    }
                    else
                    {
                        value = kvp.Value.ToString();
                    }
                }

                nameValueCollection.Add(kvp.Key.ToString(), value);
            }

            return nameValueCollection;
        }
    }
}
