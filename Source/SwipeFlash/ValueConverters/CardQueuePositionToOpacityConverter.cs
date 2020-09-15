using System;
using System.Globalization;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in a bool and returns a <see cref="Visibility"/>
    /// </summary>
    public class CardQueuePositionToOpacityConverter : BaseValueConverter<CardQueuePositionToOpacityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((int)value)
            {
                case 0:
                    return 1;
                case 1:
                    return 1;
                case 2:
                    return 0.5;
                default:
                    return 0;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
