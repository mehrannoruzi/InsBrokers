using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InsBrokers.Domain
{
    public interface IUserRepo : IGenericRepo<User>, IScopedInjection
    {
        Task<User> FindByMobileNumber(long mobileNumber);
        Task<Dictionary<string, int>> GetUserCountLastDaysAsync(int dayCount = 10);
    }
}