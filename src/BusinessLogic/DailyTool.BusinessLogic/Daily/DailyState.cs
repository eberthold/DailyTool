using System.Collections.Immutable;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyState
    {
        public IReadOnlyCollection<ParticipantModel> OrderedParticipants { get; internal set; } = ImmutableArray<ParticipantModel>.Empty;

        public DailyMeetingModel MeetingInfo { get; internal set; } = new DailyMeetingModel();

        public int MeetingId { get; internal set; }

        public Uri SprintBoardUri { get; internal set; } = new Uri("about:blank");
    }
}
