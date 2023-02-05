using DailyTool.UserInterface.Navigation;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Scrummy.Core.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace DailyTool.UserInterface
{
    public sealed partial class Shell : UserControl, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUriNavigationHandler _uriNavigationHandler;
        private readonly INavigationMap _navigationMap;
        private readonly Stack<object> _backStack = new Stack<object>();

        public static readonly DependencyProperty CanGoBackProperty = DependencyProperty.Register(
            nameof(CanGoBack),
            typeof(bool),
            typeof(Shell),
            new PropertyMetadata(false));

        public Shell(
            IServiceProvider serviceProvider,
            IUriNavigationHandler uriNavigationHandler)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _uriNavigationHandler = uriNavigationHandler;
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
            var current = content?.DataContext as INavigationTarget;
            var canLeave = await (current?.OnNavigatingFromAsync(NavigationMode.Backward)
                ?? Task.FromResult(true));

            if (!canLeave)
            {
                return;
            }

            (current as IDisposable)?.Dispose();

            _backStack.Pop();
            var backStackEntry = _backStack.Peek() as INavigationTarget;
            if (backStackEntry is null)
            {
                throw new NavigationException("no matching back stack entry found");
            }

            content = SetContentForType(backStackEntry.GetType());
            content.DataContext = backStackEntry;
        }

        public async Task<T?> NavigateAsync<T>()
            where T : class, INavigationTarget
        {
            var navigationUri = _uriNavigationHandler.GetParameterlessUriOf<T>();

            var target = await NavigateToUriAsync(navigationUri);
            return target as T;
        }

        public async Task<T?> NavigateAsync<T, TParameter>(TParameter parameter)
            where T : class, INavigationTarget<TParameter>
        {
            var navigationUri = _uriNavigationHandler.GetMatchingUriOf<T, TParameter>();

            // TODO: Fill Parameters

            var target = await NavigateToUriAsync(navigationUri);
            return target as T;
        }

        private async Task<object?> NavigateToUriAsync(string navigationUri)
        {
            var content = PART_NavigationFrame.Content as Control;
            var oldViewModel = content?.DataContext as INavigationTarget;
            var canLeave = await (oldViewModel?.OnNavigatingFromAsync(NavigationMode.Forward)
                ?? Task.FromResult(true));
            if (!canLeave)
            {
                return null;
            }

            var target = await CreateNavigationTargetForUriAsync(navigationUri);
            if (target is null)
            {
                return null;
            }

            content = SetContentForType(target.GetType());
            content.DataContext = target;

            _backStack.Push(target);
            RefreshCanGoBack();
            return target;
        }

        private async Task<object?> CreateNavigationTargetForUriAsync(string navigationUri)
        {
            var match = await _uriNavigationHandler.GetTargetTypeForUri(navigationUri);
            var target = _serviceProvider.GetService(match.Type) as INavigationTarget;
            if (target is null)
            {
                return null;
            }

            var typedNavigationTarget = typeof(INavigationTarget<>);
            var typedTargets = match
                .Type
                .GetInterfaces()
                .Where(x =>
                    x.IsGenericType &&
                    x == typedNavigationTarget.MakeGenericType(x.GenericTypeArguments))
                .ToList();

            foreach (var typedTarget in typedTargets)
            {
                var method = GetType()
                    .GetMethod(nameof(TrySetParameters), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(typedTarget.GenericTypeArguments);

                var task = (Task<bool>)method.Invoke(this, new object[] { target, match, navigationUri });
                await task;
            }

            await target.OnNavigatedToAsync(NavigationMode.Forward);
            return target;
        }

        private async Task<bool> TrySetParameters<T>(INavigationTarget<T> target, UriTemplateMatch match, string uri)
            where T : class, new()
        {
            var isMatch = _uriNavigationHandler.TryMatchParameters<T>(match.UriTemplate, uri, out T? parameter);
            if (!isMatch)
            {
                return false;
            }

            target.SetParameters(parameter);
            return true;
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

        public async Task<T?> CreateNavigationTarget<T>()
            where T : class, INavigationTarget
        {
            var navigationUri = _uriNavigationHandler.GetParameterlessUriOf<T>();

            var target = await CreateNavigationTargetForUriAsync(navigationUri);
            return target as T;
        }

        public async Task<T?> CreateNavigationTarget<T, TParam>(TParam parameter)
            where T : class, INavigationTarget<TParam>
        {
            var navigationUri = _uriNavigationHandler.GetMatchingUriOf<T, TParam>();

            var target = await NavigateToUriAsync(navigationUri);
            return target as T;
        }

        private void RefreshCanGoBack()
        {
            CanGoBack = _backStack.Count > 1;
        }
    }
}
