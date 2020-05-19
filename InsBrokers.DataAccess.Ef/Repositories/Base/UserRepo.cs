using InsBrokers.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;

namespace InsBrokers.DataAccess.Ef
{
    public class UserRepo : EfGenericRepo<User>, IUserRepo
    {
        private readonly AppDbContext _appContext;
        public UserRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public async Task<User> FindByMobileNumber(long mobileNumber) => await FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber);
    }
}
