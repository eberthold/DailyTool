using Scrummy.Core.ViewModels.Navigation;
using System;

namespace DailyTool.UserInterface.Navigation
{
    public interface INavigationMap
    {
        Type GetForTarget<T>()
            where T : INavigationTarget;

        Type GetForTarget(Type type);
    }
}
