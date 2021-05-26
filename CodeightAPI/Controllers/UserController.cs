using BLL.UserServices;
using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using CodeightAPI.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeightAPI.Controllers
{
    public class UserController : BaseController
    {
        private UserService _userService;
        public UserController()
        {
            _userService = new UserService();
        }

        //[Route("api/User/Signup")]
        [HttpPost]
        public async Task<string> Signup(UserViewModel userViewModel)
        {
            try
            {
                //Checking email format
                var EmailAdressCheck = Common.IsValidEmail(userViewModel.Email);
                if (!EmailAdressCheck.Status.Equals("Success"))
                    return JsonConvert.SerializeObject(EmailAdressCheck);

                //User email already exists or not
                var user =  (tbl_Users)(await _userService.VerifyEmailService(userViewModel.Email)).Body;
                if (user != null)
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", ConstantMessages.EmailExist, "400", null, ""));

                tbl_Users NewUser = new tbl_Users
                {
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    DateOfBirth = userViewModel.DateOfBirth,
                    PhoneNumber = userViewModel.PhoneNumber,
                    Email = userViewModel.Email.ToLower(),
                    Password = Encryption.EncodePasswordToBase64(userViewModel.Password),
                    UserType = (int?)UserType.User,
                    UserStatus = (int?)UserStatus.Verified,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userViewModel.Email.ToLower(),
                    IsActive = true,
                    ZipCode = userViewModel.ZipCode,
                    CityId = userViewModel.CityId
                    //   Address = userViewModel.Address,
                    //      UserName = userViewModel.UserName,
                    //TokenIssueDate = DateTime.Now,
                    //TokenExpiryDate = DateTime.Now,
                };

                var Result = await _userService.InsertUserService(NewUser);
                if (Result.Code == "200")
                {
                    //new code edit
                    var result = new ResponseModel { Code = "200", Body = Result.Body, Message = ConstantMessages.RegistrationSuccess, Status = "Success", AccessToken = "" }; 
                     return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


                    //old code
                    //var responseLogin = _userService.LoginService(userViewModel.Email, userViewModel.Password, UserType.User);
                    //if (responseLogin.Code == "200")
                    //{
                    //    var result = new ResponseModel { Code = "200", Body = responseLogin.Body, Message = ConstantMessages.RegistrationSuccess, Status = "Success", AccessToken = "Token generate here" };
                    //    return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    //}
                    //else
                    //    return JsonConvert.SerializeObject(responseLogin, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else
                    return JsonConvert.SerializeObject(Result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                  
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

     // [Route("api/User/Login")]
        [HttpPost]
        public async Task<string> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EmailAdressCheck = Common.IsValidEmail(loginViewModel.email.ToLower());
                    if (!EmailAdressCheck.Status.Equals("Success"))
                        return JsonConvert.SerializeObject(EmailAdressCheck);

                    var Result = await _userService.LoginService(loginViewModel.email.ToLower(), loginViewModel.password, UserType.User, loginViewModel.FCMDeviceToken);
                    //var u = (tbl_Users)(Result.Body);

                    //UserViewModel user = new UserViewModel
                    //{
                    //    UserName = u.UserName,
                    //    Email = u.Email,
                    //    UserType = (int)u.UserType,
                    //    UserStatus = (int) u.UserStatus,
                    //    UserId = u.UserId,
                    //    AccessToken = u.AccessToken
                    //};

                    //user.ProfileImage = ""; //working...
                    //  UserId = ((tbl_Users) Result.Body).UserId;  // store temprory 
                    var data = new ResponseModel { Code = Result.Code, Status = Result.Status, Message = Result.Message, Body = null, AccessToken = Result.AccessToken };                    
                    return JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else
                {
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }


        [HttpGet]
        [UserAuthorization]
        public async Task<string> Logout()
        {
            try
            {
                var Result = await _userService.LogoutService(UserId);
                return JsonConvert.SerializeObject(Result);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<string> ChangePassword(ChangePassViewModel changePassViewModel)
        {
            try
            {
                changePassViewModel.UserId = UserId;
                var result = await _userService.ChangePassword(changePassViewModel);
                return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
             
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        [HttpPost]
        public async Task<string> ForgotPassword(string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var EmailAdressCheck = Common.IsValidEmail(email);
                    if (!EmailAdressCheck.Status.Equals("Success"))
                        return JsonConvert.SerializeObject(EmailAdressCheck);

                    var user = (tbl_Users)(await _userService.VerifyEmailService(email)).Body;
                    if (user == null)
                        return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", ConstantMessages.EmailDoesNotExist, "400", null, ""));

                    var DynamicLink = Encryption.GenerateRandomString(32).ToUpper();
                    tbl_ForgotPasswordLinks PasswordLinks = new tbl_ForgotPasswordLinks
                    {
                        Email = email,
                        Link = DynamicLink,
                        UserId = user.UserId,
                        CreatedDate = DateTime.Now,
                        ExpiryDate = DateTime.Now.AddDays(2),
                        IsActive = true
                    };

                    var reset_url = System.Configuration.ConfigurationManager.AppSettings["liveUrl"] + "User/VerifyLink/?id=" + DynamicLink;
                    var _htmlBody = Mailer.ResetPasswordPopulateBody(reset_url, "~/Content/templates/resetpassword.html");

                    ResponseModel EmailResult = Mailer.SendEmail(email, _htmlBody, "Reset your Password of Codeight App");
                    if (EmailResult.Code == "200")
                    {
                        var Result = await _userService.InsertForgotLinkService(PasswordLinks);
                        if (Result.Code == "200")
                            return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Success", ConstantMessages.ChangePasswordMail, "200", null, ""));
                        else
                            return JsonConvert.SerializeObject(Result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(EmailResult, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", ConstantMessages.ModelIsNotValid, "400", null, ""));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        [HttpGet]
        public async Task<System.Web.Mvc.ActionResult> VerifyLink(string id)
        {
            try
            {
                var verify = await _userService.VerifyForgotPasswordLinkService(id);
                if (verify.Code == "200")
                {
                    ViewBag.Link = id;
                    return View("ResetPassword");
                }
                else
                {
                    ViewBag.Status = "Oh! The reset password link has expired. Please use Forgot Password option from the app once more and we will send you a new link for resetting your password.";
                    return View("Confirmation");
                }
            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, null));
            }
        }

        [HttpPost]
        public async Task<System.Web.Mvc.ActionResult> UpdatePassword()
        {
            try
            {
                string Link = Request.Form["Link"];
                var PasswordValidation = Common.CheckPasswordValid(Request.Form["NewPass"], Request.Form["ConfirmPass"]);
                if (!PasswordValidation.Status.Equals("Success"))
                {
                    ViewBag.Link = Link;
                    ViewBag.Error = PasswordValidation.Message;
                    return View("ResetPassword");
                }

                var Result = await _userService.ResetPasswordService(Link, Request.Form["NewPass"]);
                ViewBag.Status = Result.Message;
                return View("Confirmation");
            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, null));
            }
        }

        [HttpGet]
        [UserAuthorization]
        public async Task<string> UserProfile()
        {
            try
            {
                var data = await _userService.getUserByIdService(UserId);
                var _Users = (tbl_Users) data.Body;
                
                UserViewModel userViewModel = new UserViewModel();
            //  userViewModel.UserId = _Users.UserId;
                userViewModel.FirstName = _Users.FirstName;
                userViewModel.LastName = _Users.LastName;
                userViewModel.DateOfBirth = (DateTime)_Users.DateOfBirth;
                userViewModel.ProfileImage = _Users.ProfileImage;
                userViewModel.CityId = (int)(_Users.CityId.HasValue == true ? _Users.CityId : 0);
                userViewModel.CityName = _Users.tbl_Cities == null ? String.Empty : _Users.tbl_Cities.CityName;
                userViewModel.CountryName = _Users.tbl_Cities == null ? string.Empty : _Users.tbl_Cities.tbl_States.tbl_Countries.CountryName;
                userViewModel.StateName = _Users.tbl_Cities == null ? string.Empty : _Users.tbl_Cities.tbl_States.StateName;
                userViewModel.ZipCode = _Users.ZipCode;
                userViewModel.Email = _Users.Email;
                userViewModel.PhoneNumber = _Users.PhoneNumber;
                
             //   var c = country.tbl_Cities.tbl_States.tbl_Countries.CountryName;
                var result = new ResponseModel { Code = data.Code, Body = userViewModel, Message = data.Message, Status = data.Status, AccessToken = data.AccessToken };
                return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
               
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<string> EditUserProfile(UserViewModel userViewModel)
        {
            try
            {
                if (UserId == null)
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""));

                var user = (tbl_Users)(await _userService.getUserByIdService(UserId)).Body;

                string UploadedImage = "";
                if (userViewModel.imageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(userViewModel.imageUpload.FileName);
                    string extension = Path.GetExtension(userViewModel.imageUpload.FileName);
                    fileName = "content/profile_pictures/" + fileName + DateTime.Now.ToString("yymmssff") + extension;

                    string path = ConfigurationManager.AppSettings["liveUrl"] + fileName;

                    fileName = Path.Combine(Server.MapPath("~/"), fileName);
                    userViewModel.imageUpload.SaveAs(fileName);
                    //  encImage = TripleDESCryptography.Encrypt(path);
                    UploadedImage = path;
                    userViewModel.ProfileImage = UploadedImage;

                }
                else
                {
                    UploadedImage = null;
                }


                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.DateOfBirth = userViewModel.DateOfBirth;
                user.CityId = userViewModel.CityId;
                user.ZipCode = userViewModel.ZipCode;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.ModifiedDate = DateTime.Now;
                //  user.ProfileImage = Images.IsBase64(userViewModel.ProfileImage) == false ? user.ProfileImage : Images.Base64ToImage(userViewModel.ProfileImage, Server);
                user.ProfileImage = UploadedImage != null ? userViewModel.ProfileImage : user.ProfileImage;
                user.ModifiedBy = user.Email.ToLower();

                var data = await _userService.UpdateUserService(user);
                if (data.Code == "200")
                {
                    var result = new ResponseModel { Code = data.Code, Status = data.Status, Message = data.Message, Body = null, AccessToken = data.AccessToken };
                    return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                }
                else
                    return JsonConvert.SerializeObject(data.Message, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }



        [HttpPost]
        [UserAuthorization]
        public async Task<string> EditUserProfile_old(UserViewModel userViewModel)
        {
            try
            {
                if (UserId == null)
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""));

                var user = (tbl_Users)(await _userService.getUserByIdService(UserId)).Body;

                    user.FirstName = userViewModel.FirstName;
                    user.LastName = userViewModel.LastName;
                    user.DateOfBirth = userViewModel.DateOfBirth;
                    user.CityId = userViewModel.CityId;
                    user.ZipCode = userViewModel.ZipCode;
                    user.PhoneNumber = userViewModel.PhoneNumber;
                    user.ModifiedDate = DateTime.Now;
                    user.ProfileImage = Images.IsBase64(userViewModel.ProfileImage) == false ? user.ProfileImage : Images.Base64ToImage(userViewModel.ProfileImage, Server);
                    user.ModifiedBy = user.Email.ToLower();
                //  user.Address = userViewModel.Address;
                //   user.Email = userViewModel.Email.ToLower();
                // user.UserName = userViewModel.UserName;
                //     user.Password = Encryption.EncodePasswordToBase64(userViewModel.Password);
                //user.UserType = (int?)UserType.User;
                //user.UserStatus = (int?)UserStatus.Verified; 
                //TokenIssueDate = DateTime.Now,
                //TokenExpiryDate = DateTime.Now,

                var data = await _userService.UpdateUserService(user);
                if (data.Code == "200")
                {
                    var result = new ResponseModel { Code = data.Code, Status = data.Status, Message = data.Message, Body = null, AccessToken = data.AccessToken };
                    return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                   
                }
                else
                   return JsonConvert.SerializeObject(data.Message, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }


    }
}
