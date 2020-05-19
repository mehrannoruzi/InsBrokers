using Elk.Core;
using System.Threading.Tasks;

namespace InsBrokers.Domain
{
    public interface IUserRepo : IGenericRepo<User>, IScopedInjection
    {
        Task<User> FindByMobileNumber(long mobileNumber);
    }
}
