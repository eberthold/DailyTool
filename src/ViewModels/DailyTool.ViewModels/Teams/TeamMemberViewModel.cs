using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.ViewModels.Teams
{
    public class TeamMemberViewModel : ObservableObject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EMailAddress { get; set; } = string.Empty;
    }
}
