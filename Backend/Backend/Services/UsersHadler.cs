using Azure;
using ClassLib.Data.Entities;
using ClassLib.Entities;
using ClassLib.Model.Comm.Users;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class UsersHadler : BaseHadler
    {
        private readonly PattareeyaDbContext _context;

        public UsersHadler(PattareeyaDbContext context)
        {
            _context = context;
        }

        #region Methods

        //GET
        public async Task<List<Accounts>> GetUsers()
        {

            var users = await _context.Accounts.Where(w => w.is_active == true).ToListAsync();
            return users;
        }

        //GET BY ID
        public async Task<UserManagementResponse> GetUsers(Guid user_id)
        {

            var ExistingAccount = await _context.Accounts
                                               .Where(w => w.user_id == user_id && w.is_active == true)
                                               .FirstOrDefaultAsync();

            AccountProfile? AccountProf = null;
            if (ExistingAccount != null)
            {
                AccountProf = await _context.AccountProfile
                                                .Where(w => w.user_id == user_id).FirstOrDefaultAsync();
            }

            var resp = new UserManagementResponse
            {
                IsSuccess = true,
                Data = ExistingAccount,
                ErrorCode = "",
                ErrorMessage = "",
                Message = "",
                StatusCode = HttpStatusCode.OK,
                Timestamp = DateTime.Now,
                ValidationErrors = null,
                Profile = AccountProf
            };
            return resp;
        }

        //GET Profile Image
        public async Task<AccountProfile?> GetUserProfile(Guid user_id)
        {
            AccountProfile? profile = await _context.AccountProfile
                    .FirstOrDefaultAsync(x => x.user_id == user_id);

            return profile;
        }
        //Update
        public async Task<UserManagementResponse> UpdateUser(UserManagementRequest req)
        {
            try
            {
                var reqData = req.data;

                // 1) ตรวจสอบ Account
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.user_id == reqData.UserID && a.is_active);

                if (account == null)
                {
                    return ErrorResponse(req, $"User ID {reqData.UserID} not found.");
                }

                bool accountChanged = false;

                // 2) Update Account
                if (!string.IsNullOrWhiteSpace(reqData.Password))
                {
                    var (hashed, salt) = await HashPassword(reqData.Password);
                    account.password = hashed;
                    accountChanged = true;
                }

                if (!string.IsNullOrWhiteSpace(reqData.Firstname))
                {
                    account.firstname = reqData.Firstname;
                    accountChanged = true;
                }

                if (!string.IsNullOrWhiteSpace(reqData.Lastname))
                {
                    account.lastname = reqData.Lastname;
                    accountChanged = true;
                }

                if (!string.IsNullOrWhiteSpace(reqData.Address))
                {
                    account.address = reqData.Address;
                    accountChanged = true;
                }

                if (reqData.DateOfBirth.HasValue && reqData.DateOfBirth.Value != DateTime.MinValue)
                {
                    account.date_of_birth = reqData.DateOfBirth.Value;
                    accountChanged = true;
                }

                // 3) Update Profile
                var profile = await _context.AccountProfile
                    .FirstOrDefaultAsync(p => p.user_id == reqData.UserID);

                bool profileChanged = false;

                if (profile == null)
                {
                    profile = new AccountProfile
                    {
                        user_id = reqData.UserID,
                        email = reqData.Email,
                        phone_no = reqData.Phone_no,
                        profile_image = reqData.Profile_image,
                        occupation = reqData.Occupation,
                        gender = reqData.Gender,
                        create_by = reqData.UpdateBy ?? "SYSTEM",
                        create_date = DateTime.Now
                    };

                    await _context.AccountProfile.AddAsync(profile);
                    profileChanged = true;
                }
                else
                {
                    profileChanged |= UpdateIfNotEmpty(reqData.Email, v => profile.email = v);
                    profileChanged |= UpdateIfNotEmpty(reqData.Phone_no, v => profile.phone_no = v);
                    profileChanged |= UpdateIfNotEmpty(reqData.Profile_image, v => profile.profile_image = v);
                    profileChanged |= UpdateIfNotEmpty(reqData.Occupation, v => profile.occupation = v);
                    profileChanged |= UpdateIfNotEmpty(reqData.Gender, v => profile.gender = v);

                    if (profileChanged)
                    {
                        profile.update_by = reqData.UpdateBy;
                        profile.update_date = DateTime.Now;
                    }
                }

                // 4) Save changes
                if (accountChanged)
                {
                    account.update_by = reqData.UpdateBy;
                    account.update_date = DateTime.Now;
                }

                if (accountChanged || profileChanged)
                {
                    await _context.SaveChangesAsync();
                }

                // 5) Return success
                return new UserManagementResponse
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = true,
                    Data = account,
                    Profile = profile,
                    Message = "User was updated successfully",
                    StatusCode = HttpStatusCode.OK,
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new UserManagementResponse
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = false,
                    ErrorCode = "UPDATE_FAILED",
                    ErrorMessage = ex.Message,
                    Message = "User update failed.",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Timestamp = DateTime.Now
                };
            }
        }


        //Delete
        public async Task<UserManagementResponse> DeleteUser(UserManagementRequest req)
        {
            try
            {
                var req_data = req.data;
                var ExistingAccount = await _context.Accounts
                                                .Where(w => w.user_id == req_data.UserID)
                                                .FirstOrDefaultAsync();

                if (ExistingAccount == null)
                {
                    return new UserManagementResponse()
                    {
                        RequestID = req.meta.RequestID,
                        IsSuccess = false,
                        Data = null,
                        ErrorCode = "",
                        ErrorMessage = $"User ID {req_data.UserID} not found.",
                        Message = "",
                        StatusCode = HttpStatusCode.InternalServerError,
                        Timestamp = DateTime.Now,
                        ValidationErrors = null
                    };
                }

                ExistingAccount.is_active = false;
                ExistingAccount.update_by = req_data.UpdateBy;
                ExistingAccount.update_date = DateTime.Now;

                await _context.SaveChangesAsync();


                return new UserManagementResponse
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = true,
                    Data = ExistingAccount,
                    Message = "User was deleted successfully",
                    StatusCode = HttpStatusCode.OK,
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {

                return new UserManagementResponse
                {
                    RequestID = req.meta.RequestID,
                    IsSuccess = false,
                    ErrorCode = "UPDATE_FAILED",
                    ErrorMessage = ex.Message,
                    Data = null,
                    Message = "User updated failed.",
                    StatusCode = HttpStatusCode.OK,
                    Timestamp = DateTime.Now
                };
            }

        }

        private bool UpdateIfNotEmpty(string? value, Action<string> updateAction)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                updateAction(value);
                return true;
            }
            return false;
        }

        private UserManagementResponse ErrorResponse(UserManagementRequest req, string message)
        {
            return new UserManagementResponse
            {
                RequestID = req.meta.RequestID,
                IsSuccess = false,
                ErrorCode = "NOT_FOUND",
                ErrorMessage = message,
                Message = "",
                StatusCode = HttpStatusCode.NotFound,
                Timestamp = DateTime.Now
            };
        }
        #endregion
    }
}