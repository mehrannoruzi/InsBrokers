using Elk.Core;
using Elk.Http;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using DomainString = InsBrokers.Domain.Resource.Strings;

namespace InsBrokers.Portal.Controllers
{
    //[AuthorizationFilter]
    public partial class RoleController : Controller
    {
        private readonly IRoleService _roleSrv;

        public RoleController(IRoleService roleSrv)
        {
            _roleSrv = roleSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Role}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Role { Enabled = true }),
                AutoSubmitUrl = Url.Action("Add", "Role")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Role model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _roleSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _roleSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Role) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Role}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "Role"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Role model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _roleSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _roleSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(RoleSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_roleSrv.Get(filter));
            else return PartialView("Partials/_List", _roleSrv.Get(filter));
        }

    }
}