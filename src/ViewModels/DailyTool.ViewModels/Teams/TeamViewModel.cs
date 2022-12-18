using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.ViewModels.Teams
{
    public class TeamViewModel : ObservableObject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
