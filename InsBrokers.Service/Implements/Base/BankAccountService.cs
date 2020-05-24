using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using InsBrokers.DataAccess.Ef;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InsBrokers.Service.Resource;

namespace InsBrokers.Service
{
    public class BankAccountService : IBankAccountService
    {
        private readonly AppUnitOfWork _appUow;
        private readonly IGenericRepo<BankAccount> _BankAccountRepo;

        public BankAccountService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
            _BankAccountRepo = appUOW.BankAccountRepo;
        }

        public PagingListDetails<BankAccount> Get(BankAccountSearchFilter filter)
        {
            Expression<Func<BankAccount, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.UserId == filter.UserId);
                if (filter.Name != null)
                    conditions = conditions.And(x => x.BankName == filter.Name);
            }

            return _BankAccountRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.BankAccountId)); ;
        }


        public async Task<IResponse<BankAccount>> AddAsync(BankAccount model)
        {
            await _appUow.BankAccountRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<BankAccount> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<BankAccount>> UpdateAsync(BankAccount model)
        {
            var BankAccount = await _BankAccountRepo.FindAsync(model.BankAccountId);
            if (BankAccount == null) return new Response<BankAccount> { Message = ServiceMessage.RecordNotExist };

            BankAccount.BankName = model.BankName;
            BankAccount.Shaba = model.Shaba;
            BankAccount.AccountNumber = model.AccountNumber;

            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<BankAccount> { Result = BankAccount, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _BankAccountRepo.Delete(new BankAccount { BankAccountId = id });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<BankAccount>> FindAsync(int roleId)
        {
            var BankAccount = await _BankAccountRepo.FindAsync(roleId);
            if (BankAccount == null) return new Response<BankAccount> { Message = ServiceMessage.RecordNotExist };
            return new Response<BankAccount> { Result = BankAccount, IsSuccessful = true };
        }
    }
}