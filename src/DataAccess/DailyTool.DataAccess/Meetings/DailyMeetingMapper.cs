using DailyTool.BusinessLogic.Daily;
using DailyTool.DataAccess.Helpers;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.Meetings
{
    public class DailyMeetingMapper : IMapper<DailyMeetingModel, DailyMeetingEntity>, IMapper<DailyMeetingEntity, DailyMeetingModel>
    {
        public DailyMeetingEntity Map(DailyMeetingModel source)
        {
            var result = new DailyMeetingEntity();
            Merge(source, result);
            return result;
        }

        public DailyMeetingModel Map(DailyMeetingEntity source)
        {
            var result = new DailyMeetingModel();
            Merge(source, result);
            return result;
        }

        public void Merge(DailyMeetingModel source, DailyMeetingEntity destination)
        {
            destination.Id = source.Id;
            destination.TeamId = source.TeamId;
            destination.StartTime = source.StartTime;
            destination.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri.MapToEntity();
        }

        public void Merge(DailyMeetingEntity source, DailyMeetingModel destination)
        {
            destination.Id = source.Id;
            destination.TeamId = source.TeamId;
            destination.StartTime = source.StartTime;
            destination.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri.MapToModel();
        }
    }
}
