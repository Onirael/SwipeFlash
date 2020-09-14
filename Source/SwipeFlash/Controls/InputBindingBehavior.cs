using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SwipeFlash
{
    /// <summary>
    /// A dependency property 
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
            ((FrameworkElement)d).Loaded += frameworkElement_Loaded;
        }

        #endregion

        /// <summary>
        /// On <see cref="FrameworkElement.Loaded"/> moves the input bindings to the container window
        /// </summary>
        /// <param name="sender">The <see cref="FrameworkElement"/> element</param>
        /// <param name="e"></param>
        private static void frameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;
            frameworkElement.Loaded -= frameworkElement_Loaded;

            var window = Window.GetWindow(frameworkElement);
            if (window == null)
            {
                return;
            }

            // Move input bindings from the FrameworkElement to the window.
            for (int i = frameworkElement.InputBindings.Count - 1; i >= 0; i--)
            {
                var inputBinding = (InputBinding)frameworkElement.InputBindings[i];
                window.InputBindings.Add(inputBinding);
                frameworkElement.InputBindings.Remove(inputBinding);
            }
        }
    }
}
