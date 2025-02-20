namespace Extended.System.Test
{
    /// <summary>
    /// The bindable model test class.
    /// </summary>
    public class BindableModelTest
    {
        /// <summary>
        /// Tests that check loop.
        /// </summary>
        [Fact]
        public void CheckLoop()
        {
            Assert.Throws<LoopTriggerException>(() => new TestBindableModel());
        }
    }

    /// <summary>
    /// The test bindable model class.
    /// </summary>
    /// <seealso cref="BindableModel"/>
    public class TestBindableModel : BindableModel
    {
        /// <summary>
        /// Gets or sets the value of the my property one.
        /// </summary>
        public int MyPropertyOne { get; set; }

        /// <summary>
        /// Gets or sets the value of the my property two.
        /// </summary>
        public int MyPropertyTwo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBindableModel"/> class.
        /// </summary>
        public TestBindableModel()
        {
            this.AddDependency(x => x.MyPropertyOne, x => x.MyPropertyTwo)
                .AddDependency(x => x.MyPropertyTwo, x => x.MyPropertyOne);
        }
    }
}
