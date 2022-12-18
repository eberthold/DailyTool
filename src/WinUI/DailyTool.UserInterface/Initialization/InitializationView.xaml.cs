using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace DailyTool.UserInterface.Initialization
{
    public sealed partial class InitializationView : Page
    {
        public InitializationView()
        {
            InitializeComponent();
        }

        private async Task<StorageFile> GetFileForOpen()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".db");
            InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            return file;
        }

        private async Task<StorageFile> GetFileForSave()
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("db", new[] { ".db" });
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
