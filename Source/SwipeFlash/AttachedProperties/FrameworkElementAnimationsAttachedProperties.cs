using System.Windows;

namespace SwipeFlash
{
    #region Base Property

    /// <summary>
    /// A base class to run any animation method when a boolean is set to true
    /// and reverse the animation when set to false
    /// </summary>
    /// <typeparam name="Parent"></typeparam>
    public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
        where Parent : BaseAttachedProperty<Parent, bool>, new()
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var value = (bool)e.NewValue;

            // Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            // Don't fire if the value doesn't change
            if (sender.GetValue(ValueProperty) == (object)value)
                return;

            // Run the animation
            RunAnimation(element, value);
        }

        /// <summary>
        /// The animation method that is fired when the value changes
        /// </summary>
        /// <param name="element">The affected element</param>
        /// <param name="value">The new value</param>
        protected virtual void RunAnimation(FrameworkElement element, bool value) { }
    }

    #endregion

    /// <summary>
    /// Animates a framework element, flipping it horizontally
    /// </summary>
    public class AnimateFlipProperty : AnimateBaseProperty<AnimateFlipProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            if (!(element is IFlippableElement))
                return;

            await element.HorizontalFlipAsync(((IFlippableElement)element).FlipAnimDuration);
        }
    }

    /// <summary>
    /// Animates a framework element, flipping it horizontally
    /// </summary>
    public class AnimateSwipeLeftProperty : AnimateBaseProperty<AnimateSwipeLeftProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            await element.SlideAndTiltToLeftAsync((int)(Application.Current.MainWindow.Width * 1.2), 20, duration: 0.4f);
        }
    }

    /// <summary>
    /// Animates a framework element, flipping it horizontally
    /// </summary>
    public class AnimateSwipeRightProperty : AnimateBaseProperty<AnimateSwipeRightProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            await element.SlideAndTiltToRightAsync((int)(Application.Current.MainWindow.Width * 1.2), 20, duration: 0.4f);
        }
    }

}