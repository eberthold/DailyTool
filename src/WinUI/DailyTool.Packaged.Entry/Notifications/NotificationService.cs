using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.ViewModels.Notifications;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DailyTool.Packaged.Entry.Notifications
{
    public class NotificationService : ObservableObject, INotificationService
    {
        public ObservableCollection<Notification> Notifications { get; } = new ObservableCollection<Notification>();

        public Task ShowNotificationAsync(Notification notification)
        {
            if (Notifications.Contains(notification))
            {
                return Task.CompletedTask;
            }

            notification.PropertyChanged += OnNotificationChanged;
            Notifications.Add(notification);

            if (notification.IsRunning)
            {
                return Task.CompletedTask;
            }

            _ = TriggerKillNotification(notification);
            return Task.CompletedTask;
        }

        private void OnNotificationChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var notification = sender as Notification;
            if (notification is null)
            {
                return;
            }

            if (e.PropertyName != nameof(Notification.IsRunning))
            {
                return;
            }

            if (notification.IsRunning)
            {
                return;
            }

            _ = TriggerKillNotification(notification);
        }

        private async Task TriggerKillNotification(Notification notification)
        {
            var delay = TimeSpan.FromSeconds(1.5);
            await Task.Delay(delay);

            Notifications.Remove(notification);
        }
    }
}
