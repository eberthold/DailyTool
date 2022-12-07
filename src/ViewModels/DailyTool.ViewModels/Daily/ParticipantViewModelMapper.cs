﻿using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.Daily
{
    public class ParticipantViewModelMapper : IMapper<Participant, ParticipantViewModel>
    {
        public ParticipantViewModel Map(Participant source)
        {
            var result = new ParticipantViewModel
            {
                Id = source.Id,
                Name = source.Name,
            };

            Merge(source, result);

            return result;
        }

        public void Merge(Participant source, ParticipantViewModel destination)
        {
            destination.AllocatedProgress = source.AllocatedProgress;
            destination.Index = source.Index;
            destination.IsQueued = source.ParticipantState == ParticipantState.Queued;
            destination.IsActive = source.ParticipantState == ParticipantState.Active;
            destination.IsDone = source.ParticipantState == ParticipantState.Done;
        }
    }
}
