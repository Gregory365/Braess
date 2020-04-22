namespace Braess.ViewModel.Tools
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class PointCentraliserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && parameter is double)
            {
                return (double)value - ((double)parameter / 2);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && parameter is double)
            {
                return (double)value + ((double)parameter / 2);
            }

            return null;
        }
    }
}
