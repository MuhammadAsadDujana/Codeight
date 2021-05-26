using BOL.dbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeightWEB.Controllers
{
    public class BaseController : Controller
    {
        public int UserId;

        public string UserToken;

        public tbl_Users CurrentUser;
    }
}