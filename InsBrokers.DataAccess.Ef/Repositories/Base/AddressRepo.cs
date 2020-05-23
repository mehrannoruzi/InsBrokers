using Elk.EntityFrameworkCore;
using InsBrokers.Domain;

namespace InsBrokers.DataAccess.Ef
{
    public class AddressRepo : EfGenericRepo<Address>
    {
        private readonly AppDbContext _appContext;
        public AddressRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

    }
}
