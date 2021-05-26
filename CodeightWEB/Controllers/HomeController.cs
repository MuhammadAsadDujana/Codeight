using OpenTokSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeightWEB.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult StopVideo(string sessionId, string connectionId)
        {
            try
            {
                var apiKey = int.Parse("47032654");
                var apiSecret = "7e0865e8971834c65f4186ea2fda67e5043076f4";
                var opentok = new OpenTok(apiKey, apiSecret);
             //   opentok.StopArchive();
                opentok.ForceDisconnect(sessionId, connectionId);

                return RedirectToAction("Home/About");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AboutUs()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult PrivacyPage()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}