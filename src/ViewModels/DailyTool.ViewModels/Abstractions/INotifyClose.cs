namespace DailyTool.ViewModels.Abstractions
{
    internal interface INotifyClose
    {
        event EventHandler? Closed;
    }
}
