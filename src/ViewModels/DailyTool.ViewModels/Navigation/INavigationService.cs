namespace DailyTool.ViewModels.Navigation
{
    public interface INavigationService
    {
        Task<T?> NavigateAsync<T>()
            where T : class, INavigationTarget;

        Task<T?> NavigateAsync<T>(IReadOnlyDictionary<string, string> parameters)
            where T : class, INavigationTarget;

        Task NavigateBackAsync();

        Task<T> CreateNavigationTarget<T>()
            where T : INavigationTarget;
    }
}
