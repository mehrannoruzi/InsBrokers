using System;
using Elk.Core;
using Elk.Cache;
using System.IO;
using System.Text;
using System.Linq;
using ClosedXML.Excel;
using InsBrokers.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InsBrokers.CrossCutting;
using InsBrokers.DataAccess.Ef;
using Microsoft.AspNetCore.Http;
using InsBrokers.InfraStructure;
using System.Collections.Generic;
using InsBrokers.Service.Resource;
using InsBrokers.DataAccess.Dapper;
using Microsoft.Extensions.Configuration;
using DomainStrings = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Service
{
    public class UserService : IUserService, IUserActionProvider
    {
        private readonly AppUnitOfWork _appUow;
        private readonly AuthUnitOfWork _authUow;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly DapperUserRepo _dapperUserRepo;
        private readonly IMemoryCacheProvider _cacheProvider;
        private string UserCountLastDaysCacheKey() => "UserCountLastDays";
        //private string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";

        public UserService(AppUnitOfWork appUow, AuthUnitOfWork authUow, IMemoryCacheProvider cacheProvider,
            IEmailService emailService, DapperUserRepo dapperUserRepo, IConfiguration configuration)
        {
            _appUow = appUow;
            _authUow = authUow;
            _emailService = emailService;
            _configuration = configuration;
            _cacheProvider = cacheProvider;
            _dapperUserRepo = dapperUserRepo;
        }



        #region CRUD
        private string GetDayOfDate(string persianDate) => persianDate.Substring(8, 2);
        private string GetMounthOfDate(string persianDate) => persianDate.Substring(5, 2);
        private string GetYearOfDate(string persianDate) => persianDate.Substring(0, 4);


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
            user.IdentityNumber = model.IdentityNumber;
            user.BaseInsurance = model.BaseInsurance;
            user.Gender = model.Gender;

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
            user.IdentityNumber = model.IdentityNumber;
            user.Email = model.Email;
            user.BirthDay = model.BirthDay;
            user.BirthDayMi = PersianDateTime.Parse(model.BirthDay).ToDateTime();
            user.IsActive = model.IsActive;
            user.BaseInsurance = model.BaseInsurance;
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = user, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
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
            var findedUser = await _appUow.UserRepo.FirstOrDefaultAsync(conditions: x => x.UserId == userId, new List<Expression<Func<User, object>>> { x => x.Addresses, x => x.BankAccounts, x => x.Relatives });
            if (findedUser == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            return new Response<User> { Result = findedUser, IsSuccessful = true };
        }

        public async Task<IResponse<User>> FindWithAttachmentsAsync(Guid userId)
        {
            var findedUser = await _appUow.UserRepo.FirstOrDefaultAsync(
                conditions: x => x.UserId == userId,
                includeProperties: new List<Expression<Func<User, object>>>
                {
                    x => x.Addresses,
                    x => x.BankAccounts,
                    x => x.Relatives,
                    x => x.UserAttachments
                });
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
            _appUow.UserRepo.Update(user);
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
            var userMenu = (MenuModel)_cacheProvider.Get(GlobalVariables.CacheSettings.MenuModelCacheKey(userId));
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
            var userActions = new List<UserAction>();
            foreach (var item in spResult)
            {
                if (item.IsAction)
                    userActions.Add(new UserAction
                    {
                        Controller = item.ControllerName.ToLower(),
                        Action = item.ActionName.ToLower(),
                        RoleId = item.RoleId,
                        RoleNameFa = item.RoleNameFa
                    });
                if (item.ActionsList != null)
                    foreach (var child in item.ActionsList)
                        userActions.Add(new UserAction
                        {
                            Controller = child.ControllerName.ToLower(),
                            Action = child.ActionName.ToLower(),
                            RoleId = child.RoleId,
                            RoleNameFa = child.RoleNameFa
                        });
            }
            userActions = userActions.Distinct().ToList();
            //var userActions =
            //    spResult.Where(x => x.IsAction)
            //    .Select(rvm => new UserAction
            //    {
            //        Controller = rvm.ControllerName.ToLower(),
            //        Action = rvm.ActionName.ToLower(),
            //        RoleId = rvm.RoleId,
            //        RoleNameFa = rvm.RoleNameFa
            //    })
            //    .Union()
            // .Union(
            //     spResult.Where(x => !x.IsAction)
            //     .SelectMany(x => x.ActionsList.Select(rvm => new UserAction
            //     {
            //         Controller = rvm.ControllerName.ToLower(),
            //         Action = rvm.ActionName.ToLower(),
            //         RoleId = rvm.RoleId,
            //         RoleNameFa = rvm.RoleNameFa
            //     }))).ToList();
            userMenu.Menu = GetAvailableMenu(spResult, urlPrefix);
            userMenu.ActionList = userActions;

            _cacheProvider.Add(GlobalVariables.CacheSettings.MenuModelCacheKey(userId), userMenu, DateTime.Now.AddMinutes(30));
            return userMenu;
        }

        public List<MenuSPModel> GetMainMenu(Guid userId)
        {
            var userMenu = (List<MenuSPModel>)_cacheProvider.Get(GlobalVariables.CacheSettings.MainMenuCacheKey(userId));
            if (userMenu != null) return userMenu;

            userMenu = _dapperUserRepo.GetUserMenu(userId).ToList();
            _cacheProvider.Add(GlobalVariables.CacheSettings.MainMenuCacheKey(userId), userMenu, DateTime.Now.AddMinutes(30));
            return userMenu;

        }

        public void SignOut(Guid userId)
        {
            _cacheProvider.Remove(GlobalVariables.CacheSettings.MenuModelCacheKey(userId));
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

                if (!string.IsNullOrWhiteSpace(filter.DateFrom))
                {
                    var dateFrom = PersianDateTime.Parse(filter.DateFrom).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi >= dateFrom);
                }
                if (!string.IsNullOrWhiteSpace(filter.DateTo))
                {
                    var dateTo = PersianDateTime.Parse(filter.DateTo).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi < dateTo);
                }
            }

            var users = _appUow.UserRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.InsertDateMi));
            return users;
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
        {
            var items = _appUow.UserRepo.Get(x => x.Name.Contains(searchParameter) || x.Family.Contains(searchParameter) || x.NationalCode.Contains(searchParameter), o => o.OrderByDescending(x => x.UserId));
            return items?.ToDictionary(k => (object)k.UserId, v => (object)$"{v.Name} {v.Family}({v.NationalCode})"); ;
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

        public async Task<IResponse<object>> AddAttachments(IFormFile file, AttachmentType type)
        {
            var response = new Response<object>();
            try
            {
                if (file.IsNull() || file.Length < 0) return new Response<object> { Message = "هیچ فایلی آپلود نشده است." };
                if (file.Length > (1000 * 1024)) return new Response<object> { Message = "فایل آپلود شده نباید بیشتر از 1MB باشد." };

                #region Save Attachment To Host
                var fullPath = HttpFileTools.GetPath(
                    fileNameWithExtension: type.ToString() + Path.GetExtension(file.FileName),
                    root: "Files/UserAttachment");
                if (string.IsNullOrWhiteSpace(fullPath)) return new Response<object> { Message = ServiceMessage.Error };

                var fileUrl = HttpFileTools.Save(file, fullPath);
                #endregion

                #region Save Attachment To Database
                var userAttschment = new UserAttachment
                {
                    Url = fileUrl,
                    Size = file.Length,
                    UserAttachmentType = type,
                    FileType = FileType.Image,
                    Extention = Path.GetExtension(file.FileName),
                    Name = Path.GetFileNameWithoutExtension(fileUrl),
                    UserId = Guid.Parse("EC5FC734-9046-4CFB-65C3-08D7FFC82032")
                };

                await _appUow.UserAttachmentRepo.AddAsync(userAttschment);
                var fileSaveResult = await _appUow.ElkSaveChangesAsync();
                if (fileSaveResult.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.Result = userAttschment;
                    response.Message = ServiceMessage.Success;
                }
                else
                    response.Message = ServiceMessage.Error;
                #endregion

                return response;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                response.Message = ServiceMessage.Exception;
                return response;
            }
        }

        public async Task<IResponse<bool>> DeleteAttachment(int attachmentId)
        {
            var response = new Response<bool>();
            try
            {
                var attachment = await _appUow.UserAttachmentRepo.FindAsync(attachmentId);
                _appUow.UserAttachmentRepo.Delete(attachment);
                var deleteFileResult = await _appUow.ElkSaveChangesAsync();
                if (deleteFileResult.IsSuccessful)
                {
                    response.Result = true;
                    response.IsSuccessful = true;
                    response.Message = ServiceMessage.Success;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccessful = true;
                    response.Message = ServiceMessage.Error;
                }

                return response;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                response.Message = ServiceMessage.Exception;
                return response;
            }
        }

        public List<UserAttachment> GetUserAttachments(Guid userId)
        {
            var response = new Response<List<UserAttachment>>();
            try
            {
                return _appUow.UserAttachmentRepo.Get(conditions: x => x.UserId == userId);
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return null;
            }
        }

        public async Task<IResponse<User>> SignUp(PortalSignUpModel model)
        {
            using (var trans = _appUow.Database.BeginTransaction())
            {
                try
                {
                    #region Check User Exist
                    var mobileNumber = long.Parse(model.MobileNumber);
                    var user = await _appUow.UserRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber);
                    if (user != null) return new Response<User> { Message = ServiceMessage.DuplicateRecord };
                    #endregion

                    #region Save User
                    user = new User
                    {
                        Name = model.Name,
                        Family = model.Family,
                        FatherName = model.FatherName,
                        MobileNumber = mobileNumber,
                        BaseInsurance = model.BaseInsurance,
                        Password = HashGenerator.Hash(model.NationalCode),
                        IsActive = true,
                        Gender = model.Gender,
                        BirthDay = model.BirthDay,
                        BirthDayMi = PersianDateTime.Parse(model.BirthDay).ToDateTime(),
                        NationalCode = model.NationalCode,
                        IdentityNumber = model.IdentityNumber,
                        InsurancePlan = model.InsurancePlan,
                        InsuranceNumber = model.InsuranceNumber,
                        Organization = model.Organization,
                        HasAccidentsInsurance = true,

                        BankAccounts = new List<BankAccount>{
                            new BankAccount
                            {
                                BankName = model.BankName,
                                AccountNumber = model.AccountNumber,
                                Shaba = model.Shaba
                            }
                        },
                        Addresses = new List<Address> {
                            new Address
                            {
                                Province = model.Province,
                                City = model.City,
                                AddressDetails = model.AddressDetails
                            }
                        }
                    };

                    await _appUow.UserRepo.AddAsync(user);
                    var save = await _appUow.ElkSaveChangesAsync();
                    if (!save.IsSuccessful) return new Response<User> { Message = save.Message };
                    #endregion

                    #region Save User Attachments
                    foreach (var attachmentId in model.UserAttachmentIds)
                    {
                        var userAttachment = await _appUow.UserAttachmentRepo.FindAsync(attachmentId);
                        if (userAttachment.IsNull()) continue;

                        userAttachment.UserId = user.UserId;
                    }

                    var updateUserAttachments = await _appUow.ElkSaveChangesAsync();
                    if (!updateUserAttachments.IsSuccessful)
                    {
                        trans.Rollback();
                        return new Response<User> { Message = ServiceMessage.Error };
                    }
                    #endregion

                    #region Save User Role
                    await _authUow.UserInRoleRepo.AddAsync(new UserInRole { RoleId = model.MemberRoleId, UserId = user.UserId });
                    var addUserInRole = await _authUow.ElkSaveChangesAsync();
                    if (!addUserInRole.IsSuccessful)
                    {
                        trans.Rollback();
                        return new Response<User> { Message = addUserInRole.Message };
                    }
                    #endregion

                    #region Create Result
                    await trans.CommitAsync();
                    return new Response<User>
                    {
                        Result = user,
                        Message = save.Message,
                        IsSuccessful = save.IsSuccessful
                    };
                    #endregion
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    FileLoger.Error(e);
                    return new Response<User> { };
                }
            }
        }

        private int GetInsurancePlanPrice(string plan)
        {
            if (plan.Contains("برنزی")) return int.Parse(_configuration["InsurancePlanSettings:BoronziPlanPrice"]);
            else if (plan.Contains("نقره ای")) return int.Parse(_configuration["InsurancePlanSettings:NoghreiPlanPrice"]);
            else if (plan.Contains("طلایی")) return int.Parse(_configuration["InsurancePlanSettings:TalaeiPlanPrice"]);

            return 0;
        }

        public async Task<IResponse<InsuranceInformation>> GetInsuranceInfo(Guid userId)
        {
            var response = new Response<InsuranceInformation>();
            try
            {
                var user = await _appUow.UserRepo.FirstOrDefaultAsync(conditions: x => x.UserId == userId, includeProperties: new List<Expression<Func<User, object>>> { x => x.Relatives });
                if (user.IsNull()) return new Response<InsuranceInformation> { Message = ServiceMessage.RecordNotExist };

                var relativesCount = user.Relatives.Count() + 1;
                var insurancePlanPrice = GetInsurancePlanPrice(user.InsurancePlan);
                var totalPrice = relativesCount * insurancePlanPrice;
                var accidentsInsurancePrice = int.Parse(_configuration["InsurancePlanSettings:AccidentsInsurancePrice"]);

                response.Result = new InsuranceInformation();
                response.Result = new InsuranceInformation
                {
                    PaymentPart1 = (totalPrice / 3) + accidentsInsurancePrice,
                    PaymentPart2 = (totalPrice / 3),
                    PaymentPart3 = totalPrice - ((totalPrice / 3) * 2),
                    PaymentPart2Date = _configuration["InsurancePlanSettings:PaymentPart2Date"],
                    PaymentPart3Date = _configuration["InsurancePlanSettings:PaymentPart3Date"]

                };
                response.Result.Details = new List<InsuranceDetail>
                {
                    new InsuranceDetail{ Type="UserInsuranceInfo", Plan = user.InsurancePlan, Price = insurancePlanPrice, Count = relativesCount, TotalPrice = totalPrice},
                    new InsuranceDetail{ Type="AccidentsInsuranceInfo", Plan = DomainStrings.AccidentsInsurance, Price = accidentsInsurancePrice, Count = 1, TotalPrice = int.Parse(_configuration["InsurancePlanSettings:AccidentsInsurancePrice"]) * 1 },
                };

                response.IsSuccessful = true;
                response.Message = ServiceMessage.Success;
                return response;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return response;
            }
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

        public string ExportToExcel(UserSearchFilter filter)
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

            var items = _appUow.UserRepo.Get(conditions, x => x.OrderByDescending(u => u.InsertDateMi));
            var sb = new StringBuilder(",Fullname,Mobile Number,National Code,Gender,Base Insurance,Is Active,Email,FatherName,BirthDay,Register Date" + Environment.NewLine);
            int idx = 1;
            foreach (var item in items)
            {
                sb.Append($"{idx},{item.Fullname},{item.MobileNumber},{item.NationalCode},{item.Gender.GetDescription()},{item.BaseInsurance.GetDescription()},{(item.IsActive ? "بلی" : "خیر")},{item.Email},{item.FatherName},{item.BirthDay},{item.InsertDateSh}" + Environment.NewLine);
                idx++;
            }
            return sb.ToString();
        }

        public string Export(UserSearchFilter filter)
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

                if (!string.IsNullOrWhiteSpace(filter.DateFrom))
                {
                    var dateFrom = PersianDateTime.Parse(filter.DateFrom).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi >= dateFrom);
                }
                if (!string.IsNullOrWhiteSpace(filter.DateTo))
                {
                    var dateTo = PersianDateTime.Parse(filter.DateTo).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi < dateTo);
                }
            }

            var users = _appUow.UserRepo.Get(
                conditions: conditions,
                orderBy: x => x.OrderBy(x => x.UserId),
                includeProperties: new List<Expression<Func<User, object>>>
                {
                    x => x.Addresses,
                    x => x.BankAccounts,
                    x => x.Relatives,
                });

            int rowNumber = 0;
            var titleEn = ",Name,LName,PersonelCode,SodurPlace,IdentityNo,BirthYear,BirthMonth,BirthDay,CodeMelli,Address,Tel,Jens,FatherName,BSKind,Nesbat,BeginDate,EndDate,TakafolKind,Tarh,Workunit,AccNOBimegarOneNo,Bank,ShebaAcc,Mobile,Email,RegisterDate" + Environment.NewLine;
            var result = new StringBuilder(titleEn);
            foreach (var user in users)
            {
                rowNumber++;
                result.Append($"{rowNumber},{user.Name},{user.Family},{0},,{user.IdentityNumber},{GetYearOfDate(user.BirthDay)},{GetMounthOfDate(user.BirthDay)},{GetDayOfDate(user.BirthDay)},=\"{user.NationalCode}\",{user.Addresses.FirstOrDefault()?.FullAddress},,{(byte)user.Gender},{user.FatherName},{(byte)InsuranceType.Main},,,,,,,,{user.BankAccounts.FirstOrDefault()?.BankName.GetDescription()},{user.BankAccounts.FirstOrDefault()?.Shaba},{user.MobileNumber},{user.Email},{user.InsertDateSh}" + Environment.NewLine);

                foreach (var relative in user.Relatives)
                    result.Append($"{rowNumber},{relative.Name},{relative.Family},,,{relative.IdentityNumber},{GetYearOfDate(relative.BirthDay)},{GetMounthOfDate(relative.BirthDay)},{GetDayOfDate(relative.BirthDay)},{relative.NationalCode},,,{(byte)relative.Gender},{relative.FatherName},{(byte)InsuranceType.Secondary},{(byte)relative.RelativeType},,,{(byte)relative.TakafolKind},,,,,,,,{relative.InsertDateSh}" + Environment.NewLine);
            }

            return result.ToString();
        }

        public byte[] ExportExcel(UserSearchFilter filter)
        {
            #region Get Customers
            Expression<Func<User, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FullNameF))
                    conditions = conditions.And(x => x.Name.Contains(filter.FullNameF));
                if (!string.IsNullOrWhiteSpace(filter.EmailF))
                    conditions = x => x.Email.Contains(filter.EmailF);
                if (!string.IsNullOrWhiteSpace(filter.MobileNumberF))
                    conditions = x => x.MobileNumber.ToString().Contains(filter.MobileNumberF);

                if (!string.IsNullOrWhiteSpace(filter.DateFrom))
                {
                    var dateFrom = PersianDateTime.Parse(filter.DateFrom).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi >= dateFrom);
                }
                if (!string.IsNullOrWhiteSpace(filter.DateTo))
                {
                    var dateTo = PersianDateTime.Parse(filter.DateTo).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi < dateTo);
                }
            }

            var customers = _appUow.UserRepo.Get(
                conditions: conditions,
                orderBy: x => x.OrderBy(x => x.UserId),
                includeProperties: new List<Expression<Func<User, object>>>
                {
                    x => x.Addresses,
                    x => x.BankAccounts,
                    x => x.Relatives,
                });
            #endregion

            using (var workbook = new XLWorkbook())// ($"Customers-{PersianDateTime.Now.ToString(PersianDateTimeFormat.Date).Replace("/", "-")}.xlsx"))
            {
                #region Set Titles
                var worksheet = workbook.Worksheets.Add("Customers");
                worksheet.Cell(1, 1).SetValue("").Style.Font.SetBold(true);
                worksheet.Cell(1, 2).SetValue("Name").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 3).SetValue("LName").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 4).SetValue("PersonelCode").Style.Font.SetBold(true);
                worksheet.Cell(1, 5).SetValue("SodurPlace").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 6).SetValue("IdentityNo").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 7).SetValue("BirthYear").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 8).SetValue("BirthMonth").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 9).SetValue("BirthDay").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 10).SetValue("CodeMelli").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 11).SetValue("Address").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 12).SetValue("Tel").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 13).SetValue("Jens").Style.Font.SetBold(true);
                worksheet.Cell(1, 14).SetValue("FatherName").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 15).SetValue("BSKind").Style.Font.SetBold(true);
                worksheet.Cell(1, 16).SetValue("Nesbat").Style.Font.SetBold(true);
                worksheet.Cell(1, 17).SetValue("BeginDate").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 18).SetValue("EndDate").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 19).SetValue("TakafolKind").Style.Font.SetBold(true);
                worksheet.Cell(1, 20).SetValue("Tarh").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 21).SetValue("Workunit").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 22).SetValue("AccNOBimegarOneNo").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 23).SetValue("Bank").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 24).SetValue("ShebaAcc").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 25).SetValue("Mobile").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 26).SetValue("Email").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                worksheet.Cell(1, 27).SetValue("RegisterDate").SetDataType(XLDataType.Text).Style.Font.SetBold(true);
                #endregion

                int rowNumber = 1;
                int customerNumber = 0;
                foreach (var customer in customers)
                {
                    #region Set Customer Info
                    rowNumber++;
                    customerNumber++;
                    worksheet.Cell(rowNumber, 1).Value = customerNumber;
                    worksheet.Cell(rowNumber, 2).SetDataType(XLDataType.Text).SetValue(customer.Name);
                    worksheet.Cell(rowNumber, 3).SetDataType(XLDataType.Text).SetValue(customer.Family);
                    worksheet.Cell(rowNumber, 4).SetDataType(XLDataType.Text).SetValue(0);
                    worksheet.Cell(rowNumber, 5).SetDataType(XLDataType.Text).SetValue("");
                    worksheet.Cell(rowNumber, 6).SetDataType(XLDataType.Text).SetValue(customer.IdentityNumber);
                    worksheet.Cell(rowNumber, 7).SetDataType(XLDataType.Text).SetValue(GetYearOfDate(customer.BirthDay));
                    worksheet.Cell(rowNumber, 8).SetDataType(XLDataType.Text).SetValue(GetMounthOfDate(customer.BirthDay));
                    worksheet.Cell(rowNumber, 9).SetDataType(XLDataType.Text).SetValue(GetDayOfDate(customer.BirthDay));
                    worksheet.Cell(rowNumber, 10).SetDataType(XLDataType.Text).SetValue(customer.NationalCode);
                    worksheet.Cell(rowNumber, 11).SetDataType(XLDataType.Text).SetValue(customer.Addresses.FirstOrDefault()?.FullAddress);
                    worksheet.Cell(rowNumber, 12).SetDataType(XLDataType.Text).SetValue("");
                    worksheet.Cell(rowNumber, 13).SetDataType(XLDataType.Text).SetValue((byte)customer.Gender);
                    worksheet.Cell(rowNumber, 14).SetDataType(XLDataType.Text).SetValue(customer.FatherName);
                    worksheet.Cell(rowNumber, 15).SetDataType(XLDataType.Text).SetValue((byte)InsuranceType.Main);
                    worksheet.Cell(rowNumber, 16).SetDataType(XLDataType.Text).SetValue("");
                    worksheet.Cell(rowNumber, 17).SetDataType(XLDataType.Text).SetValue("");
                    worksheet.Cell(rowNumber, 18).SetDataType(XLDataType.Text).SetValue("");
                    worksheet.Cell(rowNumber, 19).SetDataType(XLDataType.Text).SetValue("");
                    worksheet.Cell(rowNumber, 20).SetDataType(XLDataType.Text).SetValue(customer.InsurancePlan);
                    worksheet.Cell(rowNumber, 21).SetDataType(XLDataType.Text).SetValue(customer.Organization);
                    worksheet.Cell(rowNumber, 22).SetDataType(XLDataType.Text).SetValue(customer.InsuranceNumber);
                    worksheet.Cell(rowNumber, 23).SetDataType(XLDataType.Text).SetValue(customer.BankAccounts.FirstOrDefault()?.BankName.GetDescription());
                    worksheet.Cell(rowNumber, 24).SetDataType(XLDataType.Text).SetValue(customer.BankAccounts.FirstOrDefault()?.Shaba);
                    worksheet.Cell(rowNumber, 25).SetDataType(XLDataType.Text).SetValue(customer.MobileNumber);
                    worksheet.Cell(rowNumber, 26).SetDataType(XLDataType.Text).SetValue(customer.Email);
                    worksheet.Cell(rowNumber, 27).SetDataType(XLDataType.Text).SetValue(customer.InsertDateSh);
                    #endregion

                    foreach (var relative in customer.Relatives)
                    {
                        #region Set Relative Info
                        rowNumber++;
                        worksheet.Cell(rowNumber, 1).Value = customerNumber;
                        worksheet.Cell(rowNumber, 2).SetDataType(XLDataType.Text).SetValue(relative.Name);
                        worksheet.Cell(rowNumber, 3).SetDataType(XLDataType.Text).SetValue(relative.Family);
                        worksheet.Cell(rowNumber, 4).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 5).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 6).SetDataType(XLDataType.Text).SetValue(relative.IdentityNumber);
                        worksheet.Cell(rowNumber, 7).SetDataType(XLDataType.Text).SetValue(GetYearOfDate(relative.BirthDay));
                        worksheet.Cell(rowNumber, 8).SetDataType(XLDataType.Text).SetValue(GetMounthOfDate(relative.BirthDay));
                        worksheet.Cell(rowNumber, 9).SetDataType(XLDataType.Text).SetValue(GetDayOfDate(relative.BirthDay));
                        worksheet.Cell(rowNumber, 10).SetDataType(XLDataType.Text).SetValue(relative.NationalCode);
                        worksheet.Cell(rowNumber, 11).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 12).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 13).SetDataType(XLDataType.Text).SetValue((byte)relative.Gender);
                        worksheet.Cell(rowNumber, 14).SetDataType(XLDataType.Text).SetValue(relative.FatherName);
                        worksheet.Cell(rowNumber, 15).SetDataType(XLDataType.Text).SetValue((byte)InsuranceType.Secondary);
                        worksheet.Cell(rowNumber, 16).SetDataType(XLDataType.Text).SetValue((byte)relative.RelativeType);
                        worksheet.Cell(rowNumber, 17).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 18).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 19).SetDataType(XLDataType.Text).SetValue((byte)relative.TakafolKind);
                        worksheet.Cell(rowNumber, 20).SetDataType(XLDataType.Text).SetValue(customer.InsurancePlan);
                        worksheet.Cell(rowNumber, 21).SetDataType(XLDataType.Text).SetValue(customer.Organization);
                        worksheet.Cell(rowNumber, 22).SetDataType(XLDataType.Text).SetValue(relative.InsuranceNumber);
                        worksheet.Cell(rowNumber, 23).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 24).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 25).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 26).SetDataType(XLDataType.Text).SetValue("");
                        worksheet.Cell(rowNumber, 27).SetDataType(XLDataType.Text).SetValue(relative.InsertDateSh);
                        #endregion
                    }
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.RightToLeft = true;
                    //worksheet.AdjustToContent();
                    workbook.SaveAs(stream);
                    var fileContent = stream.ToArray();
                    return fileContent;
                }
            }
        }

    }
}