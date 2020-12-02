using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServerOAuth.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(
            string response_type, // authorization flow type
            string client_id,
            string redirect_uri,
            string scope, // what info I want = email, phone number etc.
            string state) // random string generated, allows to check that we're going back to the right client
        {
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri); // ?redirectUri=someValue
            query.Add("state", state);              // ?redirectUri=someValue&state=someState

            return View(model: query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirectUri,
            string state)
        {
            const string code = "ABABABABABABA";

            var query = new QueryBuilder
            {
                { "code", code },
                { "state", state }
            };

            var redirectFullUri = $"{redirectUri}{query}";

            return Redirect(redirectFullUri);
        }

        [HttpGet]
        public async Task<IActionResult> Token(
            string grant_type, // flow of access token request
            string code, // confirmation of the authentication process
            string redirect_uri,
            string client_id)
        {
            // some mechanism for validating the code (like to grab from db, compare etc.)
            var claims = new[]
               {
                    new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                    new Claim("any_other_claim", "claim_xxx"),
                };

            var bytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(bytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var credentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                credentials);

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                row_claim = "authTutorial"
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }
    }
}
