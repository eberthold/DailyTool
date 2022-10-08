namespace DailyTool.BusinessLogic.Daily
{
    public class Participant
    {
        public int Id { get; internal init; }

        public string Name { get; internal init; } = string.Empty;

        public ParticipantMode ParticipantMode { get; internal set; } = ParticipantMode.Queued;

        public TimeSpan AllocatedTalkStart { get; internal set; }

        public TimeSpan AllocatedTalkDuration { get; internal set; }

        public double AllocatedTalkProgress { get; internal set; }

        public int Index { get; internal set; }

        public override bool Equals(object? obj)
        {
            var other = obj as Participant;
            if (other is null)
            {
                return false;
            }

            return other.Id == Id;
        }

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}
