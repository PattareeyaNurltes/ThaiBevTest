using Backend.Services;
using ClassLib.Data.Entities;
using ClassLib.Models.Comm;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        #region Property
        private readonly ILogger<RegisterController> _logger;
        private readonly RegisterHandler _registerHandler;
        #endregion


        #region Constructor
        public RegisterController(ILogger<RegisterController> logger, RegisterHandler regis)
        {
            _logger = logger;
            _registerHandler = regis;
        }
        #endregion

        #region Methods
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest req)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(new Response<object>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "VALIDATION_ERROR",
                    ValidationErrors = validationErrors,
                    Message = "Validation failed",
                    RequestID = req.meta.RequestID
                });
            }

            var secret = Request.Headers["X-PattareeyaApp-Secret"].ToString() ?? req.meta.API_KEY.ToString();
            var expected = Environment.GetEnvironmentVariable("APP_SECRET_KEY");

            if (secret != expected)
            {
                return Unauthorized(new Response<Accounts>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid registration secret",
                    ErrorCode = "UNAUTHORIZED"
                });
            }


            var result = await _registerHandler.AddNewUser(req);
            return Ok(result);
        }
        #endregion
    }
}
