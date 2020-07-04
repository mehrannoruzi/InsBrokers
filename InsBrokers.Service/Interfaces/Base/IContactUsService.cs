using Elk.Core;
using InsBrokers.Domain;
using System.Threading.Tasks;

namespace InsBrokers.Service
{
    public interface IContactUsService
    {
        Task<IResponse<ContactUs>> AddAsync(ContactUs model);
        Task<IResponse<bool>> DeleteAsync(int ContactUsId);
        Task<IResponse<ContactUs>> FindAsync(int ContactUsId);
        PagingListDetails<ContactUs> Get(ContactUsSearchFilter filter);
        Task<IResponse<ContactUs>> UpdateAsync(ContactUs model);
    }
}