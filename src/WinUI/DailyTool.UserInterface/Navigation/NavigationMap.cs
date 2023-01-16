using DailyTool.UserInterface.Daily;
using DailyTool.UserInterface.Dashboard;
using DailyTool.UserInterface.Initialization;
using DailyTool.UserInterface.Settings;
using DailyTool.UserInterface.Teams;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Dashboard;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Settings;
using DailyTool.ViewModels.Teams;
using Scrummy.Core.ViewModels.Navigation;
using System;

namespace DailyTool.UserInterface.Navigation
{
    public class NavigationMap : INavigationMap
    {
        /// <inheritdoc />
        public Type GetForTarget<T>()
            where T : INavigationTarget
        {
            return GetForTarget(typeof(T));
        }

        public Type GetForTarget(Type type)
        {
            return type.Name switch
            {
                nameof(InitializationViewModel) => typeof(InitializationView),
                nameof(DailyViewModel) => typeof(DailyView),
                nameof(SettingsOverviewViewModel) => typeof(SettingsOverviewPage),
                nameof(EditTeamViewModel) => typeof(EditTeamView),
                nameof(DashboardViewModel) => typeof(DashboardView),
                _ => throw new NotSupportedException($"Unmapped navigation target {type.Name}")
            };
        }
    }
}
