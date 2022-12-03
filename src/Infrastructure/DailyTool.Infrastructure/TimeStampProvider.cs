using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.Infrastructure
{
    public class TimeStampProvider : ITimestampProvider
    {
        public TimeSpan CurrentClock => DateTime.Now.TimeOfDay;
    }
}
