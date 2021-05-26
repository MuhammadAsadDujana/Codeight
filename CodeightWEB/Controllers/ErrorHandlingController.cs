using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeightWEB.Controllers
{
    [AllowAnonymous]
    public class ErrorHandlingController : Controller
    {
        // GET: ErrorHandling
        public ActionResult ErrorMessage()
        {
            return View();
        }
    }
}