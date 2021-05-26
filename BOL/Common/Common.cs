using BOL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BOL.Common
{
    public class Common
    {

        public static ResponseModel IsValidEmail(string emailaddress)
        {
            try
            {
                if (string.IsNullOrEmpty(emailaddress))
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.EmailRequired, "400", null, "");

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(emailaddress);
                if (match.Success)
                {
                    return new ResponseModel { Status = "Success" };
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.EnterValidEmail, "400", null, "");
                }
            }
            catch (FormatException ex)
            {
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public static ResponseModel IsPasswordValid(string Password, string ConfirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                    return ResponseHandler.GetResponse("Failed", "Password is required.", "400", null, "");

                if (!Password.Equals(ConfirmPassword))
                    return ResponseHandler.GetResponse("Failed", "Password and confirm password do not match.", "400", null, "");

                return new ResponseModel { Status = "Success" };
            }
            catch (Exception ex)
            {
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public static ResponseModel CheckPasswordValid(string password, string confirmPassword)
        {
            try
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' };
                if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                {
                    return ResponseHandler.GetResponse("Failed", "Please fill Password.", "400", null, "");
                }
                else if (string.IsNullOrEmpty(confirmPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return ResponseHandler.GetResponse("Failed", "Please fill Confirm Password.", "400", null, "");
                }
                else if (!password.Contains(confirmPassword))
                {
                    return ResponseHandler.GetResponse("Failed", "Password and Confirm password do not match.", "400", null, "");
                }
                else if (password.Length < 9)
                {
                    return ResponseHandler.GetResponse("Failed", "Password should be at least 8 characters long and should include numbers, letters and special characters", "400", null, "");
                }
                else if (password.IndexOfAny(special) == -1)
                {
                    return ResponseHandler.GetResponse("Failed", "Password should be at least 8 characters long and should include numbers, letters and special characters", "400", null, "");
                }
                else
                {
                    return new ResponseModel { Status = "Success", Code = "200" };
                }
            }
            catch (Exception ex)
            {
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }
    }

}
