using DailyTool.UserInterface.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Scrummy.Core.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace DailyTool.UserInterface
{
    public sealed partial class Shell : UserControl, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationMap _navigationMap;
        private readonly Stack<BackStackEntry> _backStack = new Stack<BackStackEntry>();

        public static readonly DependencyProperty CanGoBackProperty = DependencyProperty.Register(
            nameof(CanGoBack),
            typeof(bool),
            typeof(Shell),
            new PropertyMetadata(false));

        public Shell(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _navigationMap = _serviceProvider.GetRequiredService<INavigationMap>();
        }

        public bool CanGoBack
        {
            get { return (bool)GetValue(CanGoBackProperty); }
            set { SetValue(CanGoBackProperty, value); }
        }

        public void Initialize()
        {
            DataContext = _serviceProvider.GetRequiredService<ShellViewModel>();
        }

        public async Task NavigateBackAsync()
        {
            if (!CanGoBack)
            {
                return;
            }

            var content = PART_NavigationFrame.Content as Control;
            var current = content?.DataContext as INavigationTargetCore;
            var canLeave = await (current?.OnNavigatingFromAsync(NavigationMode.Backward)
                ?? Task.FromResult(true));

            if (!canLeave)
            {
                return;
            }

            (current as IDisposable)?.Dispose();

            _backStack.Pop();
            var backStackEntry = _backStack.Peek();
            current = backStackEntry.Target;
            RefreshCanGoBack();

            content = SetContentForType(current.GetType());
            content.DataContext = current;

            var navigationTargetType = backStackEntry.Target.GetType();
            var paramType = backStackEntry.Parameter.GetType();

            if (navigationTargetType.IsGenericType)
            {
                navigationTargetType.MakeGenericType(paramType);
                var onNavigatedToMethod = navigationTargetType.GetMethod(nameof(INavigationTarget.OnNavigatedToAsync));
                if (onNavigatedToMethod is null)
                {
                    throw new InvalidOperationException($"back navigation target has no {nameof(INavigationTarget.OnNavigatedToAsync)} method");
                }

                onNavigatedToMethod.Invoke(backStackEntry.Target, new object[] { backStackEntry.Parameter, NavigationMode.Backward });
            }
            else
            {
                var target = backStackEntry.Target as INavigationTarget;
                if (target is null)
                {
                    throw new InvalidOperationException("back stack entry is no navigation target");
                }

                target.OnNavigatedToAsync(ImmutableDictionary<string, string>.Empty, NavigationMode.Backward);
            }
        }

        public Task<T?> NavigateAsync<T>()
            where T : class, INavigationTarget
        {
            return NavigateAsync<T, IReadOnlyDictionary<string, string>>(ImmutableDictionary<string, string>.Empty);
        }

        public async Task<T?> NavigateAsync<T, TParameter>(TParameter parameter)
            where T : class, INavigationTarget<TParameter>
        {
            var content = PART_NavigationFrame.Content as Control;
            var oldViewModel = content?.DataContext as INavigationTargetCore;
            var canLeave = await (oldViewModel?.OnNavigatingFromAsync(NavigationMode.Forward)
                ?? Task.FromResult(true));
            if (!canLeave)
            {
                return null;
            }

            content = SetContentForType(typeof(T));
            var newViewModel = _serviceProvider.GetRequiredService<T>();
            content.DataContext = newViewModel;
            await newViewModel.OnNavigatedToAsync(parameter, NavigationMode.Forward);

            _backStack.Push(new BackStackEntry
            {
                Target = newViewModel,
                Parameter = parameter
            });
            RefreshCanGoBack();
            return newViewModel;
        }

        private Control SetContentForType(Type type)
        {
            Control? content;
            var contentType = _navigationMap.GetForTarget(type);
            content = Activator.CreateInstance(contentType) as Control;
            if (content is null)
            {
                throw new InvalidOperationException("Navigation failed");
            }

            PART_NavigationFrame.Content = content;
            return content;
        }

        public Task<T> CreateNavigationTarget<T>()
            where T : INavigationTarget
        {
            return CreateNavigationTarget<T, IReadOnlyDictionary<string, string>>(ImmutableDictionary<string, string>.Empty);
        }

        public async Task<T> CreateNavigationTarget<T, TParam>(TParam parameter)
            where T : INavigationTarget<TParam>
        {
            var result = _serviceProvider.GetRequiredService<T>();
            await result.OnNavigatedToAsync(parameter, NavigationMode.Forward);

            return result;
        }

        private void RefreshCanGoBack()
        {
            CanGoBack = _backStack.Count > 1;
        }

        private record BackStackEntry
        {
            public INavigationTargetCore Target { get; init; }

            public object Parameter { get; init; }
        }
    }
}
