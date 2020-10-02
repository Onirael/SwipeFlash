using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace SwipeFlash
{
    /// <summary>
    /// A converter taking in two <see cref="Color"/> values
    /// and a scalar to blend (lerp) them
    /// Values must be passed in in the right order:
    /// 0 : Background
    /// 1 : Foreground
    /// 2 : Scalar
    /// </summary>
    public class ColorLerpConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if there are three values
            if (values.Length != 3)
                return null;

            // If SolidColorBrush values are passed in, convert them to colors
            if (values[0] is SolidColorBrush brush1) values[0] = brush1.Color;
            if (values[1] is SolidColorBrush brush2) values[1] = brush2.Color;
            

            if (!(values[0] is Color background && 
                  values[1] is Color foreground &&
                  values[2] is double scalar))
                return null;

            // Lerp the colors
            var newColor = background.Lerp(foreground, scalar);

            // Return new color
            return newColor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
