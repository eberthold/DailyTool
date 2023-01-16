﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Notifications;
using DailyTool.ViewModels.Settings;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.Core.ViewModels.Teams;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DailyTool.UserInterface
{
    public class ShellViewModel : ObservableObject, ILoadDataAsync
    {
        private readonly INavigationService _navigationService;

        public ShellViewModel(
            INotificationService notificationService,
            INavigationService navigationService)
        {
            NotificationService = notificationService;
            _navigationService = navigationService;

            NavigateBackCommand = new AsyncRelayCommand(NavigateBackAsync);
            NavigateSettingsCommand = new AsyncRelayCommand(NavigateToSettingsAsync);
        }

        public IRelayCommand NavigateBackCommand { get; }

        public IRelayCommand NavigateSettingsCommand { get; }

        public INotificationService NotificationService { get; }

        public ObservableCollection<TeamViewModel> Teams { get; set; } = new ObservableCollection<TeamViewModel>();

        public Task LoadDataAsync()
        {
            return Task.CompletedTask;
        }

        private Task NavigateToSettingsAsync()
        {
            return _navigationService.NavigateAsync<SettingsOverviewViewModel>();
        }

        private Task NavigateBackAsync()
        {
            return _navigationService.NavigateBackAsync();
        }
    }
}