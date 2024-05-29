using System.Security.Cryptography;

namespace Extended.System
{
    /// <summary>
    /// The stream extensions class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Calculates the md 5 using the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The string.</returns>
        public static string CalculateMD5(this Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
#if !NETSTANDARD2_1 && !NET6_0_OR_GREATER
        /// <summary>
        /// Copies the to using the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task CopyToAsync(this Stream source, Stream destination, CancellationToken cancellationToken)
        {
           await source.CopyToAsync(destination).ConfigureAwait(false);
        }
#endif
    }
}
