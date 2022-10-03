using Microsoft.UI.Xaml.Data;
using System;

namespace DailyTool.UserInterface.Converters
{
    public class PercentageToOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var percentage = value as double?;
            if (percentage is null)
            {
                return 0d;
            }

            return percentage.Value / 100d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
