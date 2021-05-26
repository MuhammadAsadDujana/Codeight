using BLL.SessionServices;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using Newtonsoft.Json;
using OpenTokSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeightWEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SessionController : Controller
    {

        private SessionServices _sessionService;

        public SessionController()
        {
            _sessionService = new SessionServices();
        }

        // GET: Session
        public async Task<ActionResult> VideoManagement()
        {
            try
            {
                //  var videoCategory = roomForm.VideoCategory;
                //archiveId = "41a0cefa-a12b-4bad-9b49-cc734bd0cf20";
                // var archive = opentok.GetArchive(archiveId);
                //var Url = archive.Url;
                //var duration = archive.Duration;
                var opentok = getOpenTokInstance();

                if (opentok.ListArchives().Count > 0)
                {
                    var listArchives = opentok.ListArchives();
                    //var activeList = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE || x.Status == ArchiveStatus.STARTED);
                    var activeList = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE);
                    List<VideoManagementViewModel> finalList = new List<VideoManagementViewModel>();
                    foreach (var item in activeList)
                    {
                        VideoManagementViewModel v = new VideoManagementViewModel();
                        var userData = await _sessionService.getRoomBySessionIdService(item.SessionId);
                        var u = (tbl_Room)(userData.Body);

                        v.UserName = u != null ? u.tbl_Users.FirstName +" "+ u.tbl_Users.LastName : String.Empty;
                        v.ArchiveId = item.Id;
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(item.Duration);
                        v.VideoDuration = dateTimeOffset.TimeOfDay;
                        var dateTimeOffsetMilli = DateTimeOffset.FromUnixTimeMilliseconds(item.CreatedAt);
                        v.CreatedAt = dateTimeOffsetMilli.UtcDateTime;
                        var convertVideo = Math.Round((double)item.Size / (1024 * 1024), 1);
                        v.VideoSize = convertVideo <= 1024 ? convertVideo + " MB" : convertVideo + " KB";
                        v.SessionId = item.SessionId;
                        v.VideoURL = item.Url;
                        v.Status = item.Status.ToString();

                        finalList.Add(v);
                    }



                    //var activeList = List.Where(x => x.Status == ArchiveStatus.AVAILABLE);

                    return View(finalList);
                }
                else
                    return View();

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View();
            }
           
        }

        [HttpPost]
        public string DeleteVideo(string ArchiveId)
        {
            try
            {
                if (ArchiveId != null)
                {
                    var opentok = getOpenTokInstance();
                    opentok.DeleteArchive(ArchiveId);
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Success", "Record is deleted", "200", null, ""));
                }

                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Record is not deleted", "400", null, ""));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        public OpenTok getOpenTokInstance()
        {
            var apiKey = int.Parse("47032654");
            var apiSecret = "7e0865e8971834c65f4186ea2fda67e5043076f4";
            var opentok = new OpenTok(apiKey, apiSecret);
            return opentok;
        }

        [HttpGet]
        public async Task<ActionResult> ManageRadius()
        {
            
            var LastManageRadius = await _sessionService.GetLastRadiusService();
            if(LastManageRadius != null)
                ViewBag.LastRadiusValue = LastManageRadius;
           
            return View();
        }

        [HttpPost]
        public async Task<string> ManageRadius(double radiusValue, int PreviousId)
        {
            try
            {
                if (!radiusValue.Equals(null))
                {
                    var Result = await _sessionService.editRadiusByIdService(radiusValue, PreviousId);
                    if (Result.Code == "200")
                    {
                       // var result = new ResponseModel { Code = data.Code, Body = data.Body, Message = data.Message, Status = data.Status, AccessToken = data.AccessToken };
                        return JsonConvert.SerializeObject(Result);
                    }
                
                }
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }


    }
}