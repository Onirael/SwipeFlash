using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace SwipeFlash
{
    /// <summary>
    /// Animation helpers for <see cref="Storyboard"/>
    /// </summary>
    static class StoryboardHelpers
    {

        /// <summary>
        /// Adds a flip animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="duration">The duration of the animation</param>
        /// <param name="decelerationratio">The rate of deceleration</param>
        public static void AddHorizontalFlip(this Storyboard storyboard, float duration, float decelerationRatio = 1f)
        {
            // Scale X animation
            var scaleXAnimation = new DoubleAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                DecelerationRatio = decelerationRatio,
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new SplineDoubleKeyFrame(){ KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration * 0)), Value=1 },
                    new SplineDoubleKeyFrame(){ KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration * 0.5)), Value=0 },
                    new SplineDoubleKeyFrame(){ KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration * 1)), Value=1 },
                },
            };

            // Set the target property names
            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("RenderTransform.(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

            // Add animations
            storyboard.Children.Add(scaleXAnimation);
        }

        /// <summary>
        /// Adds a slide animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="duration">The duration of the animation</param>
        /// <param name="offest">The distance to the left to start from</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideToLeft(this Storyboard storyboard, float duration, double offset, float decelerationRatio = 0.9f)
        {
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, offset, 0),
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="duration">The duration of the animation</param>
        /// <param name="offest">The distance to the left to start from</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddSlideToRight(this Storyboard storyboard, float duration, double offset, float decelerationRatio = 0.9f)
        {
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = new Thickness(0),
                To = new Thickness(offset, 0, -offset, 0),
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="duration">The duration of the animation</param>
        /// <param name="offest">The angle to reach</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddTiltToLeft(this Storyboard storyboard, float duration, double angleOffset, float decelerationRatio = 0.9f)
        {
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = 0,
                To = -angleOffset,
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide animation to the storyboard
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="duration">The duration of the animation</param>
        /// <param name="offest">The angle to reach</param>
        /// <param name="decelerationRatio">The rate of deceleration</param>
        public static void AddTiltToRight(this Storyboard storyboard, float duration, double angleOffset, float decelerationRatio = 0.9f)
        {
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = 0,
                To = angleOffset,
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.(TransformGroup.Children)[1].(RotateTransform.Angle)"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }
    }
}
