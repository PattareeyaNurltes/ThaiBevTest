using ClassLib.Model.Comm.Users;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Frontend.Controllers
{
    public class LoginController : Controller
    {

        #region Property
        private readonly ILogger<LoginController> _logger;
        private readonly AuthenticateHandler _auth;
        #endregion


        #region Constructor
        public LoginController(ILogger<LoginController> logger, AuthenticateHandler auth)
        {
            _logger = logger;
            _auth = auth;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel req)
        {
            var result = await _auth.Authenticate(req);
            var user = result?.UserData;

            UserManagementResponse user_profile = await _auth.GetUserProfile(user.UserID);

            //Create Profile Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString() ?? ""),
                new Claim("user_id", user?.UserID.ToString() ?? ""),
                new Claim("FullName", $"{user?.Firstname} {user?.Lastname}"),
                new Claim("FirstName", user?.Firstname ?? ""),
                new Claim("ProfileImage", "")
            };

            ViewData["UserID"] = user?.UserID.ToString();



            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            //Store Token in web cookies
            Response.Cookies.Append(".AccessToken", result.TokenDetails.AccessToken ?? "", new CookieOptions
            {
                HttpOnly = true,       // ป้องกัน JS อ่าน cookie
                Secure = false,         // If true = ใช้เฉพาะ HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
