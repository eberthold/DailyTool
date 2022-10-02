using CommunityToolkit.WinUI;
using DailyTool.ViewModels.Abstractions;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace DailyTool.Packaged.Entry.Threading
{
    public class MainThreadInvoker : IMainThreadInvoker
    {
        private readonly DispatcherQueue _dispatcher;

        public MainThreadInvoker()
        {
            _dispatcher = DispatcherQueue.GetForCurrentThread();
        }

        public void Invoke(Action action)
        {
            if (_dispatcher.HasThreadAccess)
            {
                action();
                return;
            }

            _dispatcher.EnqueueAsync(action);
        }

        public async Task InvokeAsync(Func<Task> task)
        {
            if (_dispatcher.HasThreadAccess)
            {
                await task();
                return;
            }

            await _dispatcher.EnqueueAsync(task);
        }

        public async Task InvokeAsync(Action action)
        {
            if (_dispatcher.HasThreadAccess)
            {
                action();
                return;
            }

            await _dispatcher.EnqueueAsync(action);
        }
    }
}
