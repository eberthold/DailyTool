using DailyTool.ViewModels.Dashboard;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DailyTool.UserInterface
{
    public class TitleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TextTitleTemplate { get; set; }

        public DataTemplate? DashboardTitleTemplate { get; set; }

        /// <inheritdoc />
        protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is DashboardViewModel)
            {
                return DashboardTitleTemplate;
            }

            return TextTitleTemplate;
        }
    }
}
