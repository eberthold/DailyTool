namespace DailyTool.ViewModels.Abstractions
{
    internal interface INotifyClose
    {
        void AddCloseCallback(Func<Task> callback);
    }
}
