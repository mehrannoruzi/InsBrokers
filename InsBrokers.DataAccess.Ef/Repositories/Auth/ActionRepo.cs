using Elk.EntityFrameworkCore;
using InsBrokers.Domain;

namespace InsBrokers.DataAccess.Ef
{
    public class ActionRepo : EfGenericRepo<Action>
    {
        public ActionRepo(AuthDbContext authContext) : base(authContext)
        {}
    }
}
