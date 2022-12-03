using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Daily;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DailyTool.UserInterface.Daily
{
    public class ParticipantListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? IsQueuedTemplate { get; set; }

        public DataTemplate? IsActiveTemplate { get; set; }

        public DataTemplate? IsDoneTemplate { get; set; }

        protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
        {
            var participant = item as ParticipantViewModel;
            if (participant is null)
            {
                return null;
            }

            if (participant.IsDone)
            {
                return IsDoneTemplate;
            }

            if (participant.IsActive)
            {
                return IsActiveTemplate;
            }

            return IsQueuedTemplate;
        }
    }
}
