namespace DailyTool.Infrastructure.Abstractions
{
    public interface ITaskQueue
    {
        void Enqueue(Func<Task> task);

        Task ProcessQueueAsync(TaskQueueConfig? config = null);
    }
}
