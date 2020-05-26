using System;
using Elk.Core;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.DataAccess.Ef;
using System.Collections.Generic;
using InsBrokers.Service.Resource;
using Action = InsBrokers.Domain.Action;
using DomainStrings = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Service
{
    public class ActionService : IActionService
    {
        private readonly AuthUnitOfWork _authUow;

        public ActionService(AuthUnitOfWork uow)
        {
            _authUow = uow;
        }


        public async Task<IResponse<Action>> AddAsync(Action model)
        {
            await _authUow.ActionRepo.AddAsync(model);

            var saveResult = _authUow.ElkSaveChangesAsync();
            return new Response<Action> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<Action>> UpdateAsync(Action model)
        {
            var action = await _authUow.ActionRepo.FindAsync(model.ActionId);
            if (action == null) return new Response<Action> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Action) };

            action.Name = model.Name;
            action.ControllerName = model.ControllerName;
            action.ActionName = null;// model.ActionName;
            action.Icon = model.Icon;
            action.ParentId = model.ParentId;
            action.ShowInMenu = model.ShowInMenu;
            action.ActionName = model.ActionName;
            action.OrderPriority = model.OrderPriority;

            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<Action> { Result = action, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int actionId)
        {
            _authUow.ActionRepo.Delete(new Action { ActionId = actionId });
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Action>> FindAsync(int actionId)
        {
            var findedAction = await _authUow.ActionRepo.FindAsync(actionId);
            if (findedAction == null) return new Response<Action> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Action) };
            return new Response<Action> { Result = findedAction, IsSuccessful = true };
        }

        public IDictionary<object, object> Get(bool justParents = false)
            => _authUow.ActionRepo.Get(x => !justParents || (x.ControllerName == null && x.ActionName == null),
                x => x.OrderByDescending(a => a.ActionId))
                .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}({v.ControllerName}/{v.ActionName})");

        public PagingListDetails<Action> Get(ActionSearchFilter filter)
        {
            Expression<Func<Action, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.NameF))
                    conditions = x => x.Name.Contains(filter.NameF);
                if (!string.IsNullOrWhiteSpace(filter.ActionNameF))
                    conditions = x => x.ActionName.Contains(filter.ActionNameF.ToLower());
                if (!string.IsNullOrWhiteSpace(filter.ControllerNameF))
                    conditions = x => x.ControllerName.Contains(filter.ControllerNameF.ToLower());
            }

            return _authUow.ActionRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.ActionId));
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
            => _authUow.ActionRepo.Get(conditions: x => x.Name.Contains(searchParameter) || x.ControllerName.Contains(searchParameter) || x.ActionName.Contains(searchParameter), o => o.OrderBy(x => x.ActionId))
            .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}(/{v.ControllerName}/{v.ActionName})");
    }
}