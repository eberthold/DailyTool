using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace DailyTool.UserInterface.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isVisible = value as bool?;
            if (isVisible is null)
            {
                return DependencyProperty.UnsetValue;
            }

            return isVisible.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
