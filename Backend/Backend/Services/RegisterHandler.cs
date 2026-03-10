using ClassLib.Data.Entities;
using ClassLib.Models.Comm;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Services
{
    public class RegisterHandler : BaseHadler
    {
        private readonly ILogger<RegisterHandler> _logger;
        private readonly PattareeyaDbContext _context;
        

        public RegisterHandler(ILogger<RegisterHandler> logger, PattareeyaDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Response<Accounts>> AddNewUser(RegistrationRequest req)
        {
            try
            {
                var req_data = req.data;
                var (hashed_passwd, _passwd_salt) = await HashPassword(req_data.Password);
                var account = new Accounts
                {
                    user_id = Guid.NewGuid(),
                    username = req_data.UserName,
                    password = hashed_passwd,
                    password_salt = _passwd_salt,
                    firstname = req_data.FirstName,
                    lastname = req_data.LastName,
                    date_of_birth = req_data.DateOfBirth,
                    address = req_data.Address,
                    create_date = DateTime.Now,
                    create_by = "system",

                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return new Response<Accounts>
                {
                    Data = account,
                    ErrorCode = string.Empty,
                    ErrorMessage = string.Empty,
                    IsSuccess = true,
                    Message = "User created successfully",
                    RequestID = req.meta.RequestID,
                    StatusCode = HttpStatusCode.OK,
                    Timestamp = DateTime.Now,
                    ValidationErrors = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new user");

                return new Response<Accounts>
                {
                    Data = null,
                    ErrorCode = "CREATE_FAILED",
                    ErrorMessage = ex.Message,
                    IsSuccess = false,
                    Message = "Error while creating new user",
                    RequestID = req.meta.RequestID,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Timestamp = DateTime.Now,
                    ValidationErrors = null
                };
            }
        }

    }
}
