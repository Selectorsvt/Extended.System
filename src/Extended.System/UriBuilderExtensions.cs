using System.Collections.Specialized;
using System.Web;

namespace Extended.System
{
    /// <summary>
    /// The uri builder extensions class.
    /// </summary>
    public static class UriBuilderExtensions
    {
        /// <summary>
        /// Adds the query using the specified url.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="queryCollection">The query collection.</param>
        public static void AddQuery(this UriBuilder url, NameValueCollection queryCollection)
        {
            NameValueCollection currentValues = HttpUtility.ParseQueryString(url.Query);
            if (!currentValues.HasKeys())
            {
                if (queryCollection.HasKeys())
                {
                    url.Query = queryCollection.ConstructQueryString();
                }

                return;
            }

            foreach (object obj in queryCollection.Keys)
            {
                string key = (string)obj;
                currentValues.Set(key, queryCollection[key]);
            }

            url.Query = currentValues.ConstructQueryString();
        }
    }
}
