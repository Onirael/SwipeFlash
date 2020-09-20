using System;
using System.Globalization;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in a bool and returns a <see cref="Visibility"/>
    /// </summary>
    public class BooleanToCollapsedVisibilityConverter : BaseValueConverter<BooleanToCollapsedVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            else
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
