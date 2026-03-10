using ClassLib.Data.Entities;
using ClassLib.Enum;
using ClassLib.Model.Comm.Authentication;
using ClassLib.Model.Comm.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Services
{
    public class TokenHandler : BaseHadler
    {
        private readonly ILogger<TokenHandler> _logger;
        private readonly IConfiguration _config;
        private readonly PattareeyaDbContext _context;
        public TokenHandler(ILogger<TokenHandler> logger, IConfiguration config, PattareeyaDbContext dbContext)
        {
            _config = config;
            _logger = logger;
            _context = dbContext;
        }

        public async Task<LoginResponse> Login(LoginRequest req)
        {
            try
            {
                var req_data = req.data;
                var ExistingAccount = await _context.Accounts
                                                .Where(w => w.username == req_data.Username)
                                                .FirstOrDefaultAsync();
                if (ExistingAccount == null)
                {
                    return new LoginResponse()
                    {
                        RequestID = req.meta.RequestID,
                        IsSuccess = false,
                        Data = null,
                        ErrorCode = "",
                        ErrorMessage = $"User not found.",
                        Message = "",
                        StatusCode = HttpStatusCode.NotFound,
                        Timestamp = DateTime.Now
                    };
                }


                #region ComparePassword
                var (hashed_raw_pwd, salt) = await HashPassword(req_data.Password);
                var existing_pwd = ExistingAccount.password;
                if (existing_pwd != hashed_raw_pwd)
                {
                    return new LoginResponse()
                    {
                        RequestID = req.meta.RequestID,
                        IsSuccess = false,
                        Data = null,
                        ErrorCode = "UNAUTHORIZED",
                        ErrorMessage = "Incorrect username or password.",
                        Message = "",
                        StatusCode = HttpStatusCode.Unauthorized,
                        Timestamp = DateTime.Now,
                        ValidationErrors = null
                    };
                }
                #endregion

                #region Generate AccessToken
                string acc_token = GenerateAccessToken(ExistingAccount);
                string ref_token = GenerateRefreshToken();
                #endregion

                #region Update Token Info
                ExistingAccount.access_token = acc_token;
                ExistingAccount.refresh_token = ref_token;
                _context.Accounts.Update(ExistingAccount);
                #endregion

                #region Update Login Info
                var login_info = new LoginInfomations()
                {
                    last_login_date = DateTime.Now,
                    login_status = LoginStatuses.OK.ToString(),
                    user_id = ExistingAccount.user_id,
                    Account = ExistingAccount
                };
                await _context.AddAsync(login_info);
                #endregion

                return new LoginResponse()
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = true,
                    TokenDetails = new TokenDetails()
                    {
                        AccessToken = acc_token,
                        RefreshToken = ref_token
                    },
                    UserData = new LoginDataDto ()
                    {
                        UserID = ExistingAccount.user_id,
                        Username = ExistingAccount.username,
                        Firstname = ExistingAccount.firstname,
                        Lastname = ExistingAccount.lastname
                    },
                    ErrorCode = string.Empty,
                    ErrorMessage = string.Empty,
                    Message = "Login Successfully",
                    StatusCode = HttpStatusCode.OK,
                    Timestamp = DateTime.Now,
                    //TokenDetails = token_details
                };
            }
            catch (Exception ex)
            {

                return new LoginResponse()
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = false,
                    Data = null,
                    ErrorCode = "LOGIN_FAILED",
                    ErrorMessage = "Incorrect username or password.",
                    Message = "",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Timestamp = DateTime.Now,
                    ValidationErrors = null
                };
            }
        }

        public string GenerateAccessToken(Accounts user)
        {
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub,
            user.user_id.ToString()),
            new Claim(ClaimTypes.Role,"User") };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}