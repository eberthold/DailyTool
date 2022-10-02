using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.BusinessLogic.People
{
    public class Person : ObservableObject
    {
        private string _name = string.Empty;
        private bool _isParticipating;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool IsParticipating
        {
            get => _isParticipating;
            set => SetProperty(ref _isParticipating, value);
        }
    }
}