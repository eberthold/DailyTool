using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.MeetingInfos
{
    public class MeetingInfoViewModelMapper : IMapper<MeetingInfoViewModel, MeetingInfo>, IMapper<MeetingInfo, MeetingInfoViewModel>
    {
        public MeetingInfo Map(MeetingInfoViewModel source)
        {
            return new MeetingInfo
            {
                StartTime = source.StartTime,
                Duration = source.Duration,
                SprintBoardUri = source.SprintBoardUri
            };
        }

        public MeetingInfoViewModel Map(MeetingInfo source)
        {
            return new MeetingInfoViewModel
            {
                StartTime = source.StartTime,
                Duration = source.Duration,
                SprintBoardUri = source.SprintBoardUri
            };
        }
    }
}
