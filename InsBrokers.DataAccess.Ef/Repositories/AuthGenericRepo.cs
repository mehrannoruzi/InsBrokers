using Elk.EntityFrameworkCore;

namespace InsBrokers.DataAccess.Ef
{
    public class AuthGenericRepo<T> : EfGenericRepo<T> where T : class
    {
        public AuthGenericRepo(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
