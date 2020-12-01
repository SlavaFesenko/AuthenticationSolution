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

namespace ClaimsRoles.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IAuthorizationService _authorizationService;

        //public HomeController(IAuthorizationService authorizationService)
        //{
        //    _authorizationService = authorizationService;
        //}

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "ReqDoB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        public IActionResult Auth()
        {
            // claim - утверждение

            var grandmaClaims = new List<Claim> // утверждения бабушки о мне
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob"),
                new Claim(ClaimTypes.DateOfBirth, "11/11/2011"),
                new Claim(ClaimTypes.Role, "Admin"),
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

        //public async Task<IActionResult> InnerAuthInjectionExample(
        //    [FromServices] IAuthorizationService authorizationService) // [FromServices] вместо конструкторного DI 
        //{
        //    // doing some stuff here
        //    var builder = new AuthorizationPolicyBuilder();
        //    var customPolicy = builder.RequireClaim("SomeClaim");

        //    var authResult = await authorizationService.AuthorizeAsync(User, customPolicy);

        //    if (authResult.Succeed)
        //    {

        //    }
        //}
    }
}
