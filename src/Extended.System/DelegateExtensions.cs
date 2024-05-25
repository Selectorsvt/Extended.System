using System.Runtime.CompilerServices;

namespace Extended.System
{
    /// <summary>
    /// The delegate extensions class.
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// Describes whether is.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <returns>The bool.</returns>
        public static bool IsAsync(this Delegate @delegate)
        {
            if (@delegate == null)
                return false;

            return @delegate.Method.IsDefined(typeof(AsyncStateMachineAttribute), false);
        }

        /// <summary>
        /// Invokes the whith lock using the specified delegate.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="lockObject">The lock object.</param>
        /// <param name="args">The args.</param>
        public static void InvokeWhithLock(this Delegate @delegate, object lockObject, params object[] args)
        {
            var hasLock = false;
            try
            {
                Monitor.TryEnter(lockObject, ref hasLock);
                if (!hasLock)
                    return;

                @delegate?.DynamicInvoke(args);
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(lockObject);
                }
            }
        }
    }
}
