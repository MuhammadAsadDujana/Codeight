using BOL.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeightWEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NotificationController : Controller
    {
        //Get the server key from FCM console
        static string serverKey = string.Format("key={0}", ConfigurationManager.AppSettings["FCM_Server_Key"]);
        static string fcm_Url = ConfigurationManager.AppSettings["FCM_URL"].ToString();
        // Get the sender id from FCM console
        static string senderId = string.Format("id={0}", ConfigurationManager.AppSettings["FCM_SenderId"]);
        static string topicForAllUsers =  ConfigurationManager.AppSettings["TopicForAllUsers"];

        // GET: Notification
        public ActionResult PushNotifcation()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> PushNotifcation(string title, string message)
        {
            //var str = SendNotificationFromFirebaseCloud(title, message);
            var str = await NotifyAsync(topicForAllUsers, title, message);
            //return View();
            //if (str.IsCompleted == true)
            if (str)
            {
                return JsonConvert.SerializeObject(str, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            else
            {
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
            }

        }

        public async Task<bool> NotifyAsync(string to, string title, string body)
        {
            try
            {
                // Get the server key from FCM console
                //var serverKey = string.Format("key={0}", "AAAA2iBKcq0:APA91bGEqi4_KzhP_axTPgko2U-SqJ81qqSMimmkeuwG3FuZA9xD2eQMUugZLF6mjAoP1lAtTjjn5vO0eVteOEV7Oq7Qyz_O57rDUJvaxcj0de-3UM_Z-vgG5_ZUgSuugDeiHvRb-Fvk");

                // Get the sender id from FCM console
                //var senderId = string.Format("id={0}", "936844620461");

                var data = new
                {
                    to, // Recipient device token
                    notification = new { title, body }
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(data);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, fcm_Url))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                            //return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                            //  return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            //   return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // _logger.LogError($"Exception thrown in Notify Service: {ex}");
            }

            return false;
            //return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "404", null, ""));
        }
    }
}