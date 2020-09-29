using System.Windows.Media;

namespace SwipeFlash
{
    /// <summary>
    /// Helper methods for WPF colors
    /// </summary>
    public static class ColorHelpers
    {
        /// <summary>
        /// Lerps the color with another color by a blend scalar
        /// </summary>
        /// <param name="background">The background color</param>
        /// <param name="foreground">The foreground color</param>
        /// <param name="scalar">The blend amount</param>
        /// <returns></returns>
        public static Color Lerp(this Color background, Color foreground, double scalar)
        {
            // Creates a new color
            var newColor = new Color()
            {
                // Lerps components
                A = (byte)(background.A * (1 - scalar) + foreground.A * scalar),
                R = (byte)(background.R * (1 - scalar) + foreground.R * scalar),
                G = (byte)(background.G * (1 - scalar) + foreground.G * scalar),
                B = (byte)(background.B * (1 - scalar) + foreground.B * scalar),
            };

            // Returns the new color
            return newColor;
        }
    }
}
