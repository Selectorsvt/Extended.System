using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Extended.System
{
    /// <summary>
    /// The string extensions class.
    /// </summary>
    public static class StringExtensions
    {
        private static Regex htmlTagsRegex = new Regex(@"<[^>]*>");

        /// <summary>
        /// Returns the enum using the specified value.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The.</returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Erases the html using the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The string.</returns>
        public static string EraseHtml(this string value)
        {
            return htmlTagsRegex.Replace(value, string.Empty).Replace("&nbsp;", " ");
        }

        /// <summary>
        /// Returns the bytes using the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ToBytes(this string value, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Hashes the hmac using the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="key">The key.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static byte[] HashHMAC(this string message, string key, Encoding? encoding = null)
        {
            var keyBytes = key.ToBytes(encoding);
            return message.HashHMAC(keyBytes, encoding);
        }

        /// <summary>
        /// Hashes the hmac using the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="keyBytes">The key bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The byte array.</returns>
        public static byte[] HashHMAC(this string message, byte[] keyBytes, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            var messageBytes = message.ToBytes(encoding);
            var hash = new HMACSHA256(keyBytes);
            return messageBytes.ComputedHash(hash);
        }

        /// <summary>
        /// Gets the name without extension using the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The string.</returns>
        public static string GetNameWithoutExtension(this string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Gets the file info using the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The file info.</returns>
        public static FileInfo? GetFileInfo(this string? path)
        {
            return path != null && File.Exists(path) ? new FileInfo(path) : null;
        }

        /// <summary>
        /// Returns the md 5 using the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The string.</returns>
        public static string ToMD5(this string input, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

#if NET6_0_OR_GREATER
            byte[] inputBytes = encoding.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);

            return Convert.ToHexString(hashBytes);
#else
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = encoding.GetBytes(input);
                byte[] hashBytes = inputBytes.ComputedHash(md5);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
#endif
        }
    }
}
