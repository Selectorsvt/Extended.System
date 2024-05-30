using System.Collections.Specialized;

namespace Extended.System
{
    /// <summary>
    /// The name value collection extensions class.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Adds the name value collection.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public static void Add(this NameValueCollection nameValueCollection, string key, IEnumerable<string?> values)
        {
            values.ForEach(v => nameValueCollection.Add(key, v));
        }
    }
}
