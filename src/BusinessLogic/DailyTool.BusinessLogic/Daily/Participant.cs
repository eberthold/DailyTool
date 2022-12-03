namespace DailyTool.BusinessLogic.Daily
{
    public class Participant
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public ParticipantState ParticipantState { get; internal set; } = ParticipantState.Queued;

        public double AllocatedProgress { get; internal set; }

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
