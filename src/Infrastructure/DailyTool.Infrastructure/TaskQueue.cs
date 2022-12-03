using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.Infrastructure
{
    public class TaskQueue : ITaskQueue
    {
        private readonly Queue<Func<Task>> _queue = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public void Enqueue(Func<Task> task)
        {
            _queue.Enqueue(task);
        }

        public async Task ProcessQueueAsync(TaskQueueConfig? config)
        {
            config ??= TaskQueueConfig.Default;

            await _semaphore.WaitAsync().ConfigureAwait(config.AllowExecutionOnOtherThread);

            try
            {
                Func<Task>? task = null;
                while (_queue.Any())
                {
                    task = _queue.Dequeue();
                    await task().ConfigureAwait(config.AllowExecutionOnOtherThread);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
