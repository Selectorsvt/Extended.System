using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Extended.System
{
    /// <summary>
    /// The enumerable extensions class.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the encoded string using the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The string.</returns>
        public static string ToEncodedString(this IEnumerable<byte> bytes, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return encoding.GetString(bytes.ToArray());
        }

        /// <summary>
        /// Returns the hex string using the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The string.</returns>
        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            var byteArray = bytes.ToArray();
#if NET6_0_OR_GREATER
            return Convert.ToHexString(byteArray);
#else
            var hexString = BitConverter.ToString(byteArray);
            return hexString.Replace("-", "");
#endif
        }

        /// <summary>
        /// Returns the base 64 string using the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The string.</returns>
        public static string ToBase64String(this IEnumerable<byte> bytes)
        {
            return Convert.ToBase64String(bytes.ToArray());
        }

        /// <summary>
        /// Computeds the hash using the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ComputedHash(this IEnumerable<byte> bytes, HashAlgorithm hashAlgorithm)
        {
            if (hashAlgorithm == null)
                throw new ArgumentException("Not valid hash algorithm", nameof(hashAlgorithm));

            using (hashAlgorithm)
            {
                return hashAlgorithm.ComputeHash(bytes.ToArray());
            }
        }

        /// <summary>
        /// Splits the array using the specified array.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>The columns.</returns>
        public static ReadOnlyCollection<T[]> SplitArray<T>(this IEnumerable<T> array, int numberOfColumns)
        {
            int totalItems = array.Count();
            int itemsPerColumn = (int)Math.Ceiling(totalItems / (double)numberOfColumns);
            var columns = new List<T[]>();

            for (int i = 0; i < numberOfColumns; i++)
            {
                int take = itemsPerColumn;
                // If we are at the last column, only take what is left
                if (i == numberOfColumns - 1)
                {
                    take = totalItems - (itemsPerColumn * i);
                }

                columns.Add(array.Skip(i * itemsPerColumn).Take(take).ToArray());
            }

            return new ReadOnlyCollection<T[]>(columns);
        }

        /// <summary>
        /// Describes whether has item.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>The bool.</returns>
        public static bool HasItems<T>(this IEnumerable<T>? array)
        {
            return array != null && array.Any();
        }

        /// <summary>
        /// Returns the separated string using the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The string.</returns>
        public static string ToSeparatedString(this IEnumerable<string?> values, string separator)
        {
            return string.Join(separator, values);
        }

        /// <summary>
        /// Fors the each using the specified objects.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="objects">The objects.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> objects, Action<T> action)
        {
            foreach (var obj in objects)
            {
                action(obj);
            }
        }

        /// <summary>
        /// Fors the each using the specified objects.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="objects">The objects.</param>
        /// <param name="action">The action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> objects, Func<T, CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            await foreach (var obj in objects.WithCancellation(cancellationToken))
            {
                await action(obj, cancellationToken);
            }
        }

        /// <summary>
        /// Fors the each using the specified objects.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="objects">The objects.</param>
        /// <param name="action">The action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> objects, Func<T, CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            foreach (var obj in objects)
            {
                await action(obj, cancellationToken);
            }
        }

        /// <summary>
        /// Returns the separated string using the specified objects.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="objects">The objects.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The separated string.</returns>
        public static string ToSeparatedString<T>(this IEnumerable<T> objects, char separator)
        {
#if NET6_0_OR_GREATER
            return string.Join(separator, objects);
#else
            return objects.Select(x => x?.ToString()).ToSeparatedString(separator.ToString());
#endif
        }

        /// <summary>
        /// Wheres the if using the specified enumerable.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>An enumerable of T objects.</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Func<bool> condition)
        {
            return condition() ? enumerable.Where(predicate) : enumerable;
        }
    }
}
