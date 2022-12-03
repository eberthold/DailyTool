namespace DailyTool.Infrastructure.Abstractions
{
    public interface ITimestampProvider
    {
        public TimeSpan CurrentClock { get; }
    }
}
