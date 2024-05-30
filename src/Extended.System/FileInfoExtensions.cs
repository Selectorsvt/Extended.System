namespace Extended.System
{
    /// <summary>
    /// The file info extensions class.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Calculates the md 5 using the specified file info.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <returns>The string.</returns>
        public static string? CalculateMD5(this FileInfo? fileInfo)
        {
            if (fileInfo == null)
                return null;

            using (var fileStream = fileInfo.OpenRead())
            {
                return fileStream.CalculateMD5();
            }
        }

        /// <summary>
        /// Writes the json obj using the specified file info.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="obj">The obj.</param>
        public static void WriteJsonObj<T>(this FileInfo fileInfo, T obj) where T : class
        {
            using (var fileStream = fileInfo.Open(FileMode.OpenOrCreate))
            {
                fileStream.SetLength(0);

                if (obj == null)
                {
                    fileStream.Close();
                    fileInfo.Delete();
                    return;
                }

                using (var sw = new StreamWriter(fileStream))
                {
                    var jsonString = obj.SerializeJson();
                    sw.Write(jsonString);
                }
            }
        }

        /// <summary>
        /// Writes the json obj using the specified file info.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public static async Task WriteJsonObjAsync<T>(this FileInfo fileInfo, T obj, CancellationToken cancellationToken = default) where T : class
        {
            using (var fileStream = fileInfo.Open(FileMode.OpenOrCreate))
            {
                fileStream.SetLength(0);

                if (obj == null)
                {
                    fileStream.Close();
                    fileInfo.Delete();
                    return;
                }

                using (var sw = new StreamWriter(fileStream))
                {
                    var jsonString = obj.SerializeJson();
                    await sw.WriteAsync(jsonString).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Gets the json obj using the specified file info.
        /// </summary>
        /// <typeparam name="T">The object.</typeparam>
        /// <param name="fileInfo">The file info.</param>
        /// <returns>Object.</returns>
        public static T? GetJsonObj<T>(this FileInfo fileInfo) where T : class
        {
            using (var fileStream = fileInfo.Open(FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    var jsonString = sr.ReadToEnd();
                    return jsonString.DeserializeJsonObject<T>();
                }
            }
        }

        /// <summary>
        /// Gets the json obj using the specified file info.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task containing the object.</returns>
        public static async Task<T?> GetJsonObjAsync<T>(this FileInfo fileInfo, CancellationToken cancellationToken = default) where T : class
        {
            using (var fileStream = fileInfo.Open(FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fileStream))
                {
#if !NET7_0_OR_GREATER
                    var jsonString = await sr.ReadToEndAsync().ConfigureAwait(false);
#else
                    var jsonString = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
#endif
                    return jsonString.DeserializeJsonObject<T>();
                }
            }
        }
    }
}
