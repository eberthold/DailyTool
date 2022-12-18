using DailyTool.UserInterface.Navigation;
using DailyTool.ViewModels.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Scrummy.ViewModels.Shared.Data;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace DailyTool.UserInterface
{
    public sealed partial class Shell : UserControl, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private INavigationMap _navigationMap;

        public Shell(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _navigationMap = _serviceProvider.GetRequiredService<INavigationMap>();
        }

        public void Initialize()
        {
            DataContext = _serviceProvider.GetRequiredService<ShellViewModel>();
        }

        public async Task NavigateBackAsync()
        {
            if (!PART_NavigationFrame.CanGoBack)
            {
                return;
            }

            var content = PART_NavigationFrame.Content as Control;
            var vm = content?.DataContext as INavigationTarget;
            var canLeave = await (vm?.OnNavigatingFromAsync(NavigationMode.Backward)
                ?? Task.FromResult(true));
            if (!canLeave)
            {
                return;
            }

            (vm as IDisposable)?.Dispose();

            PART_NavigationFrame.GoBack();
        }

        public Task<T?> NavigateAsync<T>()
            where T : class, INavigationTarget
        {
            return NavigateAsync<T>(ImmutableDictionary<string, string>.Empty);
        }

        public async Task<T?> NavigateAsync<T>(IReadOnlyDictionary<string, string> parameters)
            where T : class, INavigationTarget
        {
            var content = PART_NavigationFrame.Content as Control;
            var oldViewModel = content?.DataContext as INavigationTarget;
            var canLeave = await (oldViewModel?.OnNavigatingFromAsync(NavigationMode.Forward)
                ?? Task.FromResult(true));
            if (!canLeave)
            {
                return null;
            }

            var contentType = _navigationMap.GetForTarget<T>();
            PART_NavigationFrame.Navigate(contentType);

            content = PART_NavigationFrame.Content as Control;
            if (content is null)
            {
                throw new InvalidOperationException("Navigation failed");
            }

            var newViewModel = _serviceProvider.GetRequiredService<T>();
            content.DataContext = newViewModel;
            await newViewModel.OnNavigatedToAsync(parameters, NavigationMode.Forward);

            if (newViewModel is ILoadDataAsync loadDataAsync)
            {
                await loadDataAsync.LoadDataAsync();
            }

            return newViewModel;
        }

        public Task<T> CreateNavigationTarget<T>()
            where T : INavigationTarget
        {
            return Task.FromResult(_serviceProvider.GetRequiredService<T>());
        }
    }
}
