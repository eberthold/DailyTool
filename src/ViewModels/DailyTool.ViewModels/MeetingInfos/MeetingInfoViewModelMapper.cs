using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.MeetingInfos
{
    public class MeetingInfoViewModelMapper : IMapper<MeetingInfoViewModel, DailyMeetingModel>, IMapper<DailyMeetingModel, MeetingInfoViewModel>
    {
        public DailyMeetingModel Map(MeetingInfoViewModel source)
        {
            var result = new DailyMeetingModel();
            Merge(source, result);
            return result;
        }

        public void Merge(MeetingInfoViewModel source, DailyMeetingModel destination)
        {
            destination.StartTime = source.StartTime;
            destination.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri;
            destination.Id = source.Id;
            destination.TeamId = source.TeamId;
        }

        public MeetingInfoViewModel Map(DailyMeetingModel source)
        {
            var result = new MeetingInfoViewModel();
            Merge(source, result);
            return result;
        }

        public void Merge(DailyMeetingModel source, MeetingInfoViewModel destination)
        {
            destination.StartTime = source.StartTime;
            destination.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri;
            destination.Id = source.Id;
            destination.TeamId = source.TeamId;
        }
    }
}
