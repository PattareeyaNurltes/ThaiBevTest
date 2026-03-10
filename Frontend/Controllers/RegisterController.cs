using ClassLib.Data.Entities;
using ClassLib.Models.Comm;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{

    public class RegisterController : Controller
    {

        #region Property
        private readonly ILogger<RegisterController> _logger;
        private readonly RegisterHandler _registerHandler;
        #endregion


        #region Constructor
        public RegisterController(ILogger<RegisterController> logger, RegisterHandler regis) { 
            _logger = logger;
            _registerHandler =  regis; 
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        // POST: Register
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", vm);
            }

            RegistrationResponse resp = await _registerHandler.SendToRegisterNewUser(vm);
            if (resp.IsSuccess)
            {
                // Redirect ไปหน้า Login
                return RedirectToAction("Index", "Login");
            }

            ViewBag.RegisterError = "Error occurred while registering new account."; 
            return View(resp);
        }
    }
}
