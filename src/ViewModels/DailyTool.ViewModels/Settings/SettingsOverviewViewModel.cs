﻿using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.ViewModels.Abstractions;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.Immutable;

namespace DailyTool.ViewModels.Settings
{
    public class SettingsOverviewViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, ITitle
    {
        private readonly ISettingsProvider _settingsProvider;

        private IReadOnlyCollection<ISettingsViewModel> _settings = ImmutableArray<ISettingsViewModel>.Empty;
        private ISettingsViewModel? _selectedItem;

        public SettingsOverviewViewModel(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public IReadOnlyCollection<ISettingsViewModel> Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        public ISettingsViewModel? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public object Title => "TODO: Einstellungen";

        public async Task LoadDataAsync()
        {
            Settings = await _settingsProvider.GetSettingsAsync();
        }

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
        {
            return LoadDataAsync();
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
            => Task.FromResult(true);
    }
}
