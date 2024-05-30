namespace Extended.System.Test
{
    /// <summary>
    /// The string test class.
    /// </summary>
    public class StringTest
    {
        /// <summary>
        /// Tests that md 5 hash.
        /// </summary>
        [Fact]
        public void MD5Hash()
        {
            var source = "Hello world";
            var md5Hash = "3e25960a79dbc69b674cd4ec67a72c62";
            var calculatedMd5 = source.ToMD5();
            Assert.Equal(calculatedMd5, md5Hash, true);
        }
    }
}
