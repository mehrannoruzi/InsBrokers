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
    public class RoleService : IRoleService
    {
        private readonly AuthUnitOfWork _authUow;

        public RoleService(AuthUnitOfWork uow)
        {
            _authUow = uow;
        }


        public async Task<IResponse<Role>> AddAsync(Role model)
        {
            await _authUow.RoleRepo.AddAsync(model);

            var saveResult = _authUow.ElkSaveChangesAsync();
            return new Response<Role> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<Role>> UpdateAsync(Role model)
        {
            var findedRole = await _authUow.RoleRepo.FindAsync(model.RoleId);
            if (findedRole == null) return new Response<Role> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Role) };

            findedRole.Enabled = model.Enabled;
            findedRole.RoleNameFa = model.RoleNameFa;
            findedRole.RoleNameEn = model.RoleNameEn;

            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<Role> { Result = findedRole, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int roleId)
        {
            _authUow.RoleRepo.Delete(new Role { RoleId = roleId });
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Role>> FindAsync(int roleId)
        {
            var findedRole = await _authUow.RoleRepo.FindAsync(roleId);
            if (findedRole == null) return new Response<Role> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Role) };

            return new Response<Role> { Result = findedRole, IsSuccessful = true };
        }

        public PagingListDetails<Role> Get(RoleSearchFilter filter)
        {
            Expression<Func<Role, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.RoleNameFaF))
                    conditions = x => x.RoleNameFa.Contains(filter.RoleNameFaF);
                if (!string.IsNullOrWhiteSpace(filter.RoleNameEnF))
                    conditions = x => x.RoleNameEn.Contains(filter.RoleNameEnF);
            }

            return _authUow.RoleRepo.Get(conditions, filter, x => x.OrderByDescending(i => i.RoleId));
        }
    }
}