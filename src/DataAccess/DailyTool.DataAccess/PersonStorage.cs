namespace DailyTool.DataAccess
{
    public class PersonStorage
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsParticipating { get; set; }
    }
}