namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IMeetingInfoService
    {
        Task LoadAsync(DailyState state);

        Task UpdateAsync(MeetingInfo meetingInfo, DailyState state);
    }
}
