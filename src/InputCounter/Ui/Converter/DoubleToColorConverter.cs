using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace InputCounter.Ui.Converter;

/// <summary>
/// Provides a double to percentage converter
/// </summary>
internal class DoubleToColorConverter : IValueConverter
{
    /// <summary>
    /// Converts a double to a color
    /// </summary>
    /// <param name="value">The value which should be converted</param>
    /// <param name="targetType">The target type</param>
    /// <param name="parameter">The parameter</param>
    /// <param name="culture">The culture</param>
    /// <returns>The color</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double tmpValue)
            return new SolidColorBrush(Colors.White);

        return tmpValue switch
        {
            > 0 => new SolidColorBrush(Colors.Green),
            < 0 => new SolidColorBrush(Colors.Red),
            _ => new SolidColorBrush(Colors.White),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}