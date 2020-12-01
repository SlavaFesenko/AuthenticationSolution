using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthBasics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Auth()
        {
            // claim - утверждение

            var grandmaClaims = new List<Claim> // утверждения бабушки о мне
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob"),
                new Claim("Grandma.Says", "Bob is a good boy"),
            };
#warning - authenticationType должен быть обязательно указан!!!
            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma"); // моя идентичность соглассно утверждениям бабушки

            var licenceClaims = new List<Claim> // утверждения департамента водительских прав о мне
            {
                new Claim(ClaimTypes.Name, "Bob D Foo"),
                new Claim("Licensse", "A+"),
            };

            var licenseIdentity = new ClaimsIdentity(licenceClaims, "license"); // моя идентичность соглассно утверждениям бабушки

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });
            //userPrincipal.RequireAuthenticatedSignIn = true;

            var result = HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
