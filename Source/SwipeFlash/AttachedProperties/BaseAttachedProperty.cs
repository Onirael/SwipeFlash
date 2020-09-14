using System;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A base attached property to replace the vanilla WPF attached property
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    /// <typeparam name="Property">The type of this attached property</typeparam>
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : new()
    {
        #region Public Events

        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        /// <summary>
        /// Fired when the value is updated, regardless of whether its value changed
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// A singleton instance of our parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        #endregion

        #region Attached Property Definitions

        /// <summary>
        /// The attached property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value",
                                                typeof(Property),
                                                typeof(BaseAttachedProperty<Parent, Property>),
                                                new PropertyMetadata(
                                                            default(Property),
                                                            new PropertyChangedCallback(OnValuePropertyChanged),
                                                            new CoerceValueCallback(OnValuePropertyUpdated)
                                                            ));

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI element that had its property changed</param>
        /// <param name="e">The arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call the parent function
            // Here the instance is cast in the code whenever Instance is called because XAML doesn't support generic types
            // Otherwise we would use where Parent : BaseAttachedProperty<Parent, Property>, new() instead of just where Parent : new()
            // The ? checks whether Instance is null
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e);

            // Call event listeners
            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueChanged(d, e);
        }

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is updated, regardless of whether its value changed
        /// </summary>
        /// <param name="d">The UI element that had its property changed</param>
        /// <param name="value">The new value of the property</param>
        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            // Call the parent function
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueUpdated(d, value);

            // Call event listeners
            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueUpdated(d, value);

            // Return the value
            return value;
        }

        /// <summary>
        /// Getter for the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="d">The element to get the property from</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The method that is called when any attached property of this type is changed
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="e">The arguments for this event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// The method that is called when any attached property of this type is updated, regardless of whether it has changed
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="value">The new value of this property</param>
        public virtual void OnValueUpdated(DependencyObject sender, object value) { }

        #endregion
    }
}
