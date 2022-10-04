namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IPersonService
    {
        Task LoadAllAsync(DailyState state);

        Task SaveAsync(IReadOnlyCollection<Person> people);

        Task AddPersonAsync(Person person, DailyState state);

        Task RemovePersonAsync(Person person, DailyState state);
    }
}
