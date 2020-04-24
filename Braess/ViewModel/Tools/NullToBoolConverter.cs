namespace Braess.ViewModel.Tools
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
