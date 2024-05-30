using System.Collections.Specialized;
using System.Text;

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

        /// <summary>
        /// Constructs the query string using the specified nvc.
        /// </summary>
        /// <param name="nvc">The nvc.</param>
        /// <returns>The query string.</returns>
        public static string ConstructQueryString(this NameValueCollection nvc)
        {
            if (nvc == null)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                string[]? values = nvc.GetValues(key);
                if (values == null)
                    continue;

                foreach (string value in values)
                {
                    sb.Append(sb.Length == 0 ? "?" : "&");
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                }
            }

            return sb.ToString();
        }
    }
}
