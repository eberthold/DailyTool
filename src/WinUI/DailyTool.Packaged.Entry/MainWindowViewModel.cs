using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.ViewModels.Notifications;

namespace DailyTool.Packaged.Entry
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(
            INotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        public INotificationService NotificationService { get; }
    }
}
