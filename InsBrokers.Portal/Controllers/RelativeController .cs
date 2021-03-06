using Elk.Core;
using Elk.Http;
using System.Linq;
using Elk.AspNetCore;
using InsBrokers.Domain;
using InsBrokers.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InsBrokers.Portal.Resource;
using DomainString = InsBrokers.Domain.Resource.Strings;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace InsBrokers.Portal.Controllers
{
    [AuthorizationFilter]
    public partial class RelativeController : Controller
    {
        private readonly IRelativeService _relativeSrv;
        private readonly IConfiguration _configuration;

        public RelativeController(IRelativeService MemberRelativeSrv, IConfiguration configuration)
        {
            _relativeSrv = MemberRelativeSrv;
            _configuration = configuration;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Relatives}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Relative()),
                AutoSubmit = false
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Relative model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            if (model.RelativeAttachmentIds.Count() <= 0) return Json(new Response<string> { Message = Strings.MustUploadAttachments });

            return Json(await _relativeSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _relativeSrv.FindWithAttachmentsAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Relatives) });
            if (findRep.Result.RelativeAttachments != null)
                foreach (var item in findRep.Result.RelativeAttachments)
                    item.Url = $"{_configuration["CustomSettings:MainAddress"]}{item.Url}";

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Relatives}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false,
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Relative model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _relativeSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _relativeSrv.DeleteAsync(id));

        [HttpPost, AllowAnonymous]
        public virtual async Task<JsonResult> DeleteAttachment(int attachmentId) => Json(await _relativeSrv.DeleteAttachment(attachmentId));

        [HttpGet]
        public virtual ActionResult Manage(RelativeSearchFilter filter)
        {
            ViewBag.ExcelExport = true;
            if (!Request.IsAjaxRequest()) return View(_relativeSrv.Get(filter));
            else return PartialView("Partials/_List", _relativeSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("Relative", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_relativeSrv.Search(q, User.GetUserId()).ToSelectListItems());

        [HttpGet, AuthEqualTo("Relative", "Manage")]
        public virtual JsonResult Excel(RelativeSearchFilter filter)
            => Json(_relativeSrv.Export(filter));
    }
}