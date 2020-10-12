using System.ComponentModel;
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
        #region Public Properties

        /// <summary>
        /// The duration of the animation in seconds
        /// </summary>
        public float AnimDuration { get; set; } = 0.5f;

        #endregion

        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var value = (bool)e.NewValue;

            // Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            // Don't fire if the value doesn't change
            if (sender.GetValue(ValueProperty) == (object)value)
                return;

            // Don't fire if in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
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

        /// <summary>
        /// The duration of the animation in seconds
        /// </summary>
        public float AnimDuration { get; set; } = 0.5f;

        #endregion

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            // Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            // Don't fire if the value doesn't change
            if (sender.GetValue(ValueProperty) == value && !IsFirstLoad)
                return;

            // Don't fire if in design mode
            if (DesignerProperties.GetIsInDesignMode(element))
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
    public class AnimateSlideInFromTopProperty : AnimateBaseProperty<AnimateSlideInFromTopProperty>
    {
        // Initializes constants
        public AnimateSlideInFromTopProperty() : base() { AnimDuration = 0.4f; }

        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            // If the animation is run forward
            if (value)
                // Slide the element in from the top
                await element.SlideFromTopAsync((int)(Application.Current.MainWindow.Width * 1.2), IsFirstLoad ? 0f : AnimDuration);
            else
                // Slide the element to the top
                await element.SlideToTopAsync((int)(Application.Current.MainWindow.Width * 1.2), IsFirstLoad ? 0f : AnimDuration);
        }
    }

    /// <summary>
    /// Animates a framework element, fading it in
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            //If the the animation is run forward
            if (value)
                // Fade the element in
                await element.FadeInAsync(IsFirstLoad ? 0f : 0.4f);
            else
                // Fade the element out
                await element.FadeOutAsync(IsFirstLoad ? 0f : 0.4f);

        }
    }

    /// <summary>
    /// Animates a framework element, scaling and fading it in
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateScaleAndFadeInProperty : AnimateBaseProperty<AnimateScaleAndFadeInProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            // If the animation is run forward
            if (value)
                // Scale and fade the element in 
                await element.ScaleAndFadeInAsync(0.7, 0.2f);
            else
                // Scale and fade the element out
                await element.ScaleAndFadeOutAsync(0.7, 0.2f);
        }
    }

    #region Instances

    /// <summary>
    /// Animates the settings menu slide in and out
    /// </summary>
    public class AnimateSettingsSlideInProperty : AnimateBaseProperty<AnimateSettingsSlideInProperty>
    {
        // Initializes constants
        public AnimateSettingsSlideInProperty() : base() { AnimDuration = 0.4f; }

        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            // If the animation is run forward
            if (value)
                // Slide the element in from the top
                await element.SlideFromTopAsync((int)(Application.Current.MainWindow.Width * 1.2), IsFirstLoad ? 0f : AnimDuration);
            else
                // Slide the element to the top
                await element.SlideToTopAsync((int)(Application.Current.MainWindow.Width * 1.2), IsFirstLoad ? 0f : AnimDuration);
        }
    }

    /// <summary>
    /// Animates 
    /// </summary>
    public class AnimateContentSlideInFromTopProperty : AnimateBaseProperty<AnimateContentSlideInFromTopProperty>
    {
        // Initializes constants
        public AnimateContentSlideInFromTopProperty() : base() { AnimDuration = 0.2f; }

        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            // If the animation is run forward
            if (value)
                // Slide the element in from the top
                await element.SlideFromTopAsync((int)element.ActualHeight, IsFirstLoad ? 0f : AnimDuration);
            else
                // Slide the element to the top
                await element.SlideToTopAsync((int)element.ActualHeight, IsFirstLoad ? 0f : AnimDuration);
        }
    }

    /// <summary>
    /// Animates a framework element, fading it in
    /// if the value is set to false, the animation is reversed
    /// </summary>
    public class AnimateContentFadeInProperty : AnimateBaseProperty<AnimateContentFadeInProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            //If the the animation is run forward
            if (value)
                // Fade the element in
                await element.FadeInAsync(IsFirstLoad ? 0f : 0.4f);
            else
                // Fade the element out
                await element.FadeOutAsync(IsFirstLoad ? 0f : 0.4f);

        }
    }

    #endregion

}