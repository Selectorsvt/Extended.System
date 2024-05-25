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
    }
}
