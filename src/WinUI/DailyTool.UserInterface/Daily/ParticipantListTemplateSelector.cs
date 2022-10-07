using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
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
            var participant = item as IParticipant;
            if (participant is null)
            {
                return null;
            }

            return participant.ParticipantMode switch
            {
                ParticipantMode.Done => IsDoneTemplate,
                ParticipantMode.Active => IsActiveTemplate,
                ParticipantMode.Queued => IsQueuedTemplate,
                _ => IsQueuedTemplate
            };
        }
    }
}
