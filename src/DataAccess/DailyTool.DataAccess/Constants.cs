namespace DailyTool.DataAccess
{
    internal static class Constants
    {
        public static IReadOnlyDictionary<Type, string> StoragePaths { get; } = new Dictionary<Type, string>
        {
            [typeof(MeetingInfoStorage)] = Path.Combine(AppContext.BaseDirectory, "meetingInfo.json"),
            [typeof(List<PersonStorage>)] = Path.Combine(AppContext.BaseDirectory, "people.json")
        };
    }
}
