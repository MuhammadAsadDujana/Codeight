using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using DAL.LogRepository;
using DAL.UserRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.UserServices
{
    public class UserService
    {
        private UserRepository _userRepository;
        private LogRepository _logRepository;
        public UserService()
        {
            _userRepository = new UserRepository();
            _logRepository = new LogRepository();
        }

        //For Apis
        public async Task<ResponseModel> InsertUserService(tbl_Users user)
        {
            try
            {
                var response = await _userRepository.InsertUserRepo(user);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", ConstantMessages.RegistrationSuccess, "200", user, null);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.RegistrationFailed, "408", user, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                //_logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "InsertUserService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = string.Format("{0},{1}", user.FirstName, user.LastName) });
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "InsertUserService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> LoginService(string email, string password, UserType userType, string fCMDeviceToken)
        {
            try
            {
                var users = await _userRepository.getAllUsersRepo();

                if (users.Count() > 0)
                {
                    
                    var IsvalidEmail = users.Where(x => x.Email == email && x.UserType == (int)userType).FirstOrDefault();
                    if (IsvalidEmail == null)
                    {
                        return ResponseHandler.GetResponse("Failed", "Email address doesn't exists", "404", null, "");
                    }

                    var user = users.Where(x => x.Email == email && x.Password == Encryption.EncodePasswordToBase64(password) && x.UserType == (int)userType).FirstOrDefault();
                    if (user == null)
                    {
                        return ResponseHandler.GetResponse("Failed", "Unable to login. Either username or password is incorrect", "404", null, "");
                    }

                    var userStatus = users.Where(x => x.Email == email && x.Password == Encryption.EncodePasswordToBase64(password)).FirstOrDefault();
                    if (userStatus.UserStatus != (int) UserStatus.Verified)
                    {
                        return ResponseHandler.GetResponse("Failed", "Unable to login. Your status is unverified please contact admin", "404", null, "");
                    }

                    
                    user.AccessToken = TokkenManager.GenerateToken(user.UserId.ToString());
                    user.TokenIssueDate = DateTime.Now;
                    user.TokenExpiryDate = DateTime.Now.AddDays(7);
                    user.FCMDeviceToken = fCMDeviceToken;

                    await _userRepository.UpdateUserRepo(user);
                    return ResponseHandler.GetResponse("Success", "Login", "200", user, user.AccessToken);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.UserAuthenticationFailed, "404", null, "");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "LoginService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> VerifyEmailService(string EmailAddress)
        {
            try
            {
                var user = await _userRepository.VerifyEmailRepo(EmailAddress);
                if (user != null)
                    return ResponseHandler.GetResponse("Success", "User Email Verified", "200", user, user.AccessToken); 

                return ResponseHandler.GetResponse("Exception", ConstantMessages.EmailDoesNotExist, "404", null, ""); //Line added
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "VerifyEmailService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> VerifyForgotPasswordLinkService(string Link)
        {
            try
            {
                var response = await _userRepository.VerifyForgotPasswordLinkRepo(Link);

                return response;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "VerifyForgotPasswordLinkService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> InsertForgotLinkService(tbl_ForgotPasswordLinks entity)
        {
            try
            {
                var response = await _userRepository.InsertForgotLinkRepo(entity);
                if (response.Code == "200")
                {
                   
                    return ResponseHandler.GetResponse("Success", "Inserted", "200", entity, null);
                }
                else
                {
                   
                    return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "InsertForgotLinkService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }


        public async Task<ResponseModel> ResetPasswordService(string Link, string NewPassword)
        {
            try
            {
                var response = await _userRepository.ResetPasswordRepo(Link, NewPassword);

               
                return response;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "ResetPasswordService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> LogoutService(int UserId)
        {
            try
            {
                var user =  await _userRepository.getUserByIdRepo(UserId);
                if (user != null)
                {
                    user.AccessToken = null;
                    user.TokenExpiryDate = user.TokenIssueDate;
                    await _userRepository.UpdateUserRepo(user);
                    return ResponseHandler.GetResponse("Success", "Logout", "200", null, "");
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.UserAuthenticationFailed, "404", null, "");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "LogoutService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> getUserByIdService(int Id)
        {
            try
            {
                var user = await  _userRepository.getUserByIdRepo(Id);
                if (user == null)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }

                return ResponseHandler.GetResponse("Success", "Get User Profile", "200", user, user.AccessToken);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "getUserByIdService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> ChangePassword(ChangePassViewModel model)
        {
            try
            {
                var user = await _userRepository.getUserByIdRepo(model.UserId);
                if (user == null || user.Password != Encryption.EncodePasswordToBase64(model.OldPassword))
                {
                    return new ResponseModel { Code = "400", Status = "Failed", Message = "Current password is not correct." };
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    return new ResponseModel { Code = "400", Status = "Failed", Message = "Password and confirm password do not match." };
                }

                if (model.NewPassword == model.OldPassword)
                {
                    return new ResponseModel { Code = "400", Status = "Failed", Message = "New password should be different." };
                }

                user.Password = Encryption.EncodePasswordToBase64(model.NewPassword);
                await _userRepository.UpdateUserRepo(user);

                return ResponseHandler.GetResponse("Success", "Password has been Changed", "200", null, user.AccessToken);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "ChangePassword", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> UpdateUserService(tbl_Users user)
        {
            try
            {
                var response = await _userRepository.UpdateUserRepo(user);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", "User updated", "200", user, user.AccessToken);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.RegistrationFailed, "408", user, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "UpdateUserService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        //For Web
        public async Task<IEnumerable<tbl_Users>> GetAllUsersService()
        {
            try
            {
                var model = await _userRepository.getAllUsersRepo();
                return model;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        //For Web Admin role
        public string GetUserByNameService(string role)
        {
            try
            {
                var user = _userRepository.GetUserByNameRepo(role);
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<ResponseModel> DeleteUserByIdService(int Id)
        {
            try
            {
                var data = await _userRepository.DeleteUserByIdRepo(Id);
                
                return ResponseHandler.GetResponse("Success", "User has been deleted", "200", data, null);
            }
            catch (Exception ex)
            {

                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "DeleteUserByIdRepo", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }
    }
}
