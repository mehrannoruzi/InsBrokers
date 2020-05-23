using Elk.EntityFrameworkCore;
using InsBrokers.Domain;

namespace InsBrokers.DataAccess.Ef
{
    public class UserInRoleRepo : EfGenericRepo<UserInRole>
    {
        public UserInRoleRepo(AuthDbContext authContext) : base(authContext)
        {}
    }
}
