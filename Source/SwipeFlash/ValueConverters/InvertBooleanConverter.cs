using System;
using System.Globalization;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in a bool and returns a <see cref="Visibility"/>
    /// </summary>
    public class InvertBooleanConverter : BaseValueConverter<InvertBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
