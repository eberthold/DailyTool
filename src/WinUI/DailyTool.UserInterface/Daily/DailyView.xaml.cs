using DailyTool.ViewModels.Daily;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

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

        private void ParticipantsTapped(object sender, TappedRoutedEventArgs e)
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

        private void ParticipantsDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            NextParticipant();
        }

        private void ParticipantsRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var vm = DataContext as DailyViewModel;
            if (vm is null)
            {
                return;
            }

            vm.PreviousSpeakerCommand.Execute(null);
        }
    }
}
