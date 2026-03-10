using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserProfileHandler _userProfileHandler;

        public AccountsController(UserProfileHandler userProfileHandler) { 
            _userProfileHandler = userProfileHandler;
        }
        public async Task<IActionResult> Index()
        {
            AccountViewModel model = await _userProfileHandler.GetAllAccout();

            return View(model);
        }
    }
}
