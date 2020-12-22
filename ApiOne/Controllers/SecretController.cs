using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    public class SecretController : Controller
    {
        [Route("")]
        [HttpGet]
        public string Index()
        {
            return "Index of ApiOne (not protected)";
        }

        [Route("/secret")]
        [Authorize]
        [HttpGet]
        public string Secret()
        {
            return "This is the secret message from AppOne.";
        }
    }
}
