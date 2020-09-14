using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SwipeFlash
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T">The type of this value converter</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
            where T : class, new()
    {
        #region Private Members

        /// <summary>
        /// A single static instance of this value converter
        /// </summary>
        private static T mConverter = null;

        #endregion

        #region Markup Extension Methods

        /// <summary>
        /// Provides a static instance of the value converter
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter ?? (mConverter = new T()); // Equivalent of "if (mConverter == null) return new T(); else return mConverter
        }

        #endregion

        #region Value Converter Methods

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
