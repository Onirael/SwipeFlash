using System.Windows;

namespace SwipeFlash
{
    #region Base Properties

    /// <summary>
    /// A base attached property class to run animations
    /// </summary>
    /// <typeparam name="Parent"></typeparam>
    public abstract class AnimateBaseTriggeredProperty<Parent> : BaseAttachedProperty<Parent, bool>
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

    /// <summary>
    /// A base class to run any animation method when a boolean is set to true
    /// and reverse the animation when set to false
    /// </summary>
    /// <typeparam name="Parent"></typeparam>
    public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
        where Parent : BaseAttachedProperty<Parent, bool>, new()
    {
        #region Public Properties

        /// <summary>
        /// A flag indicating whether this is the first time this property has been loaded
        /// </summary>
        public bool IsFirstLoad { get; set; } = true;

        #endregion

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            // Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            // Don't fire if the value doesn't change
            if (sender.GetValue(ValueProperty) == value && !IsFirstLoad)
                return;

            // On first load
            if (IsFirstLoad)
            {
                // Create a single self-unhookable event
                //      hooked to the element's Loaded RoutedEventHandler
                RoutedEventHandler onElementLoad = null;
                onElementLoad = (ss, ee) =>
                {
                    // Unhook the event
                    element.Loaded -= onElementLoad;

                    // Run the animation
                    RunAnimation(element, (bool)value);

                    // Set IsFirstLoad
                    IsFirstLoad = false;
                };

                element.Loaded += onElementLoad;

            }
            else
            {
                // Run the animation
                RunAnimation(element, (bool)value);
            }
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
    public class AnimateFlipProperty : AnimateBaseTriggeredProperty<AnimateFlipProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            if (!(element is IFlippableElement))
                return;

            await element.HorizontalFlipAsync(((IFlippableElement)element).FlipAnimDuration);
        }
    }

    /// <summary>
    /// Animates a framework element, swiping it to the left with a slight tilt
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateSwipeLeftProperty : AnimateBaseTriggeredProperty<AnimateSwipeLeftProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            if (value)
                await element.SlideAndTiltToLeftAsync((int)(Application.Current.MainWindow.Width * 1.2), 20, duration: 0.4f);
            else
                await element.SlideAndTiltFromLeftAsync((int)(Application.Current.MainWindow.Width * 1.2), 20, duration: 0.2f);
        }
    }

    /// <summary>
    /// Animates a framework element, swiping it to the right with a slight tilt
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateSwipeRightProperty : AnimateBaseTriggeredProperty<AnimateSwipeRightProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            if (value)
                await element.SlideAndTiltToRightAsync((int)(Application.Current.MainWindow.Width * 1.2), 20, duration: 0.4f);
            else
                await element.SlideAndTiltFromRightAsync((int)(Application.Current.MainWindow.Width * 1.2), 20, duration: 0.2f);
        }
    }

    /// <summary>
    /// Animates a framework element, sliding it down
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateSlideDownProperty : AnimateBaseProperty<AnimateSlideDownProperty>
    {
        protected override void RunAnimation(FrameworkElement element, bool value)
        {
            // Slide down animation
        }
    }

    /// <summary>
    /// Animates a framework element, fading it in
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
    {
        protected override void RunAnimation(FrameworkElement element, bool value)
        {
            // Fade in animation
        }
    }

}