using Elk.EntityFrameworkCore;
using InsBrokers.Domain;

namespace InsBrokers.DataAccess.Ef
{
    public class RoleRepo : EfGenericRepo<Role>
    {
        public RoleRepo(AuthDbContext authContext) : base(authContext)
        {}
    }
}
