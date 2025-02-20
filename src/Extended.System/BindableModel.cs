using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Extended.System
{
    /// <summary>
    /// The bindable model extensions class.
    /// </summary>
    public static class BindableModelExtensions
    {
        /// <summary>
        /// Adds the dependency using the specified bindable.
        /// </summary>
        /// <typeparam name="TBindable">The bindable.</typeparam>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="dependentProperty">The dependent property.</param>
        /// <param name="triggerProperties">The trigger properties.</param>
        /// <returns>Modified object.</returns>
        public static TBindable AddDependency<TBindable>(this TBindable bindable, Expression<Func<TBindable, dynamic>> dependentProperty, params Expression<Func<TBindable, dynamic>>[] triggerProperties) where TBindable : BindableModel
        {
            bindable?.AddDependency(dependentProperty.GetPropertyName(), triggerProperties.Select(x => x.GetPropertyName()).ToArray());
            return bindable!;
        }
    }

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
        private Dictionary<string, dynamic?> _context = new();

        /// <summary>
        /// The depends.
        /// </summary>
        private Dictionary<string, HashSet<string>> _depends = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableModel"/> class.
        /// </summary>
        protected BindableModel()
        {
            PropertyChanged += BindableModel_PropertyChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BindableModel"/> class.
        /// </summary>
        ~BindableModel()
        {
            PropertyChanged -= BindableModel_PropertyChanged;
        }

        /// <summary>
        /// Gets the property using the specified member name.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <returns>The dynamic.</returns>
        protected dynamic? GetProperty([CallerMemberName] string? memberName = null)
        {
            CheckIsNull(memberName);
            return _context.TryGetValue(memberName!, out dynamic? value) ? value : this.GetPropertyType(memberName)?.GetDefaultValue();
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
            if (_context.TryGetValue(memberName!, out dynamic? prevValue))
            {
                if (!(prevValue?.Equals(value) ?? value == null))
                {
                    _context[memberName!] = value;
                    OnPropertyChanged(this, memberName);
                }
            }
            else
            {
                _context.Add(memberName!, value);
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
            if (_context.TryGetValue(memberName!, out dynamic? prevValue))
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
                _context.Add(memberName!, new BindingList<T>(value));
                OnPropertyChanged(this, memberName);
            }
        }

        /// <summary>
        /// Adds the dependency using the specified dependent property.
        /// </summary>
        /// <param name="dependentProperty">The dependent property.</param>
        /// <param name="triggerProperties">The properties.</param>
        protected internal void AddDependency(string dependentProperty, params string[] triggerProperties)
        {
            triggerProperties.ForEach(tp =>
            {
                if (_depends.TryGetValue(dependentProperty, out HashSet<string>? value) && value.Any(d => d == tp))
                    throw new LoopTriggerException(tp, dependentProperty);

                _depends.SetOrAdd(tp, dependentProperty);
            });
        }

        /// <summary>
        /// Bindables the model property changed using the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The .</param>
        private void BindableModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var property = e.PropertyName;
            if (property != null)
            {
                if (_depends.TryGetValue(property, out HashSet<string>? value))
                {
                    value.ForEach(x => OnPropertyChanged(sender, x));
                }
            }
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
            memberName.CheckIsNull(nameof(memberName));
        }
    }

    /// <summary>
    /// The loop trigger exception class.
    /// </summary>
    /// <seealso cref="Exception"/>
    public sealed class LoopTriggerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoopTriggerException"/> class.
        /// </summary>
        /// <param name="triggerProperty">The trigger property.</param>
        /// <param name="dependentProperty">The dependent property.</param>
        public LoopTriggerException(string triggerProperty, string dependentProperty) : base($"Trying add trigger property {triggerProperty} for property {dependentProperty} throw loop exception")
        {
        }
    }
}
