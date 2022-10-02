namespace DailyTool.BusinessLogic.System
{
    public class TimeStampProvider : ITimeStampProvider
    {
        public TimeSpan CurrentClock => DateTime.Now.TimeOfDay;
    }
}
