using DailyTool.UserInterface.Daily;
using DailyTool.UserInterface.Initialization;
using DailyTool.UserInterface.Settings;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Navigation;
using DailyTool.ViewModels.Settings;
using System;

namespace DailyTool.UserInterface.Navigation
{
    public class NavigationMap : INavigationMap
    {
        /// <inheritdoc />
        public Type GetForTarget<T>()
            where T : INavigationTarget
            => typeof(T).Name switch
            {
                nameof(InitializationViewModel) => typeof(InitializationView),
                nameof(DailyViewModel) => typeof(DailyView),
                nameof(SettingsOverviewViewModel) => typeof(SettingsOverviewPage),
                _ => throw new NotSupportedException($"Unmapped navigation target {typeof(T).Name}")
            };
    }
}
