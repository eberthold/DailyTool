using DailyTool.BusinessLogic.Daily;
using DailyTool.DataAccess.Helpers;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.People
{
    public class PersonMapper : IMapper<PersonEntity, PersonModel>, IMapper<PersonModel, PersonEntity>
    {
        public PersonEntity Map(PersonModel source)
        {
            var result = new PersonEntity();
            Merge(source, result);
            return result;
        }

        public PersonModel Map(PersonEntity source)
        {
            var result = new PersonModel();
            Merge(source, result);
            return result;
        }

        public void Merge(PersonModel source, PersonEntity destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.EMailAddress = source.EMailAddress.MapToEntity();
        }

        public void Merge(PersonEntity source, PersonModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.EMailAddress = source.EMailAddress.MapToModel();
        }
    }
}
