using SwipeFlash.Core;
using System;
using System.Globalization;

namespace SwipeFlash
{
    /// <summary>
    /// A converter that takes in an array and returns the element corresponding to the parameter
    /// </summary>
    public class ViewModelArrayToViewModelConverter : BaseValueConverter<ViewModelArrayToViewModelConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var elementIndex = System.Convert.ToInt32(parameter);

            if (value is BaseViewModel[] VMArray)
                return VMArray[elementIndex];
            else
                return null;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
