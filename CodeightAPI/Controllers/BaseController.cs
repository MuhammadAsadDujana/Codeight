using BOL.dbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace CodeightAPI.Controllers
{
    public class BaseController : Controller
    {
        public int UserId;

        public string UserToken;

        public tbl_Users CurrentUser;

        //public static explicit operator BaseController(ControllerBase v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
