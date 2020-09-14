using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SwipeFlash
{
    /// <summary>
    /// Focuses (keyboard focus) this element on load
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is Control control))
                return;

            // Focus this control once loaded
            control.Loaded += (ss, ee) => control.Focus();
        }
    }
}
