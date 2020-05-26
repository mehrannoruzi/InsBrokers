using Elk.EntityFrameworkCore;
using InsBrokers.Domain;

namespace InsBrokers.DataAccess.Ef
{
    public class ActionInRoleRepo : EfGenericRepo<ActionInRole>
    {
        public ActionInRoleRepo(AuthDbContext authContext) : base(authContext)
        {}
    }
}
