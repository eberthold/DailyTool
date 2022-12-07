using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.MeetingInfos
{
    public class MeetingInfoMapper : IMapper<MeetingInfoStorage, MeetingInfo>, IMapper<MeetingInfo, MeetingInfoStorage>
    {
        public MeetingInfoStorage Map(MeetingInfo source)
        {
            return new()
            {
                MeetingStartTime = source.StartTime,
                MeetingDuration = source.Duration
            };
        }

        public MeetingInfo Map(MeetingInfoStorage source)
        {
            return new()
            {
                StartTime = source.MeetingStartTime,
                Duration = source.MeetingDuration
            };
        }

        public void Merge(MeetingInfo source, MeetingInfoStorage destination)
        {
            throw new NotImplementedException();
        }

        public void Merge(MeetingInfoStorage source, MeetingInfo destination)
        {
            throw new NotImplementedException();
        }
    }
}
