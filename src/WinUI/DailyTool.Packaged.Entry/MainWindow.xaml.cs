using DailyTool.Packaged.Entry.Navigation;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace DailyTool.Packaged.Entry
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window, INavigationService
    {
        private readonly INavigationMap _navigationMap;
        private readonly IServiceProvider _serviceProvider;
        private object? _dataContext;

        public MainWindow(
            INavigationMap navigationMap,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _navigationMap = navigationMap;
            _serviceProvider = serviceProvider;
        }

        public object? DataContext
        {
            get => _dataContext;
            set
            {
                _dataContext = value;
                PART_Content.DataContext = value;
            }
        }

        public async Task GoBackAsync()
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

        public async Task<T?> NavigateAsync<T>()
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

            PART_NavigationFrame.Navigate(_navigationMap.GetForTarget<T>());

            content = PART_NavigationFrame.Content as Control;
            if (content is null)
            {
                throw new InvalidOperationException("Navigation failed");
            }

            var newViewModel = _serviceProvider.GetRequiredService<T>();
            content.DataContext = newViewModel;
            await newViewModel.OnNavigatedToAsync(NavigationMode.Forward);

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