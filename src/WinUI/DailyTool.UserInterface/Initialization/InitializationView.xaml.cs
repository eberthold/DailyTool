using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace DailyTool.UserInterface.Initialization
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitializationView : Page
    {
        private readonly IMeetingInfoRepository? _meetingInfoRepository;
        private readonly IPersonRepository? _personRepository;

        public InitializationView()
        {
            InitializeComponent();

            _meetingInfoRepository = UIHelper.ServiceProvider?.GetService<IMeetingInfoRepository>();
            _personRepository = UIHelper.ServiceProvider?.GetService<IPersonRepository>();
        }

        private async void OnImportMeetingInfo(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var file = await GetFileForOpen();
            await ((IImportable)_meetingInfoRepository).ImportAsync(file.Path);
        }

        private async void OnExportMeetingInfo(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var file = await GetFileForSave();
            await ((IExportable)_meetingInfoRepository).ExportAsync(file.Path);
        }

        private async void OnImportPeople(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var file = await GetFileForOpen();
            await((IImportable)_personRepository).ImportAsync(file.Path);
        }

        private async void OnExportPeople(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var file = await GetFileForSave();
            await((IExportable)_personRepository).ExportAsync(file.Path);
        }

        private async Task<StorageFile> GetFileForOpen()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            return file;
        }

        private async Task<StorageFile> GetFileForSave()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("json", new[] { ".json" });
            InitializePicker(picker);
            var file = await picker.PickSaveFileAsync();
            return file;
        }

        private void InitializePicker(object picker)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(UIHelper.CurrentWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        }
    }
}
