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
    }
}
