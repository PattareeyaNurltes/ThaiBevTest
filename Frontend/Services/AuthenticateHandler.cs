using ClassLib.Data.Entities;
using ClassLib.Model.Comm.Authentication;
using ClassLib.Model.Comm.Users;
using ClassLib.Models.Comm;
using Frontend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Reflection;
using System.Security.Claims;

namespace Frontend.Services
{

    public class AuthenticateHandler
    {
        private readonly ILogger<AuthenticateHandler> _logger;
        private readonly RequestsHandler _requestsHandler;
        private readonly string secret_key = Environment.GetEnvironmentVariable("APP_SECRET_KEY")
            ?? throw new Exception("APP_SECRET_KEY not set.");
        private readonly string base_api = Environment.GetEnvironmentVariable("BASE_API")
            ?? throw new Exception("BASE_API not set.");
        public AuthenticateHandler(ILogger<AuthenticateHandler> logger, RequestsHandler requestsHandler)
        {
            _logger = logger;
            _requestsHandler = requestsHandler;
        }

        public async Task<LoginResponse?> Authenticate(LoginViewModel vm)
        {
            var requestID = Guid.NewGuid();
            try
            {
                var payload = new LoginRequest
                {
                    meta = new LoginRequestMeta
                    {
                        RequestID = requestID,
                        API_KEY = secret_key
                    },
                    data = new LoginRequestData
                    {
                        Username = vm.username,
                        Password = vm.password
                    }
                };
                string combined_url = $"{base_api}/api/Authentication";

                var result = await _requestsHandler.PostAsync<LoginRequest, LoginResponse>
                    (combined_url, payload);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuthenticateHandler : Login : Error while logging in.");

                return new LoginResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Data = null,
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Message = ex.Message,
                    RequestID = requestID,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Timestamp = DateTime.Now,
                    ValidationErrors = null
                };
            }


        }

        public async Task<UserManagementResponse> GetUserProfile(Guid user_id) 
        { 
            //Send Request
            string combined_url = $"{base_api}/api/UserManagement/{user_id}";

            var result = await _requestsHandler.GetAsync<UserManagementResponse>(combined_url);

            return result;
        }
    }
}