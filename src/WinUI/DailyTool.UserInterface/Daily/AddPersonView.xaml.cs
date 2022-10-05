using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.System;

namespace DailyTool.UserInterface.Daily
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPersonView : UserControl
    {
        public AddPersonView()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private async void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext is null)
            {
                return;
            }

            // TODO: dirty hack, but it does the job for the current state of the tool
            await Task.Delay(250);
            PART_Name.Focus(FocusState.Programmatic);
        }

        private void UserControl_PreviewKeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    if (!PART_Add.Command.CanExecute(null))
                    {
                        break;
                    }

                    PART_Add.Command?.Execute(null);
                    break;

                case VirtualKey.Escape:
                    if (!PART_Cancel.Command.CanExecute(null))
                    {
                        break;
                    }

                    PART_Cancel.Command?.Execute(null);
                    break;

                default:
                    break;
            }
        }
    }
}
