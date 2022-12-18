using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.Teams
{
    public class TeamMemberMapper : IMapper<PersonModel, TeamMemberViewModel>
    {
        public TeamMemberViewModel Map(PersonModel source)
        {
            var result = new TeamMemberViewModel();
            Merge(source, result);
            return result;
        }

        public void Merge(PersonModel source, TeamMemberViewModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.EMailAddress = source.EMailAddress;
        }
    }
}
