using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SwipeFlash
{
    /// <summary>
    /// A dependency property moving the input bindings from a framework element to its container window
    /// Only works once, cannot toggle
    /// </summary>
    public class InputBindingBehavior
    {
        #region Dependency Property
        
        public static bool GetPropagateInputBindingsToWindow(FrameworkElement obj)
        {
            return (bool)obj.GetValue(PropagateInputBindingsToWindowProperty);
        }

        public static void SetPropagateInputBindingsToWindow(FrameworkElement obj, bool value)
        {
            obj.SetValue(PropagateInputBindingsToWindowProperty, value);
        }

        public static readonly DependencyProperty PropagateInputBindingsToWindowProperty =
                               DependencyProperty.RegisterAttached("PropagateInputBindingsToWindow", typeof(bool), typeof(InputBindingBehavior),
                               new PropertyMetadata(false, OnPropagateInputBindingsToWindowChanged));

        /// <summary>
        /// On Dependency Property changed, binds <see cref="frameworkElement_Loaded(object, RoutedEventArgs)"/> to the <see cref="FrameworkElement"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPropagateInputBindingsToWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Check if dependency object is a framework element
            if (!(d is FrameworkElement element))
                return;

            // If the element hasn't yet been loaded
            if (!element.IsLoaded)
                // Hook the helper method to the Loaded event
                element.Loaded += MoveInputBindingsToWindow;
            else
                // Manually run the method
                MoveInputBindingsToWindow(element, null);
        }

        #endregion

        /// <summary>
        /// Moves the input bindings to the container window
        /// </summary>
        /// <param name="sender">The <see cref="FrameworkElement"/> element</param>
        /// <param name="e"></param>
        private static void MoveInputBindingsToWindow(object sender, RoutedEventArgs e)
        {
            // Check if sender is a framework element
            if (!(sender is FrameworkElement element))
                return;

            // Unhook from the Loaded RoutedEventHandler
            element.Loaded -= MoveInputBindingsToWindow;

            // Get the current window
            var window = Window.GetWindow(element);

            // If the window is invalid
            if (window == null)
                return;

            // Move input bindings from the FrameworkElement to the window.
            for (int i = element.InputBindings.Count - 1; i >= 0; i--)
            {
                // Get input bindings
                var inputBinding = element.InputBindings[i];
                // Add input bindings to window
                window.InputBindings.Add(inputBinding);
                // Remove input bindings from element
                element.InputBindings.Remove(inputBinding);
            }
        }
    }
}
