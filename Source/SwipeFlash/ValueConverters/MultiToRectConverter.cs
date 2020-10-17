using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in two doubles and returns a <see cref="Rect"/>, 
    /// the doubles should be given in order (width, height)
    /// </summary>
    public class MultiToRectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // If there aren't two values in the array, quit
            if (values.Count() != 2)
                return null;

            double[] doubleValues;
            // Tries to cast the values to double
            try { doubleValues = values.Cast<double>().ToArray(); }
            catch { return null; }

            // Creates the new Rect
            var newRect = new Rect(0, 0, doubleValues[0], doubleValues[1]);

            return newRect;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
