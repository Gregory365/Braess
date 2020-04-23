namespace Braess.ViewModel.Tools
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class LineCentraliserConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double average = ((double)values[0] + (double)values[1]) / 2;

            return average - double.Parse((string)parameter);

            //return parameter != null && (double)values[0] < (double)values[1] ? average - double.Parse((string)parameter) : average;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
