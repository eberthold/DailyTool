using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.ViewModels.Notifications;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.Core.ViewModels.Teams;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DailyTool.Packaged.Entry
{
    public class MainWindowViewModel : ObservableObject, ILoadDataAsync
    {
        public MainWindowViewModel(
            INotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        public INotificationService NotificationService { get; }

        public ObservableCollection<TeamViewModel> Teams { get; set; } = new ObservableCollection<TeamViewModel>();

        public Task LoadDataAsync()
        {
            return Task.CompletedTask;
        }
    }
}
