namespace DailyTool.ViewModels.Notifications
{
    public interface INotificationService
    {
        Task ShowNotificationAsync(Notification notification);
    }
}
