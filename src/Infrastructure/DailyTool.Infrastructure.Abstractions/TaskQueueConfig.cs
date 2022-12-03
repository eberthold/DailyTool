namespace DailyTool.Infrastructure.Abstractions
{
    public record TaskQueueConfig
    {
        public static TaskQueueConfig Default = new();

        public bool AllowExecutionOnOtherThread { get; init; } = true;
    }
}