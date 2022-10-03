using DailyTool.ViewModels.Daily;
using Microsoft.UI.Xaml.Controls;

namespace DailyTool.UserInterface.Daily
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DailyView : Page
    {
        public DailyView()
        {
            InitializeComponent();
        }

        private void ItemsControl_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            NextParticipant();
        }

        private void NextParticipant()
        {
            var vm = DataContext as DailyViewModel;
            if (vm is null)
            {
                return;
            }

            vm.NextSpeakerCommand.Execute(null);
        }

        private void Grid_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            NextParticipant();
        }
    }
}
