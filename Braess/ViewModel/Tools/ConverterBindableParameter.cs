﻿namespace Braess.ViewModel.Tools
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [ContentProperty(nameof(Binding))]
    public class ConverterBindableParameter : MarkupExtension
    {
        public ConverterBindableParameter()
        {
        }

        public ConverterBindableParameter(string path)
        {
            Binding = new Binding(path);
        }

        public ConverterBindableParameter(Binding binding)
        {
            Binding = binding;
        }

        public Binding Binding { get; set; }

        public BindingMode Mode { get; set; }

        public IValueConverter Converter { get; set; }

        public Binding ConverterParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            Binding.Mode = Mode;
            multiBinding.Bindings.Add(Binding);
            if (ConverterParameter != null)
            {
                ConverterParameter.Mode = BindingMode.OneWay;
                multiBinding.Bindings.Add(ConverterParameter);
            }

            var adapter = new MultiValueConverterAdapter
            {
                Converter = Converter,
            };
            multiBinding.Converter = adapter;
            return multiBinding.ProvideValue(serviceProvider);
        }

        [ContentProperty(nameof(Converter))]
        private class MultiValueConverterAdapter : IMultiValueConverter
        {
            private object lastParameter;

            public IValueConverter Converter { get; set; }

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (Converter == null)
                {
                    return values[0]; // Required for VS design-time
                }

                if (values.Length > 1)
                {
                    lastParameter = values[1];
                }

                return Converter.Convert(values[0], targetType, lastParameter, culture);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                if (Converter == null)
                {
                    return new object[] { value }; // Required for VS design-time
                }

                return new object[] { Converter.ConvertBack(value, targetTypes[0], lastParameter, culture) };
            }
        }
    }
}
