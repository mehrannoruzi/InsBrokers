using System;
using Elk.Core;
using InsBrokers.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InsBrokers.Service
{
    public interface IUserService : IScopedInjection
    {
        Task<IResponse<User>> AddAsync(User model);
        Task<IResponse<User>> UpdateAsync(User model);
        Task<IResponse<User>> UpdateProfile(User model);
        Task<IResponse<bool>> DeleteAsync(Guid userId);
        Task<IResponse<User>> FindAsync(Guid userId);
        Task<IResponse<User>> FindByMobileNumber(long mobileNumber);
        MenuModel GetAvailableActions(Guid userId, List<MenuSPModel> spResult = null, string urlPrefix = "");
        List<MenuSPModel> GetMainMenu(Guid userId);
        Task<IResponse<User>> Authenticate(long mobileNumber, string password);
        void SignOut(Guid userId);
        PagingListDetails<User> Get(UserSearchFilter filter);
        IDictionary<object, object> Search(string query, int take = 10);
        Task<IResponse<string>> RecoverPassword(long mobileNumber, string from, EmailMessage model);
        Task<IResponse<object>> AddUserAttachments(IFormFile file, UserAttachmentType type);
        Task<IResponse<User>> SignUp(PortalSignUpModel model);
        Task<IResponse<int>> GetUserCount();
        Task<IResponse<Dictionary<string, int>>> GetUserCountLastDaysAsync(int dayCount = 10);
        string Export(UserSearchFilter filter);
        byte[] ExportExcel(UserSearchFilter filter);
    }
}