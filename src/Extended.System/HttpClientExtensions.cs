#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
#if NET462
using System.Net.Http;
#endif
using System.Net.Http.Headers;

namespace Extended.System
{
    /// <summary>
    /// The http client extensions class.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// The chrome header.
        /// </summary>
        private const string ChromeHeader = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

        /// <summary>
        /// Downloads the file task using the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="cancellationToken">The token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task DownloadFileTaskAsync(
            this HttpClient client,
#if NET7_0_OR_GREATER
            [StringSyntax(StringSyntaxAttribute.Uri)] string uri,
#else
            string uri,
#endif
            string fileName,
            CancellationToken cancellationToken = default)
        {
            using (var s = await client.GetStreamAsync(uri, cancellationToken).ConfigureAwait(false))
            {
                using (var fs = new FileStream(fileName, FileMode.CreateNew))
                {
                    await s.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Downloads the file task using the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string fileName, CancellationToken cancellationToken = default)
        {
            using (var s = await client.GetStreamAsync(uri, cancellationToken).ConfigureAwait(false))
            {
                using (var fs = new FileStream(fileName, FileMode.CreateNew))
                {
                    await s.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);
                }
            }
        }

#if NET462 || NETSTANDARD2_0_OR_GREATER
        /// <summary>
        /// Get stream.
        /// </summary>
        /// <param name="client">Http Client.</param>
        /// <param name="requestUri">Uri.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task<Stream> GetStreamAsync(this HttpClient client, Uri? requestUri, CancellationToken cancellationToken)
        {
            return await client.GetStreamAsync(requestUri).ConfigureAwait(false);
        }

        /// <summary>
        /// Get stream.
        /// </summary>
        /// <param name="client">Http Client.</param>
        /// <param name="requestUri">Uri.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task<Stream> GetStreamAsync(this HttpClient client, string? requestUri, CancellationToken cancellationToken)
        {
            return await client.GetStreamAsync(requestUri).ConfigureAwait(false);
        }
#endif

        /// <summary>
        /// Adds the default header using the specified http client.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns>The modified http client.</returns>
        public static HttpClient AddDefaultHeader(this HttpClient httpClient, string name, params string?[] values)
        {
            httpClient.DefaultRequestHeaders.Add(name, values);
            return httpClient;
        }

        /// <summary>
        /// Adds the autorization header using the specified http client.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="sheme">The sheme.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The modified http client.</returns>
        public static HttpClient AddAutorizationHeader(this HttpClient httpClient, string sheme, string? parameters)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(sheme, parameters);
            return httpClient;
        }

        /// <summary>
        /// Adds the bearer authorization using the specified http client.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="token">The token.</param>
        /// <returns>The modified http client.</returns>
        public static HttpClient AddBearerAuthorization(this HttpClient httpClient,  string token)
        {
            return httpClient.AddAutorizationHeader("Bearer", token);
        }

        /// <summary>
        /// Adds the browser header using the specified http client.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="browserValue">The browser value.</param>
        /// <returns>The modified http client.</returns>
        public static HttpClient AddBrowserHeader(this HttpClient httpClient, string? browserValue = null)
        {
            return httpClient.AddDefaultHeader("user-agent", browserValue ?? ChromeHeader);
        }
    }
}
