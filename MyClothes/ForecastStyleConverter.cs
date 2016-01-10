using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WeatherView
{
    //
    // A ValueConverter to change a forecast temperature difference into a color
    // for the background text in the two forecast controls. If the temperature in the
    // forecast is cooler than the current temperature, the color is blue. If the
    // temperature is warmer, the color is red. If the temperature is the same,
    // the color is green.
    //
    class ForecastStyleConverter : IValueConverter
    {

        // Convertor to change color of text background depending on temperature
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           
            if ((int)value>0) return new SolidColorBrush(Colors.Red);
            if ((int)value<0) return new SolidColorBrush(Colors.Blue);
            if ((int)value == 0) return new SolidColorBrush(Colors.LightGreen);

            return new SolidColorBrush(Colors.Black);

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
