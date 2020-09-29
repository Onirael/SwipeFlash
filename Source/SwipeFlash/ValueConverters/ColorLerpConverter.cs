using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in two colors and a scalar and returns a color lerp
    /// </summary>
    public class ColorLerpConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if there are two values
            if (values.Length != 3)
                return null;

            // Initialize expected input members
            double blendScalar = 0;
            List<Color> colors = new List<Color>();


            // Count each type
            int foundColors = 0;
            int foundScalars = 0;
            foreach (var value in values)
            {
                if (value is Color color)
                {
                    colors.Add(color);
                    foundColors++;

                }
                else if (value is double dValue)
                {
                    blendScalar = dValue;
                    foundScalars++;
                }
            }
            
            // Check if the type counts match
            if (foundColors != 2 || foundScalars != 1)
                return null;

            // Lerp the colors
            var newColor = colors[0].Lerp(colors[1], blendScalar);

            // Return new color
            return newColor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
