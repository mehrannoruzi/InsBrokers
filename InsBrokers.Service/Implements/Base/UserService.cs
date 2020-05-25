using System;
using Elk.Core;
using Elk.Cache;
using System.Text;
using System.Linq;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.DataAccess.Ef;
using System.Collections.Generic;
using InsBrokers.Service.Resource;
using InsBrokers.DataAccess.Dapper;
using DomainStrings = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Service
{
    public class UserService : IUserService, IUserActionProvider
    {
        private readonly AppUnitOfWork _appUow;
        private readonly AuthUnitOfWork _authUow;
        private readonly IEmailService _emailService;
        private readonly DapperUserRepo _dapperUserRepo;
        private readonly IMemoryCacheProvider _cacheProvider;
        private string UserCountLastDaysCacheKey() => "UserCountLastDays";
        private string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";

        public UserService(AppUnitOfWork appUow, AuthUnitOfWork authUow, IMemoryCacheProvider cacheProvider,
            IEmailService emailService, DapperUserRepo dapperUserRepo)
        {
            _appUow = appUow;
            _authUow = authUow;
            _emailService = emailService;
            _cacheProvider = cacheProvider;
            _dapperUserRepo = dapperUserRepo;
        }


        #region CRUD

        public async Task<IResponse<User>> AddAsync(User model)
        {
            model.UserId = Guid.NewGuid();
            model.Password = HashGenerator.Hash(model.Password);
            await _appUow.UserRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<User>> UpdateProfile(User model)
        {
            var user = await _appUow.UserRepo.FindAsync(model.UserId);
            if (user == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
                user.Password = HashGenerator.Hash(model.NewPassword);
            user.Name = model.Name;
            user.Family = model.Family;
            user.FatherName = model.FatherName;
            user.Email = model.Email;
            user.BirthDay = model.BirthDay;
            user.NationalCode = model.NationalCode;
            user.BaseInsurance = model.BaseInsurance;

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = user, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<User>> UpdateAsync(User model)
        {
            var user = await _appUow.UserRepo.FindAsync(model.UserId);
            if (user == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            //if (model.MustChangePassword)
            //    findedUser.Password = HashGenerator.Hash(model.Password);
            if (!string.IsNullOrWhiteSpace(user.NewPassword))
                user.Password = HashGenerator.Hash(model.NewPassword);
            user.Name = model.Name;
            user.Family = model.Family;
            user.FatherName = model.FatherName;
            user.NationalCode = model.NationalCode;
            user.Email = model.Email;
            user.BirthDay = model.BirthDay;
            user.IsActive = model.IsActive;

            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = user, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(Guid userId)
        {
            _appUow.UserRepo.Delete(new User { UserId = userId });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<User>> FindAsync(Guid userId)
        {
            var findedUser = await _appUow.UserRepo.FindAsync(userId);
            if (findedUser == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            return new Response<User> { Result = findedUser, IsSuccessful = true };
        }

        #endregion


        public IEnumerable<UserAction> GetUserActions(string userId, string urlPrefix = "")
            => GetAvailableActions(Guid.Parse(userId), null, urlPrefix).ActionList;

        public Task<IEnumerable<UserAction>> GetUserActionsAsync(string userId, string urlPrefix = "")
            => (Task.Run(() => GetAvailableActions(Guid.Parse(userId), null, urlPrefix).ActionList));

        public async Task<IResponse<User>> FindByMobileNumber(long mobileNumber)
        {
            var user = await _appUow.UserRepo.FindByMobileNumber(mobileNumber);
            return new Response<User>
            {
                IsSuccessful = user != null,
                Result = user,
                Message = user == null ? ServiceMessage.RecordNotExist : string.Empty
            };
        }

        public async Task<IResponse<User>> Authenticate(long mobileNumber, string password)
        {
            var user = await _appUow.UserRepo.FindByMobileNumber(mobileNumber);
            if (user == null) return new Response<User> { Message = ServiceMessage.InvalidUsernameOrPassword };

            if (!user.IsActive) return new Response<User> { Message = ServiceMessage.AccountIsBlocked };

            var hashedPassword = HashGenerator.Hash(password);
            if (user.Password != hashedPassword)
            {
                FileLoger.Message($"UserService/Authenticate-> Invalid Password Login ! Username:{mobileNumber} Password:{password}");
                return new Response<User> { Message = ServiceMessage.InvalidUsernameOrPassword };
            }
            //if (user.NewPassword == hashedPassword)
            //{
            //    user.Password = user.NewPassword;
            //    user.NewPassword = null;
            //}
            user.LastLoginDateMi = DateTime.Now;
            user.LastLoginDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<User> { IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message, Result = user };
        }

        private string GetAvailableMenu(List<MenuSPModel> spResult, string urlPrefix = "")
        {
            var sb = new StringBuilder(string.Empty);
            foreach (var item in spResult.Where(x => x.ShowInMenu && (x.IsAction || (!x.IsAction && x.ActionsList.Any(v => v.ShowInMenu)))).OrderBy(x => x.OrderPriority))
            {
                if (!item.IsAction && !item.HasChild) continue;
                #region Add Menu
                sb.AppendFormat("<li {0}><a href='{1}'><i class='{2} default-i'></i><span class='nav-label'>{3}</span> {4}</a>",
                            item.IsAction ? "" : "class='link-parent'",
                            item.IsAction ? $"{urlPrefix}/{item.ControllerName.ToLower()}/{item.ActionName.ToLower()}" : "#",
                            item.Icon,
                            item.Name,
                            item.IsAction ? "" : "<span class='fa arrow'></span>");
                #endregion

                if (!item.IsAction && item.HasChild)
                {
                    #region Add Sub Menu
                    sb.Append("<ul class='nav nav-second-level collapse'>");
                    foreach (var v in item.ActionsList.Where(x => x.ShowInMenu).OrderBy(x => x.OrderPriority))
                    {
                        sb.AppendFormat("<li><a href='{0}' >{1}</a><li>",
                        $"{urlPrefix}/{v.ControllerName.ToLower()}/{v.ActionName.ToLower()}",
                        v.Name);
                    }
                    sb.Append("</ul>");
                    #endregion
                }
                sb.Append("</li>");
            }
            return sb.ToString();
        }

        public MenuModel GetAvailableActions(Guid userId, List<MenuSPModel> spResult = null, string urlPrefix = "")
        {
            var userMenu = (MenuModel)_cacheProvider.Get(MenuModelCacheKey(userId));
            if (userMenu != null) return userMenu;

            userMenu = new MenuModel();
            if (spResult == null) spResult = _dapperUserRepo.GetUserMenu(userId).ToList();

            #region Find Default View
            foreach (var menuItem in spResult)
            {
                if (menuItem.IsAction && menuItem.IsDefault)
                {
                    userMenu.DefaultUserAction = new UserAction
                    {
                        Action = menuItem.ActionName,
                        Controller = menuItem.ControllerName
                    };
                    break;
                }
                var actions = menuItem.ActionsList;
                if (actions.Any(x => x.IsDefault))
                {
                    userMenu.DefaultUserAction = new UserAction
                    {
                        Action = actions.FirstOrDefault(x => x.IsDefault).ActionName,
                        Controller = actions.FirstOrDefault(x => x.IsDefault).ControllerName
                    };
                    break;
                }
            }
            if (userMenu.DefaultUserAction == null || userMenu.DefaultUserAction.Controller == null) return null;
            #endregion

            var userActions =
                spResult.Where(x => x.IsAction)
                .Select(rvm => new UserAction
                {
                    Controller = rvm.ControllerName.ToLower(),
                    Action = rvm.ActionName.ToLower(),
                    RoleId = rvm.RoleId,
                    RoleNameFa = rvm.RoleNameFa
                })
             .Union(
                 spResult.Where(x => !x.IsAction)
                 .SelectMany(x => x.ActionsList.Select(rvm => new UserAction
                 {
                     Controller = rvm.ControllerName.ToLower(),
                     Action = rvm.ActionName.ToLower(),
                     RoleId = rvm.RoleId,
                     RoleNameFa = rvm.RoleNameFa
                 }))).ToList();
            userMenu.Menu = GetAvailableMenu(spResult, urlPrefix);
            userMenu.ActionList = userActions;

            _cacheProvider.Add(MenuModelCacheKey(userId), userMenu, DateTime.Now.AddMinutes(30));
            return userMenu;
        }

        public void SignOut(Guid userId)
        {
            _cacheProvider.Remove(MenuModelCacheKey(userId));
        }

        public PagingListDetails<User> Get(UserSearchFilter filter)
        {
            Expression<Func<User, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FullNameF))
                    conditions = conditions.And(x => x.Name.Contains(filter.FullNameF));
                if (!string.IsNullOrWhiteSpace(filter.EmailF))
                    conditions = x => x.Email.Contains(filter.EmailF);
                if (!string.IsNullOrWhiteSpace(filter.MobileNumberF))
                    conditions = x => x.MobileNumber.ToString().Contains(filter.MobileNumberF);
            }

            var items = _appUow.UserRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.InsertDateMi));
            return items;
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
        {
            var items = _appUow.UserRepo.Get(x => x.Name.Contains(searchParameter) || x.Family.Contains(searchParameter), o => o.OrderByDescending(x => x.UserId));
            return items?.ToDictionary(k => (object)k.UserId, v => (object)$"{v.Name} {v.Family}({v.Email})"); ;
        }

        public async Task<IResponse<string>> RecoverPassword(long mobileNumber, string from, EmailMessage model)
        {
            var user = await _appUow.UserRepo.FindByMobileNumber(mobileNumber);
            if (user == null) return new Response<string> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            user.MustChangePassword = true;
            var newPassword = Randomizer.GetUniqueKey(6);
            user.Password = HashGenerator.Hash(newPassword);
            _appUow.UserRepo.Update(user);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            if (!saveResult.IsSuccessful) return new Response<string> { IsSuccessful = false, Message = saveResult.Message };

            model.Subject = ServiceMessage.RecoverPassword;
            model.Body = model.Body.Fill(newPassword);
            _emailService.Send(user.Email, new List<string> { from }, model);
            return new Response<string> { IsSuccessful = true, Message = saveResult.Message };
        }

        public async Task<IResponse<User>> SignUp(PortalSignUpModel model)
        {
            var user = await _appUow.UserRepo.FirstOrDefaultAsync(conditions: x => x.Email == model.Email);
            if (user != null) return new Response<User> { Message = ServiceMessage.RecordNotExist };
            user = new User
            {
                Name = model.Name,
                Family = model.Family,
                FatherName = model.FatherName,
                Email = model.Email,
                MobileNumber = long.Parse(model.MobileNumber),
                BaseInsurance = model.BaseInsurance,
                Password = HashGenerator.Hash(model.Password),
                IsActive = true,
                NationalCode = model.NationalCode,
                BankAccounts = new List<BankAccount>{
                    new BankAccount
                    {
                        BankName = model.BankName,
                        AccountNumber = model.AccountNumber,
                        Shaba = model.Shaba
                    }
                },
                Addresses = new List<Address> {
                    new Address {
                        Province = model.Province,
                        City = model.City,
                        AddressDetails = model.AddressDetails
                    }
                }
            };
            using var bt = _appUow.Database.BeginTransaction();
            await _appUow.UserRepo.AddAsync(user);
            var save = await _appUow.ElkSaveChangesAsync();
            if (!save.IsSuccessful) return new Response<User> { Message = save.Message };
            await _authUow.UserInRoleRepo.AddAsync(new UserInRole
            {
                RoleId = model.MemberRoleId,
                UserId = user.UserId
            });
            var addUserInRole = await _authUow.ElkSaveChangesAsync();
            if (!addUserInRole.IsSuccessful)
            {
                bt.Rollback();
                return new Response<User> { Message = addUserInRole.Message };
            }
            bt.Commit();
            return new Response<User>
            {
                IsSuccessful = save.IsSuccessful,
                Result = user,
                Message = save.Message
            };
        }

        public async Task<IResponse<int>> GetUserCount()
        {
            var result = new Response<int>();
            try
            {
                result.Result = await _appUow.UserRepo.GetUserCount();

                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return result;
            }
        }

        public async Task<IResponse<Dictionary<string, int>>> GetUserCountLastDaysAsync(int dayCount = 10)
        {
            var result = new Response<Dictionary<string, int>>();
            try
            {
                var cache = (Response<Dictionary<string, int>>)_cacheProvider.Get(UserCountLastDaysCacheKey());
                if (cache != null) return cache;

                result.Result = await _appUow.UserRepo.GetUserCountLastDaysAsync(dayCount);
                if (result.Result.Count() == 0) return new Response<Dictionary<string, int>> { Message = ServiceMessage.RecordNotExist };

                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                _cacheProvider.Add(UserCountLastDaysCacheKey(), result, DateTimeOffset.Now.AddMinutes(30));

                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return result;
            }
        }
    }
}