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
    }
}
