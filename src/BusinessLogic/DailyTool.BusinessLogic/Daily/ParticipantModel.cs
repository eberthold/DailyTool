using DailyTool.Infrastructure.Abstractions.Data;

namespace DailyTool.BusinessLogic.Daily
{
    public class ParticipantModel : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; init; } = string.Empty;

        public ParticipantState ParticipantState { get; internal set; } = ParticipantState.Queued;

        public double AllocatedProgress { get; internal set; }

        public int Index { get; internal set; }

        public override bool Equals(object? obj)
        {
            var other = obj as ParticipantModel;
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
