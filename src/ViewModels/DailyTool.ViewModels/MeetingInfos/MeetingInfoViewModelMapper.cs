using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.MeetingInfos
{
    public class MeetingInfoViewModelMapper : IMapper<MeetingInfoViewModel, MeetingInfo>, IMapper<MeetingInfo, MeetingInfoViewModel>
    {
        public MeetingInfo Map(MeetingInfoViewModel source)
        {
            var result = new MeetingInfo();
            Merge(source, result);
            return result;
        }

        public void Merge(MeetingInfoViewModel source, MeetingInfo destination)
        {
            destination.StartTime = source.StartTime;
            destination.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri;
        }

        public MeetingInfoViewModel Map(MeetingInfo source)
        {
            var result = new MeetingInfoViewModel();
            Merge(source, result);
            return result;
        }

        public void Merge(MeetingInfo source, MeetingInfoViewModel destination)
        {
            destination.StartTime = source.StartTime;
            destination.Duration = source.Duration;
            destination.SprintBoardUri = source.SprintBoardUri;
        }
    }
}
