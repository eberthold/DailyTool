using Scrummy.Core.BusinessLogic.Meeting;

namespace Scrummy.Core.ViewModels.Parameters
{
    public record MeetingParameter : TeamParameter
    {
        public KnownMeetingType MeetingType { get; init; }
    }
}
