using System;
using InsBrokers.Domain;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                new Claim(ClaimTypes.Email, string.IsNullOrWhiteSpace(user.Email) ? string.Empty : user.Email),
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