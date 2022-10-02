using DailyTool.UserInterface.Daily;
using DailyTool.UserInterface.Initialization;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Navigation;
using System;

namespace DailyTool.Packaged.Entry.Navigation
{
    internal class NavigationMap : INavigationMap
    {
        /// <inheritdoc />
        public Type GetForTarget<T>()
            where T : INavigationTarget
            => typeof(T).Name switch
            {
                nameof(InitializationViewModel) => typeof(InitializationView),
                nameof(DailyViewModel) => typeof(DailyView),
                _ => throw new NotSupportedException($"Unmapped navigation target {typeof(T).Name}")
            };
    }
}
