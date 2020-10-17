using System;
using System.Globalization;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in a <see cref="CornerRadius"/> and returns a double, 
    /// the double value is taken from the value of the top left corner radius
    /// </summary>
    public class CornerRadiusToDoubleConverter : BaseValueConverter<CornerRadiusToDoubleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check the value of the input
            if (!(value is CornerRadius radius))
                return null;

            return radius.TopLeft;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
