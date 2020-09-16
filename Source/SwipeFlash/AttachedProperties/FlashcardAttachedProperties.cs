using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// Animates the transition to a new card queue position
    /// </summary>
    public class AnimateQueuePositionProperty : BaseAttachedProperty<AnimateQueuePositionProperty, int>
    {
        #region Public Properties

        public bool IsFirstLoad { get; set; } = true;

        #endregion

        #region On Value Changed

        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Check if sender is a FrameworkElement
            if (!(sender is FrameworkElement element))
                return;

            // Run the transition animation
            AnimateTransition(element, (int)e.NewValue, IsFirstLoad ? 0f : .2f);

            IsFirstLoad = false;
        }

        #endregion

        #region Private Helpers

        private async void AnimateTransition(FrameworkElement element, int newQueuePosition, float duration)
        {
            await element.MoveInQueueAsync(newQueuePosition, duration);
        }

        #endregion
    }
}