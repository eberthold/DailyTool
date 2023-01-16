using DailyTool.ViewModels.Teams;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DailyTool.UserInterface.Settings
{
    public class SettingsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TeamsTemplate { get; set; }

        protected override DataTemplate? SelectTemplateCore(object? item)
        {
            return item switch
            {
                TeamsOverviewViewModel _ => TeamsTemplate,
                _ => null
            };
        }

        /// <inheritdoc />
        protected override DataTemplate? SelectTemplateCore(object? item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
