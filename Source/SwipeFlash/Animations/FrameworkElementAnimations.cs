using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SwipeFlash
{
    /// <summary>
    /// Helpers to animate framework elements in specific ways
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        /// Horizontal flip animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="duration">The duration of the animation</param>
        /// <returns></returns>
        public static async Task HorizontalFlipAsync(this FrameworkElement element, float duration = 0.5f)
        {
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddHorizontalFlip(duration);

            // Begin the animation
            sb.Begin(element);
            
            // Wait for it to finish
            await Task.Delay((int)duration * 1000);
        }

        public static async Task SlideAndTilttoLeftAsync(this FrameworkElement element, int width, double angle, float duration = 0.5f)
        {
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideToLeft(duration, width);

            // Add the tilt animation
            sb.AddTiltToLeft(duration, angle);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)duration * 1000);
        }

        public static async Task SlideAndTiltToRightAsync(this FrameworkElement element, int width, double angle, float duration = 0.5f)
        {
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideToRight(duration, width);

            // Add the tilt animation
            sb.AddTiltToRight(duration, angle);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)duration * 1000);
        }

        #region Private Helpers

        private static void SetElementTransform(this FrameworkElement element)
        {
            // Check whether the element has a transform group
            if (!(element.RenderTransform is TransformGroup))
            {
                // Create new transform group
                var newTransform = new TransformGroup();

                // Set the new transform's children
                newTransform.Children = new TransformCollection() { new ScaleTransform(), new RotateTransform() };

                // Set the element's transform to the new transform
                element.RenderTransform = newTransform;

                // Set the element's transform origin to its center
                element.RenderTransformOrigin = new Point(.5, .5);
            }
        }

        #endregion

    }
}
