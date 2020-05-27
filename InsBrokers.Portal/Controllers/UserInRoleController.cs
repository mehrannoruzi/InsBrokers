using System;
using Elk.Core;
using System.Linq;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using Microsoft.AspNetCore.Authorization;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{

    //[AuthorizationFilter]
    public partial class UserInRoleController : Controller
    {
        private readonly IUserInRoleService _userInRoleSrv;

        public UserInRoleController(IUserInRoleService userInRoleBusiness)
        {
            _userInRoleSrv = userInRoleBusiness;
        }


        [HttpGet, AllowAnonymous]
        public virtual async Task<JsonResult> Add(int id)
            => Json(new Modal
            {
                AutoSubmit = false,
                Title = $"{Strings.Add} {DomainString.User}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", new UserInRole { RoleId = id }),
                AutoSubmitUrl = Url.Action("Add", "UserInRole"),

            });

        [HttpPost, AllowAnonymous]
        public virtual async Task<JsonResult> Add(UserInRole model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var addRep = await _userInRoleSrv.Add(model);

            if (!addRep.IsSuccessful) return Json(addRep);
            var getRep = _userInRoleSrv.Get(model.UserId).ToList();
            getRep.ForEach((x) =>
            {
                x.Role.UserInRoles = null;
            });

            return Json(new Response<string>
            {
                IsSuccessful = true,
                Result = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_List", getRep)
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _userInRoleSrv.Delete(id));

        [HttpGet, AllowAnonymous]
        public virtual PartialViewResult Get(Guid userId) => PartialView("Partials/_List", _userInRoleSrv.Get(userId));
    }
}