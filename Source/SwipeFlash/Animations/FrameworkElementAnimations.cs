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
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task HorizontalFlipAsync(this FrameworkElement element, float duration = 0.5f)
        {
            // Create the element's transform
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddHorizontalFlip(duration);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Slide and tilt to left animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="width">The length of the translation in pixels</param>
        /// <param name="angle">The tilt angle in degrees</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task SlideAndTiltToLeftAsync(this FrameworkElement element, int width, double angle, float duration = 0.5f)
        {
            // Create the element's transform
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideToLeft(duration, width);

            // Add the tilt animation
            sb.AddTilt(duration, -angle);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Slide and tilt to right animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="width">The width of the slide animation in pixels</param>
        /// <param name="angle">The tilt angle in degrees</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task SlideAndTiltToRightAsync(this FrameworkElement element, int width, double angle, float duration = 0.5f)
        {
            // Create the element's transform
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideToRight(duration, width);

            // Add the tilt animation
            sb.AddTilt(duration, angle);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Slide and tilt to left animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="width">The length of the translation in pixels</param>
        /// <param name="angle">The tilt angle in degrees</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task SlideAndTiltFromLeftAsync(this FrameworkElement element, int width, double angle, float duration = 0.5f)
        {
            // Create the element's transform
            element.SetElementTransform(new RotateTransform(-angle));

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideFromLeft(duration, width);

            // Add the tilt animation
            sb.AddTilt(duration, 0);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Slide and tilt to right animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="width">The width of the slide animation in pixels</param>
        /// <param name="angle">The tilt angle in degrees</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task SlideAndTiltFromRightAsync(this FrameworkElement element, int width, double angle, float duration = 0.5f)
        {
            // Create the element's transform
            element.SetElementTransform(new RotateTransform(angle));

            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideFromRight(duration, width);

            // Add the tilt animation
            sb.AddTilt(duration, 0);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Slide from top animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="width">The width of the slide animation in pixels</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task SlideFromTopAsync(this FrameworkElement element, int width, float duration = 0.5f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideFromTop(duration, width);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Slide to top animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="width">The width of the slide animation in pixels</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task SlideToTopAsync(this FrameworkElement element, int width, float duration = 0.5f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add the horizontal flip animation
            sb.AddSlideToTop(duration, width);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Fade in animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task FadeInAsync(this FrameworkElement element, float duration = 0.5f, bool useBackgroundOpacity=false)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add the fade in animation
            sb.AddFadeIn(duration, useBackgroundOpacity:useBackgroundOpacity);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Fade out animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task FadeOutAsync(this FrameworkElement element, float duration = 0.5f, bool useBackgroundOpacity=false)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add the fade out animation
            sb.AddFadeOut(duration, useBackgroundOpacity:useBackgroundOpacity);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Scale up and fade in animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task ScaleAndFadeInAsync(this FrameworkElement element, double minScale, float duration = 0.5f)
        {
            element.SetElementTransform();
            
            // Create the storyboard
            var sb = new Storyboard();

            // Add the fade in animation
            sb.AddFadeIn(duration);

            // Add the scale up animation
            sb.AddScaleUp(duration, minScale);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));

        }

        /// <summary>
        /// Scale down and fade out animation
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task ScaleAndFadeOutAsync(this FrameworkElement element, double minScale, float duration = 0.5f)
        {
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the fade out animation
            sb.AddFadeOut(duration);

            // Add the scale down animation
            sb.AddScaleDown(duration, minScale);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));

        }

        /// <summary>
        /// Moves a queued element to a new queue position, 
        /// translating and fading it in
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="newCardQueuePosition">The new position of the element in the queue</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task MoveInQueueAsync(this FrameworkElement element, int newCardQueuePosition, bool reverseAnimation, float duration = 0.1f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add the card queue translation animation
            sb.AddCardQueueTranslation(duration, newCardQueuePosition, reverseAnimation);

            // Add the card queue fade in animation
            sb.AddCardQueueFadeIn(duration, newCardQueuePosition, reverseAnimation);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));
        }
        
        /// <summary>
        /// Scale up the gradient radius on the X axis
        /// </summary>
        /// <param name="element">The framework element to animate</param>
        /// <param name="duration">The duration of the animation in seconds</param>
        /// <returns></returns>
        public static async Task BurstGradientBrushXRadiusAsync(this FrameworkElement element, double minScale, float duration = 0.5f)
        {
            element.SetElementTransform();

            // Create the storyboard
            var sb = new Storyboard();

            // Add the scale up animation
            sb.AddBurstGradientBrushX(duration, minScale);

            // Begin the animation
            sb.Begin(element);

            // Wait for it to finish
            await Task.Delay((int)(duration * 1000));

        }

        #region Private Helpers

        private static void SetElementTransform(this FrameworkElement element, 
                                                     RotateTransform rotateTransform = null,
                                                     ScaleTransform scaleTransform = null)
        {
            // Check whether the element has a transform group
            if (!(element.RenderTransform is TransformGroup))
            {
                // Create new transform group
                var newTransform = new TransformGroup();

                // Set the new transform's children

                // Creates a rotate transform if none has been specified
                if (rotateTransform == null)
                    scaleTransform = new ScaleTransform();
                // Creates a scale transform if none has been specified
                if (rotateTransform == null)
                    rotateTransform = new RotateTransform();

                newTransform.Children = new TransformCollection() { scaleTransform, rotateTransform };

                // Set the element's transform to the new transform
                element.RenderTransform = newTransform;

                // Set the element's transform origin to its center
                element.RenderTransformOrigin = new Point(.5, .5);

                return;
            }

            if (rotateTransform != null)
            {
                // Gets the transform group
                var transform = element.RenderTransform as TransformGroup;

                // Sets the rotate transform
                transform.Children[1] = rotateTransform;
            }

            if (scaleTransform != null)
            {
                // Gets the transform group
                var transform = element.RenderTransform as TransformGroup;

                // Sets the rotate transform
                transform.Children[1] = scaleTransform;
            }
        }

        #endregion

    }
}
