namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipant
    {
        public ParticipantMode ParticipantMode { get; set; }

        public int Id { get; init; }

        public string Name { get; init; }

        public TimeSpan AllocatedTalkStart { get; set; }

        public TimeSpan AllocatedTalkDuration { get; set; }

        public double AllocatedTalkProgress { get; set; }

        public int Index { get; set; }
    }
}
