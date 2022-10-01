using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace DailyTool.UserInterface.Converters
{
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isHidden = value as bool?;
            if (isHidden is null)
            {
                return DependencyProperty.UnsetValue;
            }

            return isHidden.Value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
