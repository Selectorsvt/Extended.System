using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Extended.System
{
    /// <summary>
    /// The bindable model class.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged"/>
    /// <seealso cref="INotifyPropertyChanging"/>
    public abstract class BindableModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// The Property Changing event.
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// The Property Changed event.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// The session.
        /// </summary>
        private Dictionary<string, dynamic?> session = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableModel"/> class.
        /// </summary>
        protected BindableModel()
        {
        }

        /// <summary>
        /// Gets the property using the specified member name.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <returns>The dynamic.</returns>
        protected dynamic? GetProperty([CallerMemberName] string? memberName = null)
        {
            CheckIsNull(memberName);
            return session.ContainsKey(memberName!) ? session[memberName!] : default;
        }

        /// <summary>
        /// Sets the property using the specified value.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="memberName">The member name.</param>
        protected void SetProperty<T>(T value, [CallerMemberName] string? memberName = null)
        {
            CheckIsNull(memberName);

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(memberName));
            if (session.ContainsKey(memberName!))
            {
                var prevValue = session[memberName!];

                if (!prevValue?.Equals(value) ?? value != null)
                {
                    session[memberName!] = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
                }
            }
            else
            {
                session.Add(memberName!, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
            }
        }

        /// <summary>
        /// Sets the property using the specified value.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="memberName">The member name.</param>
        protected void SetProperty<T>(IList<T> value, [CallerMemberName] string? memberName = null)
        {
            CheckIsNull(memberName);

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(memberName));
            if (session.ContainsKey(memberName!))
            {
                var prevValue = session[memberName!]!;

                if (!prevValue?.Equals(value) ?? value != null)
                {
                    prevValue!.Clear();
                    if (value != null)
                    {
                        foreach (var item in value)
                        {
                            prevValue?.Add(item);
                        }
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
                }
            }
            else
            {
                session.Add(memberName!, new BindingList<T>(value));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
            }
        }

        /// <summary>
        /// Adds the dependency using the specified dependent property.
        /// </summary>
        /// <param name="dependentProperty">The dependent property.</param>
        /// <param name="properties">The properties.</param>
        protected void AddDependency(string dependentProperty, string[] properties)
        {
            PropertyChanged += (s, e) =>
            {
                if (properties.Contains(e.PropertyName))
                    PropertyChanged?.Invoke(s, new PropertyChangedEventArgs(dependentProperty));
            };
        }

        /// <summary>
        /// Checks the is null using the specified member name.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <exception cref="ArgumentNullException">Posible exception.</exception>
        private void CheckIsNull(string? memberName)
        {
            if (memberName == null)
                throw new ArgumentNullException(nameof(memberName));
        }
    }
}
