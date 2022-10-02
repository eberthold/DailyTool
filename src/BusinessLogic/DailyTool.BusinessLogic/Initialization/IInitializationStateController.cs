using DailyTool.BusinessLogic.People;

namespace DailyTool.BusinessLogic.Initialization
{
    public interface IInitializationStateController
    {
        Task<InitializationStageState> GetStateAsync();

        Task AddPersonAsync(Person person);

        Task RemovePersonAsync(Person person);

        Task SaveStateAsync();
    }
}
