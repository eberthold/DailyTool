namespace DailyTool.DataAccess.People
{
    public class PersonStorage
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsParticipating { get; set; }
    }
}