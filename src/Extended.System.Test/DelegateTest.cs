namespace Extended.System.Test
{
    /// <summary>
    /// The unit test class.
    /// </summary>
    public class DelegateTest
    {
        /// <summary>
        /// Tests that test 1.
        /// </summary>
        [Fact]
        public void AsyncDelegate()
        {
            var isAsync = new Action(async () => { await Task.Delay(10).ConfigureAwait(true); }).IsAsync();
            Assert.True(isAsync);
        }

        /// <summary>
        /// Tests that test 1.
        /// </summary>
        [Fact]
        public void NotAsync()
        {
            var isAsync = new Action(() => { }).IsAsync();
            Assert.False(isAsync);
        }
    }
}