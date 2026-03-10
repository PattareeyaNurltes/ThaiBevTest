using ClassLib.Data.Entities;
using ClassLib.Entities;
using ClassLib.Model.Comm.Users;
using ClassLib.Models.Comm;
using Frontend.Models;
using System.Net;

namespace Frontend.Services;

public class UserProfileHandler
{
    private readonly ILogger<UserProfileHandler> _logger;
    private readonly RequestsHandler _requestsHandler;
    private readonly string secret_key = Environment.GetEnvironmentVariable("APP_SECRET_KEY")
        ?? throw new Exception("APP_SECRET_KEY not set.");
    private readonly string base_api = Environment.GetEnvironmentVariable("BASE_API")
        ?? throw new Exception("BASE_API not set.");

    public UserProfileHandler(ILogger<UserProfileHandler> logger, RequestsHandler requestsHandler)
    {
        _logger = logger;
        _requestsHandler = requestsHandler;
    }

    public async Task<AccountViewModel> GetAllAccout()
    {
        try
        {
            //Send Request
            string combined_url = $"{base_api}/api/UserManagement";

            var result = await _requestsHandler.GetAsync<List<Accounts>>(combined_url);
            var acc_vm = new AccountViewModel
            {
                accounts = result
            };

            return acc_vm;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProfileHandler : GetAllAccout : Error while creating new user");

            return new AccountViewModel ();
        }
    }

    public async Task<Object> GetProfileImage(Guid user_id)
    {
        try
        {
            //Send Request
            string combined_url = $"{base_api}/api/UserManagement/GetProfileImage/{user_id}";

            var result = await _requestsHandler.GetAsync<Object>(combined_url);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProfileHandler : GetProfileImage : Error while creating new user");

            return $"INTERNAL_SERVER_ERROR : {ex.Message}";
        }
    }
    public async Task<UserManagementResponse> GetProfileDetails(Guid user_id)
    {

        Guid requestID = Guid.NewGuid();

        try
        {
            //Send Request
            string combined_url = $"{base_api}/api/UserManagement/{user_id}";

            var result = await _requestsHandler.GetAsync<UserManagementResponse>(combined_url);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProfileHandler : GetProfileDetails : Error while creating new user");

            return new UserManagementResponse
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

    public async Task<UserManagementResponse> UpdateProfileDetails(Guid user_id, string updateBy, ProfileViewModel vm)
    {

        Guid requestID = Guid.NewGuid();

        try
        {

            //Send Request
            var payload = new UserManagementRequest
            {
                meta = new UserManagementRequestMeta
                {
                    RequestID = requestID,
                    API_KEY = secret_key
                },
                data = new UserManagementRequestData
                {
                    UserID = user_id,
                    Firstname = vm.Firstname,
                    Lastname = vm.Lastname,
                    Email = vm.Email,
                    Phone_no = vm.Phone_No,
                    Profile_image = vm.Profile_Image,
                    DateOfBirth = vm.DateOfBirth,
                    Occupation = vm.Occupation,
                    Gender = vm.Gender,
                    UpdateBy = updateBy
                }
            };
            string combined_url = $"{base_api}/api/UserManagement/update";

            var result = await _requestsHandler.PutAsync<UserManagementRequest, UserManagementResponse>
                    (combined_url, payload);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProfileHandler : UpdateProfileDetails : Error while creating new user");

            return new UserManagementResponse
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

