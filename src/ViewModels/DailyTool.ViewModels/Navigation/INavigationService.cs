namespace DailyTool.ViewModels.Navigation
{
    public interface INavigationService
    {
        Task<T?> NavigateAsync<T>()
            where T : class, INavigationTarget;

        Task GoBackAsync();

        Task<T> CreateNavigationTarget<T>()
            where T : INavigationTarget;
    }
}
