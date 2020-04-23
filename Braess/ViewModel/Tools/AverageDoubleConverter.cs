namespace Braess.ViewModel.Tools
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class AverageDoubleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double total = 0;
            foreach (double value in values)
            {
                total += value;
            }

            return total / values.Length;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
