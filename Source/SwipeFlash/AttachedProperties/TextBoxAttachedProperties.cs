using System.Windows;
using System.Windows.Controls;

namespace SwipeFlash
{
    /// <summary>
    /// If set to true, the entire text of the textbox is selected when it gets focused
    /// </summary>
    class SelectAllTextOnFocusProperty : BaseAttachedProperty<SelectAllTextOnFocusProperty, bool>
    {
        /// <summary>
        /// Whether the TextBox is currently focused
        /// </summary>
        public bool IsMouseFocused { get; set; } = false;

        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            // Type check the sender
            if (!(sender is TextBox textBox))
                return;

            // If the property is enabled
            if ((bool)e.NewValue)
            {
                // Add the methods to the event
                textBox.GotFocus += OnGotFocus;
                textBox.LostMouseCapture += OnLostMouseCapture;
                textBox.LostFocus += OnLostFocus;
            }
            else
            {
                // Remove the methods from the event
                textBox.GotFocus -= OnGotFocus;
                textBox.LostMouseCapture -= OnLostMouseCapture;
                textBox.LostFocus -= OnLostFocus;
            }
        }

        /// <summary>
        /// Called when the <see cref="TextBox"/> loses mouse or keyboard focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            // Resets the is mouse focused flag
            IsMouseFocused = false;
        }

        /// <summary>
        /// Called when the mouse click has been released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLostMouseCapture(object sender, RoutedEventArgs e)
        {
            // If this is the first click on the textbox
            if (!IsMouseFocused)
                // Selects the entire text
                SelectAllText(sender as TextBox);
            
            // Sets the is mouse focused flag
            IsMouseFocused = true;
        }

        /// <summary>
        /// If the <see cref="TextBox"/> gets keyboard or mouse focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            // Selects the entire text
            SelectAllText(sender as TextBox);
        }

        /// <summary>
        /// Selects the entire <see cref="TextBox"/> text
        /// </summary>
        /// <param name="target">The target element</param>
        private void SelectAllText(TextBox target)
        {
            // Selects all the text of the textbox
            target.SelectAll();
        }
    }
}
