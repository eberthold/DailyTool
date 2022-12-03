using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.ViewModels.Notifications
{
    public class Notification : ObservableObject, INotification
    {
        private string _text = string.Empty;
        private NotificationType _notificationType;
        private bool _isRunning;

        public NotificationType NotificationType
        {
            get => _notificationType;
            set => SetProperty(ref _notificationType, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}