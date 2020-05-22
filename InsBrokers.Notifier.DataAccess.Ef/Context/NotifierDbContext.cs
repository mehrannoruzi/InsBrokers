using InsBrokers.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace InsBrokers.Notifier.DataAccess.Ef
{
    public class NotifierDbContext : ElkDbContext
    {
        public NotifierDbContext() { }

        public NotifierDbContext(DbContextOptions<NotifierDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Application>().HasIndex(x => x.Token).HasName("IX_Token").IsUnique();
            builder.Entity<EventMapper>().HasIndex(x => new { x.Type, x.NotifyStrategy}).HasName("IX_NotifyStrategy").IsUnique();

            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<INotifierEntity>(typeof(Notification).Assembly);
        }
    }
}
