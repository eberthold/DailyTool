using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public class PersonViewModel : ObservableObject
    {
        private string _name = string.Empty;
        private bool _isParticipating;

        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public bool IsParticipating
        {
            get { return _isParticipating; }
            set { SetProperty(ref _isParticipating, value); }
        }
    }
}
