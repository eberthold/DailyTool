namespace Scrummy.Core.ViewModels.Navigation
{
    public interface INavigationService
    {
        bool CanGoBack { get; }

        Task<T?> NavigateAsync<T>()
            where T : class, INavigationTarget;

        Task<T?> NavigateAsync<T, TParam>(TParam parameter)
            where T : class, INavigationTarget<TParam>;

        Task NavigateBackAsync();

        Task<T> CreateNavigationTarget<T>()
            where T : INavigationTarget;

        Task<T> CreateNavigationTarget<T, TParam>(TParam parameter)
            where T : INavigationTarget<TParam>;
    }
}
