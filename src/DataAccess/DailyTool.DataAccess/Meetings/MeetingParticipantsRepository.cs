using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using Microsoft.EntityFrameworkCore;
using Scrummy.Core.BusinessLogic.Exceptions;

namespace DailyTool.DataAccess.Meetings
{
    public class MeetingParticipantsRepository : IMeetingParticipantsRepository
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public MeetingParticipantsRepository(
            IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IReadOnlyCollection<ParticipantModel>> GetParticipantsAsync(int dailyMeetingId)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var entities = await context
                .MeetingParticipants
                .Include(x => x.Person)
                .Where(x => x.DailyMeetingId == dailyMeetingId)
                .ToListAsync()
                .ConfigureAwait(false);

            return entities
                .Select(x =>
                    new ParticipantModel
                    {
                        Id = x.Id,
                        Name = x.Person.Name
                    })
                .ToList();
        }

        public async Task<int> AddParticipantAsync(int dailyMeetingId, int personId)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var meeting = await context.Meetings.FindAsync(dailyMeetingId).ConfigureAwait(false);
            if (meeting is null)
            {
                throw new NotFoundException<DailyMeetingModel>(dailyMeetingId);
            }

            var person = await context.People.FindAsync(personId).ConfigureAwait(false);
            if (person is null)
            {
                throw new NotFoundException<PersonModel>(personId);
            }

            var entity = new MeetingParticipantEntity
            {
                DailyMeeting = meeting,
                Person = person
            };

            context.MeetingParticipants.Add(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return entity.Id;
        }

        public async Task RemoveParticipantAsync(int participantId)
        {
            await using var context = _dbContextFactory.CreateDbContext();

            var entity = await context.MeetingParticipants.FindAsync(participantId).ConfigureAwait(false);
            if (entity is null)
            {
                return;
            }

            context.MeetingParticipants.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
