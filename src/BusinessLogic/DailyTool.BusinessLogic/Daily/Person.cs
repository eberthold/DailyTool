namespace DailyTool.BusinessLogic.Daily
{
    public class Person
    {
        private int _id;

        public int Id
        {
            get => _id;
            init => _id = value;
        }

        public string Name { get; set; } = string.Empty;

        public bool IsParticipating { get; set; }

        internal void UpdateId(int id)
        {
            _id = id;
        }
    }
}