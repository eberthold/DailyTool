using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyTool.UserInterface.Settings
{
    public class SettingsTemplateSelector : DataTemplateSelector
    {
        public List<MappingEntry> Map { get; set; } = new List<MappingEntry>();

        protected override DataTemplate? SelectTemplateCore(object? item)
        {
            if (item is null)
            {
                return null;
            }

            var dataTemplate = Map
                .SingleOrDefault(x => x.DataType == item.GetType())
                ?.Template;

            return dataTemplate;
        }

        /// <inheritdoc />
        protected override DataTemplate? SelectTemplateCore(object? item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
