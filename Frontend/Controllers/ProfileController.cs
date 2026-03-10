using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Frontend.Controllers
{
    public class ProfileController : Controller
    {
        #region Property
        private readonly ILogger<ProfileController> _logger;
        private readonly UserProfileHandler _profile;
        #endregion


        #region Constructor
        public ProfileController(ILogger<ProfileController> logger, UserProfileHandler profile)
        {
            _logger = logger;
            _profile = profile;
        }
        #endregion

        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirst("user_id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Guid user_id = Guid.Parse(userId);

            var resp = await _profile.GetProfileDetails(user_id);
            var user = resp.Data;
            var user_profile = resp.Profile;


            var model = new ProfileViewModel
            {
                Firstname = user?.firstname ?? "",
                Lastname = user?.lastname ?? "",
                Email = user_profile?.email ?? "",
                Phone_No = user_profile?.phone_no ?? "",
                Profile_Image = user_profile?.profile_image ?? "",   // base64 หรือ path ก็ได้
                DateOfBirth = user.date_of_birth,
                Occupation = user_profile?.occupation ?? "",
                Gender = user_profile?.gender ?? ""
            };
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> GetProfileImage(Guid id)
        {
            var resp = await _profile.GetProfileImage(id);

            return Ok(resp);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile([FromBody] ProfileViewModel model)
        {
            if (model == null)
                return BadRequest(new { message = "Invalid data" });

            var userId = User.FindFirst("user_id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid user_id = Guid.Parse(userId);
            var updateBy = User.FindFirst("FirstName")?.Value ?? "SYSTEM";
            
            var resp = await _profile.UpdateProfileDetails(user_id, updateBy, model);

            return Ok(resp);
        }

    }
}
