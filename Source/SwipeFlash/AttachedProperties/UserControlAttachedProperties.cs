using System.Windows;

namespace SwipeFlash
{
    public class IsPendingDestroyProperty : BaseAttachedProperty<IsPendingDestroyProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && sender is FrameworkElement element)
            {
                element.RemoveChild();
            }
        }
    }
}