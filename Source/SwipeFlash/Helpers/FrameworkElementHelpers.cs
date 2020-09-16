using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SwipeFlash
{
    /// <summary>
    /// Helpers for a <see cref="UserControl"/>
    /// </summary>
    public static class FrameworkElementHelpers
    {
        public static void RemoveFromParent(this FrameworkElement element)
        {
            var parent = VisualTreeHelper.GetParent(element);

            // If parent is a ContainerVisual
            if (parent is ContainerVisual containerVisual)
            {
                containerVisual.Children.Remove(element);
                return;
            }
            
            // If parent is a Panel
            if (parent is Panel panel)
            {
                panel.Children.Remove(element);
                return;
            }
            
            // If parent is a Decorator
            if (parent is Decorator decorator)
            {
                if (decorator.Child == element)
                    decorator.Child = null;

                return;
            }
            
            // If parent is a ContentPresenter
            if (parent is ContentPresenter contentPresenter)
            {
                if (contentPresenter.Content == element)
                {
                    contentPresenter.Content = null;
                }
                return;
            }

            // If parent is a ContentControl
            if (parent is ContentControl contentControl)
            {
                if (contentControl.Content == element)
                {
                    contentControl.Content = null;
                }
                return;
            }
        }
    }
}
