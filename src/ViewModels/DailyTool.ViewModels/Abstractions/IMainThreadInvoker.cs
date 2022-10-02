namespace DailyTool.ViewModels.Abstractions
{
    public interface IMainThreadInvoker
    {
        void Invoke(Action action);

        Task InvokeAsync(Func<Task> task);

        Task InvokeAsync(Action action);
    }
}
