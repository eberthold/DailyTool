using System.Collections.Immutable;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyState
    {
        public IReadOnlyCollection<Participant> OrderedParticipants { get; internal set; } = ImmutableArray<Participant>.Empty;

        public MeetingInfo MeetingInfo { get; internal set; } = new MeetingInfo();

        public Uri SprintBoardUri { get; internal set; } = new Uri("about:blank");
    }
}
