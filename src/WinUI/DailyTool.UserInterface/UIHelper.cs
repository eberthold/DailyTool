using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace DailyTool.UserInterface
{
    public class UIHelper
    {
        public static ServiceProvider? ServiceProvider { get; set; }

        public static Window? CurrentWindow { get; set; }
    }
}
