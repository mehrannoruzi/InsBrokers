using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InsBrokers.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsBrokers.Portal.Controllers
{
    public class BaseAuthController : Controller
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public BaseAuthController(IHttpContextAccessor httpAccessor)
        {
            _httpAccessor = httpAccessor;
        }
        protected async Task CreateCookie(User user, bool remeberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.MobileNumber.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Fullname", user.Fullname)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = remeberMe,
            };
            await _httpAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}