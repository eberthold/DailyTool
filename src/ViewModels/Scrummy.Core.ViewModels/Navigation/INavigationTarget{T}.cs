namespace Scrummy.Core.ViewModels.Navigation
{
    public interface INavigationTarget<T> : INavigationTargetCore
    {
        /// <summary>
        /// Triggered when navigation to view model has happened.
        /// </summary>
        Task OnNavigatedToAsync(T parameters, NavigationMode navigationMode);
    }
}
