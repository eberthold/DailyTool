namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantFactory<T>
        where T : IParticipant
    {
        T Create(Person person);
    }
}
