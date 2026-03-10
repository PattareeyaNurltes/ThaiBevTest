using Backend.Services;
using ClassLib.Model.Comm.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        #region Property
        private readonly ILogger<UserManagementController> _logger;
        private readonly UsersHadler _usersHandler;
        #endregion

        #region Construtor
        public UserManagementController(ILogger<UserManagementController> logger, UsersHadler usersHandler)
        {
            _logger = logger;
            _usersHandler = usersHandler;
        }
        #endregion

        // GET: api/UserManagement
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _usersHandler.GetUsers();
            return Ok(users);
        }

        // GET: api/UserManagement/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _usersHandler.GetUsers(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        //GET: api/UserManagement/GetProfileImage/{id}
        [HttpGet("GetProfileImage/{id}")]
        public async Task<IActionResult> GetProfileImage(Guid id)
        {
            var profile = await _usersHandler.GetUserProfile(id);

            if (profile == null || string.IsNullOrEmpty(profile.profile_image))
                return NotFound();

            return Ok(new { profile_image = profile.profile_image });
        }


        // PUT: api/UserManagement/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserManagementRequest req)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                   .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new UserManagementResponse
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = false,
                    ErrorCode = "BAD_REQUEST",
                    ErrorMessage = "Request data incorrect.",
                    Data = null,
                    Message = "",
                    StatusCode = HttpStatusCode.BadRequest,
                    Timestamp = DateTime.UtcNow,
                    ValidationErrors = validationErrors
                });
            }

            var secret = Request.Headers["X-PattareeyaApp-Secret"].ToString() ?? req.meta.API_KEY.ToString();
            var expected = Environment.GetEnvironmentVariable("APP_SECRET_KEY");

            if (secret != expected)
            {
                return Unauthorized(new UserManagementResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid the api secret key",
                    ErrorCode = "UNAUTHORIZED"
                });
            }

            var result = await _usersHandler.UpdateUser(req);
            return Ok(result);
        }

        // DELETE: api/UserManagement/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] UserManagementRequest req)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                   .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new UserManagementResponse
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = false,
                    ErrorCode = "BAD_REQUEST",
                    ErrorMessage = "Request data incorrect.",
                    Data = null,
                    Message = "",
                    StatusCode = HttpStatusCode.BadRequest,
                    Timestamp = DateTime.UtcNow,
                    ValidationErrors = validationErrors
                });
            }

            var secret = Request.Headers["X-PattareeyaApp-Secret"].ToString() ?? req.meta.API_KEY.ToString();
            var expected = Environment.GetEnvironmentVariable("APP_SECRET_KEY");

            if (secret != expected)
            {
                return Unauthorized(new UserManagementResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid the api secret key",
                    ErrorCode = "UNAUTHORIZED"
                });
            }

            var result = await _usersHandler.DeleteUser(req);
            return Ok(result);
        }
    }
}
