using Microsoft.UI.Xaml;
using System;

namespace DailyTool.UserInterface.Settings
{
    public class MappingEntry
    {
        public Type? DataType { get; set; }

        public DataTemplate Template { get; set; } = new DataTemplate();
    }
}
