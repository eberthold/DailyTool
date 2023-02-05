namespace Scrummy.Core.ViewModels.Navigation
{
    public interface INavigationTarget
    {
        /// <summary>
        /// Triggered when navigation to view model has happened.
        /// </summary>
        Task OnNavigatedToAsync(NavigationMode navigationMode);

        /// <summary>
        /// Triggered when navigation away from view model is about to happen.
        /// </summary>
        /// <returns><see langword="false"/> if navigation should be canceled.</returns>
        Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode);
    }
}
