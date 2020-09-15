using System;
using System.Globalization;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in a bool and returns a <see cref="Visibility"/>
    /// </summary>
    public class CardQueuePositionToMarginConverter : BaseValueConverter<CardQueuePositionToMarginConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int leftMarginOffset = -5;
            int topMarginOffset = -10;

            return new Thickness((int)value * leftMarginOffset, (int)value * topMarginOffset, -(int)value * leftMarginOffset, -(int)value * topMarginOffset);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
