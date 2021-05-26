using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.ViewModel
{
    public class LoginViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string FCMDeviceToken { get; set; }
    }
}
