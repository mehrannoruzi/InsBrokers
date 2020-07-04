using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.DataAccess.Ef;
using InsBrokers.Service.Resource;
using DomainStrings = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Service
{
    public class ContactUsService : IContactUsService
    {
        private readonly AppUnitOfWork _appUow;

        public ContactUsService(AppUnitOfWork uow)
        {
            _appUow = uow;
        }


        public async Task<IResponse<ContactUs>> AddAsync(ContactUs model)
        {
            await _appUow.ContactUsRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<ContactUs> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<ContactUs>> UpdateAsync(ContactUs model)
        {
            var findedContactUs = await _appUow.ContactUsRepo.FindAsync(model.ContactUsId);
            if (findedContactUs == null) return new Response<ContactUs> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.ContactUs) };

            findedContactUs.FullName = model.FullName;
            findedContactUs.Subject = model.Subject;
            findedContactUs.MobileNumber = model.MobileNumber;
            findedContactUs.Content = model.Content;

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<ContactUs> { Result = findedContactUs, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int ContactUsId)
        {
            _appUow.ContactUsRepo.Delete(new ContactUs { ContactUsId = ContactUsId });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<ContactUs>> FindAsync(int ContactUsId)
        {
            var findedContactUs = await _appUow.ContactUsRepo.FindAsync(ContactUsId);
            if (findedContactUs == null) return new Response<ContactUs> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.ContactUs) };

            return new Response<ContactUs> { Result = findedContactUs, IsSuccessful = true };
        }

        public PagingListDetails<ContactUs> Get(ContactUsSearchFilter filter)
        {
            Expression<Func<ContactUs, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Subject))
                    conditions = conditions.And(x => x.Subject.Contains(filter.Subject));
                if (!string.IsNullOrWhiteSpace(filter.MobileNumber))
                    conditions = conditions.And(x => x.MobileNumber == long.Parse(filter.MobileNumber));
            }

            return _appUow.ContactUsRepo.Get(conditions, filter, x => x.OrderByDescending(i => i.ContactUsId));
        }
    }
}