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
            return session.TryGetValue(memberName!, out dynamic? value) ? value : this.GetPropertyType(memberName)?.GetDefaultValue();
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
            OnPropertyChanging(memberName);
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(memberName));
            if (session.TryGetValue(memberName!, out dynamic? prevValue))
            {
                if (!(prevValue?.Equals(value) ?? value == null))
                {
                    session[memberName!] = value;
                    OnPropertyChanged(this, memberName);
                }
            }
            else
            {
                session.Add(memberName!, value);
                OnPropertyChanged(this, memberName);
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
            OnPropertyChanging(memberName);
            if (session.TryGetValue(memberName!, out dynamic? prevValue))
            {
                if (!(prevValue?.Equals(value) ?? value == null))
                {
                    prevValue!.Clear();
                    if (value != null)
                    {
                        foreach (var item in value)
                        {
                            prevValue?.Add(item);
                        }
                    }

                    OnPropertyChanged(this, memberName);
                }
            }
            else
            {
                session.Add(memberName!, new BindingList<T>(value));
                OnPropertyChanged(this, memberName);
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
                    OnPropertyChanged(s, dependentProperty);
            };
        }

        /// <summary>
        /// Raises the property changed using the specified property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void OnPropertyChanged(string? propertyName)
        {
            OnPropertyChanged(this, propertyName);
        }

        /// <summary>
        /// Ons the property changing using the specified property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void OnPropertyChanging(string? propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Fires the property changed using the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">The property name.</param>
        protected void OnPropertyChanged(object? sender, string? propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Checks the is null using the specified member name.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <exception cref="ArgumentNullException">Posible exception.</exception>
        private static void CheckIsNull(string? memberName)
        {
            if (memberName == null)
#if NET6_0_OR_GREATER
                ArgumentNullException.ThrowIfNull(nameof(memberName));
#else
           throw new ArgumentNullException(nameof(memberName));
#endif

        }
    }
}
