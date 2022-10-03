using DailyTool.BusinessLogic.People;

namespace DailyTool.BusinessLogic.Initialization
{
    public interface IInitializationStateService
    {
        Task<InitializationStageState> GetStateAsync();

        Task AddPersonAsync(Person person);

        Task RemovePersonAsync(Person person);

        Task SaveStateAsync();
    }
}
