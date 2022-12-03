using DailyTool.BusinessLogic.Daily;
using DailyTool.ViewModels.People;

namespace DailyTool.ViewModels.Extensions
{
    internal static class PersonExtensions
    {
        internal static PersonViewModel ToViewModel(this Person person)
        {
            return new PersonViewModel
            {
                Id = person.Id,
                Name = person.Name,
                IsParticipating = person.IsParticipating
            };
        }

        internal static Person ToBusinessObject(this PersonViewModel person)
        {
            return new Person
            {
                Id = person.Id,
                Name = person.Name,
                IsParticipating = person.IsParticipating
            };
        }
    }
}
