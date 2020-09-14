using SwipeFlash.Core;
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
    public class AnimateFlipProperty : AnimateBaseProperty<AnimateFlipProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            if (!(element is IFlippableElement))
                return;

            if (!IsFirstLoad)
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
            if (!IsFirstLoad)
                await element.SlideToLeftAsync((int)((int)Application.Current.MainWindow.Width), duration: 0.4f);
        }
    }    
    
    /// <summary>
    /// Animates a framework element, flipping it horizontally
    /// </summary>
    public class AnimateSwipeRightProperty : AnimateBaseProperty<AnimateSwipeRightProperty>
    {
        protected async override void RunAnimation(FrameworkElement element, bool value)
        {
            if (!IsFirstLoad)
                await element.SlideToRightAsync((int)((int)Application.Current.MainWindow.Width), duration: 0.4f);
        }
    }
}