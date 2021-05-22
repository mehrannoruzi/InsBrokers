using Elk.Core;
using InsBrokers.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace InsBrokers.DataAccess.Ef
{
    public class AppDbContext : ElkDbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(x => x.MobileNumber).HasName("IX_MobileNumber").IsUnique();

            builder.OverrideDeleteBehavior(DeleteBehavior.Cascade);
            builder.RegisterAllEntities<IEntity>(typeof(User).Assembly);
        }
    }
}