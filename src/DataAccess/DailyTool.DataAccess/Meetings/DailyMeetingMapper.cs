using DailyTool.BusinessLogic.Daily;
using DailyTool.DataAccess.Helpers;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.Meetings
{
    public class DailyMeetingMapper : IMapper<DailyMeetingModel, DailyMeetingEntityContainer>, IMapper<DailyMeetingEntityContainer, DailyMeetingModel>
    {
        public DailyMeetingEntityContainer Map(DailyMeetingModel source)
        {
            var result = new DailyMeetingEntityContainer();
            Merge(source, result);
            return result;
        }

        public DailyMeetingModel Map(DailyMeetingEntityContainer source)
        {
            var result = new DailyMeetingModel();
            Merge(source, result);
            return result;
        }

        public void Merge(DailyMeetingModel source, DailyMeetingEntityContainer destination)
        {
            destination.Meeting.Id = source.Id;
            destination.Meeting.TeamId = source.TeamId;
            destination.Meeting.StartTime = source.StartTime;
            destination.Meeting.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri.MapToEntity();
        }

        public void Merge(DailyMeetingEntityContainer source, DailyMeetingModel destination)
        {
            destination.Id = source.Meeting.Id;
            destination.TeamId = source.Meeting.TeamId;
            destination.StartTime = source.Meeting.StartTime;
            destination.Duration = source.Meeting.Duration;
            destination.SprintBoardUri = source.SprintBoardUri.MapToModel();
        }
    }
}
