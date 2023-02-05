namespace Scrummy.Core.ViewModels.Navigation
{
    public interface INavigationTarget<T> : INavigationTarget
    {
        void SetParameters(T parameter);
    }
}
