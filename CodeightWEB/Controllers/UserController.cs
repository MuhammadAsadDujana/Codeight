using BLL.UserServices;
using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using CodeightWEB.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CodeightWEB.Controllers
{
   // [AllowAnonymous]
    public class UserController : BaseController
    {
        private UserService _userService;
        public UserController()
        {
            _userService = new UserService();
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["UserCookie"];
            if (cookie != null)
            {
                string encryptedPass = cookie["password"].ToString();
                byte[] pass = Convert.FromBase64String(encryptedPass);
                string decryptedPass = ASCIIEncoding.ASCII.GetString(pass);
                ViewBag.email = cookie["email"].ToString();
                ViewBag.password = decryptedPass.ToString();
            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Login(string email, string password, bool RememberMe)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var EmailAdressCheck = Common.IsValidEmail(email.ToLower());
                    if (!EmailAdressCheck.Status.Equals("Success"))
                        return JsonConvert.SerializeObject(EmailAdressCheck);


                    var Result = await _userService.LoginService(email.ToLower(), password, UserType.Admin, null);
                    var User = (tbl_Users)Result.Body;

                    if (Result.Status == "Success")
                    {
                        Session["Token"] = Result.AccessToken;
                        Session["UserId"] = User.UserId;
                        FormsAuthentication.SetAuthCookie(User.Email, false);

                        HttpCookie cookie = new HttpCookie("UserCookie");
                        if (RememberMe == true)
                        {
                            byte[] pass = ASCIIEncoding.ASCII.GetBytes(password);
                            string encryptedPass = Convert.ToBase64String(pass);
                            cookie["email"] = email;
                            cookie["password"] = encryptedPass;
                            cookie.Expires = DateTime.Now.AddHours(6);
                            HttpContext.Response.Cookies.Add(cookie);
                        }
                        else
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            HttpContext.Response.Cookies.Add(cookie);
                        }
                    }

                 //   var data = new ResponseModel { Code = Result.Code, Status = Result.Status, Message = Result.Message, Body = Result.Body, AccessToken = Result.AccessToken };
                    //    return JsonConvert.SerializeObject(data);
                    return JsonConvert.SerializeObject(Result.Code);
                }
                else
                {
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
                }
            }

            catch (Exception ex)
            {
                //  return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }

        }


        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UserManagement()
        {
            try
            {
                var Result = await _userService.GetAllUsersService();
                var users = Result.OrderByDescending(x => x.CreatedDate).ToList();
                
                return View(users);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
        }


        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async  Task<string> markVerifiedUser(int UserId, int userStatus)
        {
            try
            {
               // var result = new ResponseModel();
                if (!UserId.Equals(null))
                {
                    var user = (tbl_Users) (await _userService.getUserByIdService(UserId)).Body;
                    user.UserStatus = userStatus;
                //  user.IsActive = userStatus == 1 ? true : false;
                    var data = await _userService.UpdateUserService(user);
                    if(data.Code == "200")
                    {
                      //  var  result = new ResponseModel { Code = data.Code, Body = data.Body, Message = data.Message, Status = data.Status, AccessToken = user.AccessToken };
                        return JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    }

                    //foreach (var item in checkedList)
                    //{
                    //    var Userid = new Guid(item);
                    //    var user = (User)(await _userService.GetUserBy(Userid)).Body;
                    //    user.UserStatus = UserStatus.Verified;
                    //    var data = await _userService.UpdateUser(user);
                    //    if (data.Code == "200")
                    //    {
                    //        //var _htmlBody = Mailer. ContactUsPopulateBody(user.FirstName + " " + user.LastName, user.Email, message, "~/content/templates/contactus.html");
                    //        var _htmlBody = Mailer.VerifyUserByAdmin(user.LastName, "~/content/templates/verifyuserbyadmin.html");
                    //        var toemail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"];
                    //        //ResponseModel EmailResult = Mailer.SendEmail(toemail, _htmlBody, subject);
                    //        ResponseModel emailResult = Mailer.SendEmail(user.Email, _htmlBody, "You're now a Verified user");
                    //    }
                    //    result = new ResponseModel { Code = data.Code, Body = data.Body, Message = data.Message, Status = data.Status, AccessToken = user.AccessToken };
                    //}
                    ////var model = await _userService.GetAllUsers();
                    ////var users = model.ToList();
                    //return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
                //    return JsonConvert.SerializeObject(result);
                //}

                //return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", ConstantMessages.ModelIsNotValid, "400", null, ""));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }

        }


        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
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
                    var _htmlBody = Mailer.ResetPasswordPopulateBody(reset_url, "~/content/templates/resetpassword.html");

                    ResponseModel EmailResult = Mailer.SendEmail(email, _htmlBody, "Reset your Password - Codeight");
                    if (EmailResult.Code == "200")
                    {
                        var Result = await _userService.InsertForgotLinkService(PasswordLinks);
                        if (Result.Code == "200")
                            return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Success", ConstantMessages.ChangePasswordMail, "200", null, ""));
                        else
                            return JsonConvert.SerializeObject(Result);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(EmailResult);
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
        [AllowAnonymous]
        public async Task<ActionResult> VerifyLink(string id)
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
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePassword()
        {
            try
            {
                string Link = Request.Form["Link"];
                var PasswordValidation = Common.CheckPasswordValid(Request.Form["NewPass"], Request.Form["ConfirmPass"]);
                if (!PasswordValidation.Status.Equals("Success"))
                {
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
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Logout()
        {
            //HttpContext.Session.Clear();
            //FormsAuthentication.SignOut();
            //return Json(true, JsonRequestBehavior.AllowGet);

            try
            {

                var Result = await _userService.LogoutService((int)Session["UserId"]);
                HttpContext.Session.Clear();
                
                FormsAuthentication.SignOut();
               // return JsonConvert.SerializeObject(Result);
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return Json(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""), JsonRequestBehavior.AllowGet);
                //    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }

        }

     
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassword()
        {
            return View();
        }

     
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<string> ChangePassword(ChangePassViewModel model)
        {
            try
            {
                //if (Session["UserId"] == null)
                //    return RedirectToAction("Login");

                var PasswordValidation = Common.CheckPasswordValid(model.NewPassword, model.ConfirmPassword);
                if (!PasswordValidation.Status.Equals("Success"))
                {
                    //  ViewBag.Error = PasswordValidation.Message;
                    return JsonConvert.SerializeObject(PasswordValidation);
                }



                //model.UserId = (int)Session["UserId"];
                //return JsonConvert.SerializeObject(await _userService.ChangePassword(model));

                model.UserId = (int)Session["UserId"];
                var Result = await _userService.ChangePassword(model);

                var data = new ResponseModel { Code = Result.Code, Status = Result.Status, Message = Result.Message, Body = Result.Body, AccessToken = Result.AccessToken };
                return JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }


        [HttpPost]
        public async Task<string> DeleteUser(int UserId)
        {
            try
            {
                var data = await _userService.DeleteUserByIdService(UserId);
                if(data.Code == "200")
                {
                    var result = new ResponseModel { Code = data.Code, Body = data.Body, Message = data.Message, Status = data.Status, AccessToken = data.AccessToken };
                    return JsonConvert.SerializeObject(result);
                }

                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Record is not deleted", "400", null, ""));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }


        [HttpGet]
        public async Task<ActionResult> EditUser(int userId)
        {
            try
            {
                if (userId != null)
                {
                    var user = await _userService.getUserByIdService(userId);
                    var data = (tbl_Users)user.Body;

                    return View(data);
                }
               
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            return View();
        }
    }
}