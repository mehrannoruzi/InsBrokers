using Elk.Core;
using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Service
{
    public interface IBankAccountService
    {
        Task<IResponse<BankAccount>> AddAsync(BankAccount model);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<BankAccount>> FindAsync(int roleId);
        PagingListDetails<BankAccount> Get(BankAccountSearchFilter filter);
        Task<IResponse<BankAccount>> UpdateAsync(BankAccount model);
    }
}