using DailyTool.BusinessLogic.Daily;

namespace DailyTool.ViewModels.People
{
    public interface IPersonViewModelFactory
    {
        PersonViewModel Create(Person person);
    }
}
