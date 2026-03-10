using Backend.Services;
using ClassLib.Data.Entities;
using ClassLib.Model.Comm.Authentication;
using ClassLib.Models.Comm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        #region Property
        private readonly ILogger<AuthenticationController> _logger;
        private readonly TokenHandler _tokenHandler;
        #endregion


        #region Constructor
        public AuthenticationController(ILogger<AuthenticationController> logger, TokenHandler token)
        {
            _logger = logger;
            _tokenHandler = token;
        }
        #endregion

        #region Methods
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authentication([FromBody] LoginRequest req)
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


            var result = await _tokenHandler.Login(req);
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> RequestToken([FromBody] LoginRequest req)
        #endregion
    }
}
