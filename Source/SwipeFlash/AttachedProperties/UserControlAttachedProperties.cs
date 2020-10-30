using System;
using System.Windows;
using System.Windows.Input;

namespace SwipeFlash
{
    public class IsPendingDestroyProperty : BaseAttachedProperty<IsPendingDestroyProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && sender is FrameworkElement element)
            {
                // Removes the framework element from its parents and garbage collects its VM
                element.DataContext = null;
                element.RemoveFromParent();
                GC.Collect();
            }
        }
    }

    public class StoredInputBindingsProperty : BaseAttachedProperty<StoredInputBindingsProperty, InputBinding[]> { }

    /// <summary>
    /// An attached property to enable or disable input bindings on framework elements, 
    /// the input bindings are propagated to the window
    /// </summary>
    public class IsInputBindingEnabledProperty : BaseAttachedProperty<IsInputBindingEnabledProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Type check the sender
            if (!(sender is FrameworkElement element))
                return;

            // Gets the stored input bindings from the attached property
            var storedInputBindings = element.GetValue(StoredInputBindingsProperty.ValueProperty) as InputBinding[];

            // If the input bindings are set to enabled
            if ((bool)e.NewValue)
            {
                // If input bindings are stored and the element's input bindings are empty
                if (storedInputBindings != null && element.InputBindings.Count == 0)
                {
                    // For each stored binding
                    foreach (InputBinding binding in storedInputBindings)
                    {
                        // If the binding is not already in the element's input bindings
                        if (!element.InputBindings.Contains(binding))
                            // Add it to the collection
                            element.InputBindings.Add(binding);
                    }
                }

                // If the element hasn't yet been loaded
                if (!element.IsLoaded)
                    // Hook the helper method to the Loaded event
                    element.Loaded += CopyInputBindingsToWindow;
                else
                    // Manually run the method
                    CopyInputBindingsToWindow(element, null);

            }
            // If the input bindings are set to disabled
            else
            {
                // If the element was loaded
                if (element.IsLoaded)
                    RemoveCardBindingsFromWindow(element);

                // Store all input bindings and clear them on the element
                if (storedInputBindings == null)
                {
                    storedInputBindings = new InputBinding[element.InputBindings.Count];
                }

                // Copies the input bindings to the local array
                element.InputBindings.CopyTo(storedInputBindings, 0);
                element.InputBindings.Clear();

                // Stores the input bindings on the element
                element.SetValue(StoredInputBindingsProperty.ValueProperty, storedInputBindings);
            }
        }

        /// <summary>
        /// Moves the input bindings to the container window
        /// </summary>
        /// <param name="sender">The <see cref="FrameworkElement"/> element</param>
        /// <param name="e"></param>
        private void CopyInputBindingsToWindow(object sender, RoutedEventArgs e)
        {
            // Check if sender is a framework element
            if (!(sender is FrameworkElement element))
                return;

            // Unhook from the Loaded RoutedEventHandler
            element.Loaded -= CopyInputBindingsToWindow;

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
            }
        }

        /// <summary>
        /// Removes the card input bindings from the window
        /// </summary>
        /// <param name="element"></param>
        private void RemoveCardBindingsFromWindow(FrameworkElement element)
        {
            // Gets the main window
            var window = Application.Current.MainWindow;

            // For each window input binding
            for (int i = window.InputBindings.Count - 1; i >= 0; i--)
            {
                // Remove it from the window if it is contained in the element
                if (element.InputBindings.Contains(window.InputBindings[i]))
                    window.InputBindings.RemoveAt(i);
            }
        }
    }
}