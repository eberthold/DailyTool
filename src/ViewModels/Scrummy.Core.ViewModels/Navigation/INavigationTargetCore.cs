namespace Scrummy.Core.ViewModels.Navigation
{
    public interface INavigationTargetCore
    {
        /// <summary>
        /// Triggered when naviation away from view model is about to happen.
        /// </summary>
        /// <returns><see langword="false"/> if navigation should be cancelled.</returns>
        Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode);
    }
}
