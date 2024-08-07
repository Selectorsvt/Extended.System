using Timer = System.Timers.Timer;

namespace Extended.System
{
    /// <summary>
    /// The scheduled timer class.
    /// </summary>
    public sealed class ScheduledTimer : IDisposable
    {
        /// <summary>
        /// The timer.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The is scheduled.
        /// </summary>
        private bool IsScheduled { get; set; }

        /// <summary>
        /// The async delegate.
        /// </summary>
        public event Func<CancellationToken, Task>? AsyncDelegate;
        private CancellationTokenSource? cancellationTokenSource;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTimer"/> class.
        /// </summary>
        public ScheduledTimer()
        {
            timer = new Timer();
            timer.Elapsed += SyncTimer_Elapsed;
            timer.AutoReset = false;
        }

        /// <summary>
        /// Syncs the timer elapsed using the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The .</param>
        private async void SyncTimer_Elapsed(object? sender, global::System.Timers.ElapsedEventArgs e)
        {
            var token = cancellationTokenSource!.Token;
            await Invoke(token).ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes the cancellation token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="Exception">{nameof(AsyncDelegate)} not initialized.</exception>
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        public async Task Invoke(CancellationToken cancellationToken)
        {
            if (IsScheduled)
                timer.Stop();

            await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (AsyncDelegate != null)
                    await AsyncDelegate.Invoke(cancellationToken).ConfigureAwait(false);
                else
                    throw new Exception($"{nameof(AsyncDelegate)} not initialized.");
            }
            finally
            {
                semaphoreSlim.Release();
                if (IsScheduled)
                    timer.Start();
            }
        }

        /// <summary>
        /// Starts the time span.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        public void Start(TimeSpan timeSpan)
        {
            if (!IsScheduled)
            {
                timer.Interval = timeSpan.TotalMilliseconds;
                cancellationTokenSource = new CancellationTokenSource();
                timer.Start();
                IsScheduled = true;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            timer.Stop();
            IsScheduled = false;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
            cancellationTokenSource?.Dispose();
            semaphoreSlim.Dispose();
        }
    }
}
