namespace DailyTool.BusinessLogic.System
{
    public interface ITimeStampProvider
    {
        public TimeSpan CurrentClock { get; }
    }
}
