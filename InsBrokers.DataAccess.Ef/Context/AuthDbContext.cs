using Elk.Core;
using InsBrokers.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace InsBrokers.DataAccess.Ef
{
    public class AuthDbContext : ElkDbContext
    {
        public AuthDbContext() { }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<IAuthEntity>(typeof(Role).Assembly);
        }
    }
}
