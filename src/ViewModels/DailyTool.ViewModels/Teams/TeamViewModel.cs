using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.Infrastructure.Abstractions.Data;

namespace DailyTool.ViewModels.Teams
{
    public class TeamViewModel : ObservableObject, IIdentifiable
    {
        private int _id;
        private string _name = string.Empty;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
