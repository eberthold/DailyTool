using DailyTool.ViewModels.Navigation;
using System;

namespace DailyTool.Packaged.Entry.Navigation
{
    public interface INavigationMap
    {
        Type GetForTarget<T>()
            where T : INavigationTarget;
    }
}
