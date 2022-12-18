namespace DailyTool.ViewModels.Navigation
{
    public interface INavigationTarget
    {
        /// <summary>
        /// Triggered when navigation to view model has happened.
        /// </summary>
        Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode);

        /// <summary>
        /// Triggered when naviation away from view model is about to happen.
        /// </summary>
        /// <returns><see langword="false"/> if navigation should be cancelled.</returns>
        Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode);
    }
}
