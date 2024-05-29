#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
#if NET462
using System.Net.Http;
#endif

namespace Extended.System
{
    /// <summary>
    /// The http client extensions class.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Downloads the file task using the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="cancellationToken">The token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task DownloadFileTaskAsync(this HttpClient client,
#if NET7_0_OR_GREATER
            [StringSyntax(StringSyntaxAttribute.Uri)] string uri,
#else
            string uri,
#endif
            string fileName,
            CancellationToken cancellationToken = default)
        {
            await client.DownloadFileTaskAsync(new Uri(uri), fileName, cancellationToken).ConfigureAwait(false);
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
    }
}
