
using ClassLib.Data.Entities;
using ClassLib.Model.Comm.Authentication;
using ClassLib.Models.Comm;
using Frontend.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class RegisterHandler
    {
        private readonly ILogger<RegisterHandler> _logger;
        private readonly RequestsHandler _requestsHandler;
        private readonly string secret_key =Environment.GetEnvironmentVariable("APP_SECRET_KEY")
            ?? throw new Exception("APP_SECRET_KEY not set.");
        private readonly string base_api =Environment.GetEnvironmentVariable("BASE_API")
            ?? throw new Exception("BASE_API not set.");

        public RegisterHandler(ILogger<RegisterHandler> logger, RequestsHandler requestsHandler)
        {
            _logger = logger;
            _requestsHandler = requestsHandler;
        }

        public async Task<RegistrationResponse> SendToRegisterNewUser(RegisterViewModel vm)
        {
            Guid requestID = Guid.NewGuid();

            try
            {
                //Send Request
                var payload = new RegistrationRequest () { 
                    meta = new RegistrationRequestMeta 
                    {
                         RequestID = requestID,
                         API_KEY = secret_key.ToString()
                    },
                    data = new RegistrationRequestData
                    {
                        Address = vm.Address,
                        DateOfBirth = vm.DateOfBirth,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName
                    }
                };

                string combined_url = $"{base_api}/api/register";

                var result = await _requestsHandler.PostAsync<RegistrationRequest, RegistrationResponse>
                    (combined_url,payload);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegisterHandler : AddNewUser : Error while creating new user");

                return new RegistrationResponse
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
    }
}
