using Timer = System.Timers.Timer;

namespace Extended.System
{
    /// <summary>
    /// The concurent timer process class.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public class ConcurentTimerProcess : IDisposable
    {
        /// <summary>
        /// The locker.
        /// </summary>
        private object _locker = new object();

        /// <summary>
        /// The timer.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The action.
        /// </summary>
        private Action _action;

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurentTimerProcess"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="timeSpan">The time span.</param>
        public ConcurentTimerProcess(Action action, TimeSpan timeSpan)
        {
#if NET7_0_OR_GREATER
            _timer = new Timer(timeSpan);
#else
            _timer = new Timer(timeSpan.TotalMilliseconds);
#endif
            _action = action;
            _timer.Elapsed += Callback;
        }

        /// <summary>
        /// Callbacks the sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The .</param>
        private void Callback(object? sender, global::System.Timers.ElapsedEventArgs e)
        {
            _action.InvokeWhithLock(_locker);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the disposing.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                _timer.Elapsed -= Callback;
                _timer.Dispose();
            }

            isDisposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ConcurentTimerProcess"/> class.
        /// </summary>
        ~ConcurentTimerProcess()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
