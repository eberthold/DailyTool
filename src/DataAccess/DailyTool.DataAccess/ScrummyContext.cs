using DailyTool.DataAccess.Meetings;
using DailyTool.DataAccess.People;
using DailyTool.DataAccess.Teams;
using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess
{
    public class ScrummyContext : DbContext
    {
        public ScrummyContext(DbContextOptions<ScrummyContext> options)
            : base(options)
        {
        }

        public DbSet<TeamEntity> Teams { get; set; }

        public DbSet<PersonEntity> People { get; set; }

        public DbSet<DailyMeetingEntity> Dailies { get; set; }

        public DbSet<MeetingParticipantEntity> MeetingParticipants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamEntity>();
            modelBuilder.Entity<PersonEntity>();
            modelBuilder.Entity<DailyMeetingEntity>();
            modelBuilder.Entity<MeetingParticipantEntity>();
        }
    }
}
