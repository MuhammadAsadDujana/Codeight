using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BLL.SessionServices;
using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using CodeightAPI.Attributes;
using Newtonsoft.Json;
using OpenTokSDK;

namespace CodeightAPI.Controllers
{
    public class SessionController : BaseController
    {

        private SessionServices _sessionService;
        public SessionController()
        {
            _sessionService = new SessionServices();
        }

        [HttpGet]
        [UserAuthorization]
        public async Task<object> GetRecentSessionList()
        {
            try
            {
                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                string PublisherSessionId = null;
                var apiKey = int.Parse(ConfigurationManager.AppSettings["OTapiKey"]);
                var apiSecret = ConfigurationManager.AppSettings["OTapiSecret"];
                var opentok = new OpenTok(apiKey, apiSecret);

                var nearByVideoList = await _sessionService.getRecentNearByVideoListService(UserId);
                List<NearByUserViewModel> viewModels = new List<NearByUserViewModel>();
                List<ArchiveList> archiveRecentList = new List<ArchiveList>();

                if(nearByVideoList.Count() > 0)
                {
                    // PublisherSessionId = nearByVideoList.Select(x => x.PublisherSessionId).FirstOrDefault();

                    foreach (var item in nearByVideoList)
                    {
                        PublisherSessionId = item.PublisherSessionId;
                        var roomResult = await _sessionService.getRoomBySessionIdService(PublisherSessionId);
                        var room = (tbl_Room)roomResult.Body;
                        var listArchives = opentok.ListArchives(0, 0, PublisherSessionId);
                        var archive = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE);
                        if (archive.Count() > 0)
                        {
                         //   archiveRecentList.Add(listArchives);
                            //var list =  listArchives.Select(x => new
                            //  {
                            //    Url = x.Url,
                            //    ArchiveId = x.Id,
                            //    Status = x.Status.ToString(),
                            //    VideoDuration = x.Duration
                            //  });

                            foreach (var i in archive)
                            {
                                NearByUserViewModel v = new NearByUserViewModel();
                                v.VideoURL = i.Url;
                                v.ArchiveId = i.Id;
                                v.Status = i.Status.ToString();
                                v.SessionId = i.SessionId;
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(i.Duration);
                                v.VideoDuration = dateTimeOffset.TimeOfDay;
                                v.UserId = (int)item.roomJoinedUserId;
                              //  v.Role = Role.PUBLISHER.ToString();
                                v.VideoCategory = item.VideoCategory;
                                v.roomCreatedLat = room.Latitude;
                                v.roomCreatedLng = room.longitude;
                                viewModels.Add(v);
                            }
                        }

                       
                    }

                    if (viewModels.Count > 0)
                        return Json(new { AvailableList = viewModels }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                    else
                        return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [UserAuthorization]
        public async Task<object> GetActiveSessionList()
        {
            try
            {

                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

               // string PublisherSessionId = null;

                var opentok = getOpenTokInstance();
               // var listArchives = opentok.ListArchives();
                //var activeList = listArchives.Where(x => x.Status == ArchiveStatus.STARTED);
                //List<LiveStream> finalList = new List<LiveStream>();

                var nearByVideoList = await _sessionService.getActiveNearByVideoListService(UserId);
                List<NearByUserViewModel> viewModels = new List<NearByUserViewModel>();

                if (nearByVideoList.Count() > 0)
                {
                //  var Publisher = nearByVideoList.Select(x => new { x.PublisherSessionId, x.PublisherToken, x.VideoCategory } ).FirstOrDefault();

                    foreach (var item in nearByVideoList)
                    {
                        //  PublisherSessionId = item.PublisherSessionId;
                        var roomResult = await _sessionService.getRoomBySessionIdService(item.PublisherSessionId);
                        var room = (tbl_Room)roomResult.Body;
                        var listArchives = opentok.ListArchives(0, 0, item.PublisherSessionId);
                        var activeList = listArchives.Where(x => x.Status == ArchiveStatus.STARTED);

                        //var subscriberSessionId = Publisher.PublisherSessionId;
                        //var subscriberToken = opentok.GenerateToken(subscriberSessionId, Role.SUBSCRIBER);

                    if (activeList.Count() > 0)
                        {

                            foreach (var i in activeList)
                            {
                                NearByUserViewModel v = new NearByUserViewModel();
                                //v.VideoURL = i.Url;
                                v.ArchiveId = i.Id;
                                v.Status = i.Status.ToString();
                                v.SessionId = i.SessionId;
                                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(i.Duration);
                                //v.VideoDuration = dateTimeOffset.TimeOfDay;
                                v.PublisherToken = item.PublisherToken;
                              //  v.Role = Role.PUBLISHER.ToString();
                                v.VideoCategory = item.VideoCategory;
                                v.UserId = (int) item.roomJoinedUserId;
                                //  v.UserId = (int)item.roomJoinedUserId; 
                                v.roomCreatedLat = room.Latitude;
                                v.roomCreatedLng = room.longitude;
                                viewModels.Add(v);
                            }



                            //return Json(new { ActiveList = activeList, publisherToken = Publisher.PublisherToken, Role = Role.PUBLISHER.ToString(), videoCategory = Publisher.VideoCategory }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                        }

                    }

                    if (viewModels.Count > 0)
                        return Json(new { AvailableList = viewModels }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                    else
                        return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                }
                else
                    return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        [UserAuthorization]
        public async Task<object> JoinSession([FromBody] RoomViewModel roomForm)
        {
            try
            {
                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                var Result = await _sessionService.getRoomBySessionIdService(roomForm.SessionId);
                
                if(Result.Code == "200")
                {
                    string archiveId = null, videoURL= null, videoStatus =null;
                    Role role;
                    var apiKey = int.Parse(ConfigurationManager.AppSettings["OTapiKey"]);
                    var apiSecret = ConfigurationManager.AppSettings["OTapiSecret"];
                    var opentok = new OpenTok(apiKey, apiSecret);

                    //  ArchiveStatus archiveStatus;
                    var room = (tbl_Room)Result.Body;
                    int roomJoinedUserId = UserId;
                    int roomCreatedUserId = (int) room.UserId;
                    var UserEmail = room.tbl_Users.Email;
                    var videoCategory = room.VideoCategory;
               
                    var subscriberSessionId = roomForm.SessionId;
                    var subscriberToken = opentok.GenerateToken(subscriberSessionId, Role.SUBSCRIBER);

                    //string SessionId = roomCreated.SessionId;
                    // var streamList = opentok.ListStreams(roomForm.SessionId);  //not in use just to check
                      
                    var listArchives = opentok.ListArchives(0, 0, roomForm.SessionId);
                    
                    if(listArchives.Count > 0)
                    {
                        //foreach (var item in listArchives)
                        //{
                        //   archiveId = item.Id.ToString();
                        //   videoURL = item.Url;
                        //    videoStatus = item.Status.ToString();
                        //}
                        //var activeList = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE || x.Status == ArchiveStatus.STARTED);

                        return Json(new
                        {
                            ListArchives = listArchives,
                            SubscriberToken = subscriberToken,
                            Role = Role.SUBSCRIBER.ToString()
                        });
                    }

                    //var data = _sessionService.getNearByVideoBySessionIdService(roomForm.SessionId);
                    //var nearByVideo = data.Result;
                    //continue from here

                        //tbl_NearByUser nearByUser = new tbl_NearByUser
                        //{
                        //    PublisherSessionId = roomForm.SessionId,
                        //    PublisherToken = roomForm.PublisherToken,
                        //    SubscriberToken = subscriberToken,
                        //    roomJoinedUserId = roomJoinedUserId,
                        //    roomCreatedUserId = roomCreatedUserId,
                        //    ArchiveId = archiveId,
                        //    CreatedDate = DateTime.Now,
                        //    IsActive = true,
                        //    Latitude = Convert.ToDouble(roomForm.Latitude),
                        //    Longitude = Convert.ToDouble(roomForm.longitude),
                        //    ExpireDate = DateTime.Now.AddHours(24),
                        //    VideoURL = videoURL
                        //};

                        //  nearByUserList.Add(nearByUser); // to show nearby users list in json response
                        //var res = await _sessionService.InsertNearByUserService(nearByUser);
                        //if(res.Code == "200")
                        //{
                        //    var nearByUserData = (tbl_NearByUser)res.Body;
                        //    return Json(new { roomJoinedUserId = nearByUserData.roomJoinedUserId, PublisherSessionId = nearByUserData.PublisherSessionId, 
                        //        SubscriberToken = nearByUserData.SubscriberToken, role = Role.SUBSCRIBER.ToString(), ArchiveId = nearByUserData.ArchiveId, 
                        //        VideoURL = nearByUserData .VideoURL, ExpireDate = nearByUser.ExpireDate, Latitude = nearByUser.Latitude, 
                        //        Longitude = nearByUser.Longitude, videoStatus = videoStatus });
                        //}
                        return Json(ResponseHandler.GetResponse("Failed", Result.Message, Result.Code, Result.Body, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                    //  nearByUserList.Add(nearByUserData); // to show nearby users list in json response

                    //string SessionId =  roomCreated.SessionId;
                    //var listArchives = opentok.ListArchives(0,0,SessionId);
                    //var activeList = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE || x.Status == ArchiveStatus.STARTED);


                }
                else
                    return Json(ResponseHandler.GetResponse("Failed", Result.Message, Result.Code, Result.Body, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<object> CreateSession([FromBody] RoomViewModel roomForm)
        {
            try
            {

                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                 // return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""));

                roomForm.UserId = UserId;
                var apiKey = int.Parse(ConfigurationManager.AppSettings["OTapiKey"]);
                var apiSecret = ConfigurationManager.AppSettings["OTapiSecret"];
                var opentok = new OpenTok(apiKey, apiSecret);
                var videoCategory = roomForm.VideoCategory;
                string sessionId;
                string token;
                Role role;
                List<object> nearByUserList = null;

                //var data = await _sessionService.getRoomByUserIdService(roomForm.UserId);
                //var room = (tbl_Room)data.Body;
                //if (room != null)
                //{
                //    //15 minutes were passed from start logic will be here
                //    var start = DateTime.Now;
                //    var oldDate = room.TokenExpiryDate;
                //    if (start.Subtract((DateTime)oldDate) >= TimeSpan.FromMinutes(15))
                //    {
                //        await _sessionService.DeleteRoomService(room);
                //    }

                //}

                var session = opentok.CreateSession("", MediaMode.ROUTED, ArchiveMode.ALWAYS);
                sessionId = session.Id;
                
                role = Role.PUBLISHER;
                var tokenIssueDate = DateTime.Now;
                var tokenExpiryDate = DateTime.Now.AddMinutes(15);
                token = opentok.GenerateToken(sessionId, role, 0, videoCategory);

                var roomInsert = new tbl_Room
                {
                    SessionId = sessionId,
                    Token = token,
                    VideoCategory = videoCategory,
                    Role = role.ToString(),
                    UserId = roomForm.UserId,
                    TokenIssueDate = tokenIssueDate,
                    TokenExpiryDate = tokenExpiryDate,
                    Latitude = roomForm.lat,
                    longitude = roomForm.lng,
                    IsActive = true
                };

                var Result = await _sessionService.InsertRoomService(roomInsert);
                if (Result.Code == "200")
                {
                    var roomCreated = (tbl_Room)Result.Body;
                    
                    double lat = Convert.ToDouble(roomCreated.Latitude);
                    double lng = Convert.ToDouble(roomCreated.longitude);
                    var nearByList = await _sessionService.getNearByLocationsService(lat, lng);
                    //  string topicForSpecificUser = ConfigurationManager.AppSettings["TopicForSpecificUser"]; //value = "/topics/user_"

                    //string[] commaSeparatedList = null;
                    List<string> stringList = new List<string>();
                    nearByUserList = new List<object>();
                    foreach (var item in nearByList)
                    {
                        if (item.FCMDeviceToken != null)
                        {
                            stringList.Add(item.FCMDeviceToken);

                            //new code added : 3/24/2021
                            tbl_NearByUser nearByUser = new tbl_NearByUser
                            {
                                PublisherSessionId = roomCreated.SessionId,
                                PublisherToken = roomCreated.Token,
                              //  SubscriberToken = subscriberToken,
                                roomJoinedUserId = item.UserId,
                                roomCreatedUserId = roomCreated.UserId,
                              //  ArchiveId = archiveId,
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                Latitude = item.Latitude,
                                Longitude = item.Longitude,
                                ExpireDate = DateTime.Now.AddHours(24),
                                VideoCategory = videoCategory
                                //  VideoURL = videoURL
                            };

                            //  nearByUserList.Add(nearByUser); // to show nearby users list in json response
                            var res = await _sessionService.InsertNearByUserService(nearByUser);
                            //new code added end : 3/24/2021 
                        }

                        // commaSeparatedList = new string[] { item.FCMDeviceToken };

                        //var subscriberSessionId = roomCreated.SessionId;
                        //var subscriberToken = opentok.GenerateToken(subscriberSessionId, Role.SUBSCRIBER);

                        //tbl_NearByUser nearByUser = new tbl_NearByUser
                        //{
                        //    SessionId = roomCreated.SessionId,
                        //    PublisherToken = roomCreated.Token,
                        //    SubscriberToken = subscriberToken,
                        //    UserId = item.UserId,
                        //    CreatedDate = DateTime.Now,
                        //    IsActive = true
                        //    Latitude = Convert.ToDouble(roomCreated.Latitude),
                        //    Longitude = Convert.ToDouble(roomCreated.longitude),
                        //};

                        //  nearByUserList.Add(nearByUser); // to show nearby users list in json response
                        //  var res = await _sessionService.InsertNearByUserService(nearByUser);
                        //  var nearByUserData = (tbl_NearByUser) res.Body;
                        //  nearByUserList.Add(nearByUserData); // to show nearby users list in json response

                        //string SessionId =  roomCreated.SessionId;
                        //var listArchives = opentok.ListArchives(0,0,SessionId);
                        //var activeList = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE || x.Status == ArchiveStatus.STARTED);

                    }

                   



                  //  var a = commaSeparatedList;  //not in use
                  //  var b = stringList;
                    var str = await NotifyAsync(stringList, roomCreated, "Criminal activity going on", "Click here to see the live video");

                    //if (db.SaveChanges() == 1)
                    //{
                    //    var str = NotifyAsync("/topics/Codeight", "Criminal activity going on", "Click here to see the live video");

                    //    if (str.IsCompleted == true)
                    //    {
                    //        return JsonConvert.SerializeObject(str, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    //    }
                    //    else
                    //    {
                    //        return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "Model state is not valid", "400", null, ""));
                    //    }
                    //}
                }
                else
                {
                    return Json(ResponseHandler.GetResponse("Failed", "Insertion failed", Result.Status, Result.Body, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }


                return Json(new { sessionId = sessionId, token = token, apiKey = apiKey, role = role.ToString(), videoCategory = videoCategory, nearByUserList = nearByUserList }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }  

        }

        [HttpPost]
        [UserAuthorization]
        public async Task<object> MapClickedSessionList([FromBody] RoomViewModel roomForm)
        {

            try
            {
                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                if (roomForm.Latitude.Equals(null) || roomForm.longitude.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "Latitude and Longitude are required", "404", null, ""));

                var opentok = getOpenTokInstance();
                List<NearByUserViewModel> viewModels = new List<NearByUserViewModel>();
                string PublisherSessionId = "";
                double lat = roomForm.Latitude;
                double lng = roomForm.longitude;
                var nearByList = await _sessionService.getMapClickedSessionListService(lat, lng);

                if (nearByList.Count() > 0)
                {
                    // PublisherSessionId = nearByVideoList.Select(x => x.PublisherSessionId).FirstOrDefault();

                    foreach (var item in nearByList)
                    {
                        PublisherSessionId = item.SessionId;
                        var listArchives = opentok.ListArchives(0, 0, PublisherSessionId);
                        var archive = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE);
                        if (archive.Count() > 0)
                        {

                            foreach (var i in archive)
                            {
                                NearByUserViewModel v = new NearByUserViewModel();
                                v.VideoURL = i.Url;
                                v.ArchiveId = i.Id;
                                v.Status = i.Status.ToString();
                                v.SessionId = i.SessionId;
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(i.Duration);
                                v.VideoDuration = dateTimeOffset.TimeOfDay;
                                v.UserId = (int)item.UserId;
                                v.Role = item.Role;
                                v.VideoCategory = item.VideoCategory;
                                v.roomCreatedLat = item.Latitude;
                                v.roomCreatedLng = item.longitude;
                                viewModels.Add(v);
                            }
                        }


                    }

                    if (viewModels.Count > 0)
                        return Json(new { AvailableList = viewModels }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                    else
                        return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);


                }
                else
                    return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [UserAuthorization]
        public async Task<object> MySessionList()
        {

            try
            {
                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                var opentok = getOpenTokInstance();
                string PublisherSessionId = "";
                var mySessionList = await _sessionService.getMySessionListService(UserId);
                List<NearByUserViewModel> viewModels = new List<NearByUserViewModel>();
                List<ArchiveList> archiveRecentList = new List<ArchiveList>();

                if (mySessionList.Count() > 0)
                {
                    // PublisherSessionId = nearByVideoList.Select(x => x.PublisherSessionId).FirstOrDefault();

                    foreach (var item in mySessionList)
                    {
                        PublisherSessionId = item.SessionId;
                        var listArchives = opentok.ListArchives(0, 0, PublisherSessionId);
                        var archive = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE);
                        if (archive.Count() > 0)
                        {        

                            foreach (var i in archive)
                            {
                                NearByUserViewModel v = new NearByUserViewModel();
                                v.VideoURL = i.Url;
                                v.ArchiveId = i.Id;
                                v.Status = i.Status.ToString();
                                v.SessionId = i.SessionId;
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(i.Duration);
                                v.VideoDuration = dateTimeOffset.TimeOfDay;
                                v.UserId = (int)item.UserId;
                                v.Role = item.Role;
                                v.VideoCategory = item.VideoCategory;
                                v.roomCreatedLat = item.Latitude;
                                v.roomCreatedLng = item.longitude;
                                viewModels.Add(v);
                            }
                        }


                    }

                    if (viewModels.Count > 0)
                        return Json(new { AvailableList = viewModels }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                    else
                        return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(ResponseHandler.GetResponse("Failed", "Record not found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [UserAuthorization]
        public async Task<object> DisconnectSession()
        {
            try
            {
                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                var nearByVideoList = await _sessionService.DisconnectNearByVideoService(UserId);

                if (nearByVideoList.Code == "200")
                {
                    //return Json(new { msg = "Session disconnected successfully !" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                    return Json(ResponseHandler.GetResponse(nearByVideoList.Status, nearByVideoList.Message, nearByVideoList.Code, nearByVideoList.Body, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }
                
                return Json(ResponseHandler.GetResponse("Failed", "Please provide User token!", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null));
            }  
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<object> SaveUserLastLocation([FromBody] MarkerViewModel viewModel)
         {
            try
            {
                if (UserId <= 0 || UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "User Not Found", "404", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

                viewModel.UserId = UserId;
                if(viewModel.Latitude.Equals(null) || viewModel.Longitude.Equals(null) || viewModel.UserId.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "Latitude and Longitude and UserId are required", "400", null, ""));

                var markerUpdated = await _sessionService.UpdatePreviousMarkerService(UserId, viewModel.Latitude, viewModel.Longitude);

                if (markerUpdated.Code == "200")
                    return Json(ResponseHandler.GetResponse("Success", markerUpdated.Message, "200", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                else
                    return Json(ResponseHandler.GetResponse("Failed", markerUpdated.Message, "400", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null));
            }
         
        }


        [HttpGet]
        [UserAuthorization]
        public async Task<object> GetActiveSessionList_Old()
        {
            try
            {

                var opentok = getOpenTokInstance();
                var listArchives = opentok.ListArchives();
                var activeList = listArchives.Where(x => x.Status == ArchiveStatus.STARTED);
                List<LiveStream> finalList = new List<LiveStream>();

                // var nearByVideoList = await _sessionService.getNearByVideoListService(UserId);
                //  List<NearByUserViewModel> viewModels = new List<NearByUserViewModel>();

                if (activeList.Count() > 0)
                {

                    foreach (var item in activeList)
                    {

                        var data = await _sessionService.getRoomBySessionIdService(item.SessionId);
                        var room = (tbl_Room)data.Body;

                        LiveStream v = new LiveStream();
                        v.UserId = (int)room.UserId;
                        v.SessionId = item.SessionId;
                        v.ArchiveId = item.Id;
                        v.VideoCategory = room.VideoCategory;
                        v.Lat = room.Latitude;
                        v.Lng = room.longitude;
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(item.Duration);
                        v.VideoDuration = dateTimeOffset.TimeOfDay;
                        v.VideoURL = item.Url;
                        v.Status = item.Status.ToString();
                        finalList.Add(v);
                    }

                    return Json(new { finalList = finalList }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }

                //var activeList = List.Where(x => x.Status == ArchiveStatus.AVAILABLE);

                return Json(new { msg = "No active video found" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

        }

        //front end no use
        [HttpGet]
        public async Task<object> getNearByLocationList([FromBody] MarkerViewModel viewModel)
        {
            try
            {
                if (viewModel.Latitude.Equals(null) || viewModel.Longitude.Equals(null))
                    return Json(ResponseHandler.GetResponse("Failed", "Latitude and Longitude are required", "400", null, ""));

                var Result = await _sessionService.getNearByLocationsService((double)viewModel.Latitude, (double)viewModel.Longitude);
                
                if (Result != null)
                    return Json(ResponseHandler.GetResponse("Success", "Near by users locations.. List total count: " + Result.Count(), "200", Result, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
                else
                    return Json(ResponseHandler.GetResponse("Failed", ConstantMessages.CoordinatesDoesNotExist, "400", null, ""), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null));
            }

        }

        //front end no use
        [HttpGet]
        // [UserAuthorization]
        public async Task<object> GetRecentSessionList_old()
        {
            try
            {
                var opentok = getOpenTokInstance();

                var listArchives = opentok.ListArchives();
                var activeList = listArchives.Where(x => x.Status == ArchiveStatus.AVAILABLE);

                List<LiveStream> finalList = new List<LiveStream>();
                if (activeList.Count() > 0)
                {
                    foreach (var item in activeList)
                    {
                        var data = await _sessionService.getRoomBySessionIdService(item.SessionId);
                        var room = (tbl_Room)data.Body;
                        LiveStream v = new LiveStream();
                        if (room != null)
                        {
                            v.UserId = (int)room.UserId;
                            v.Lat = room.Latitude;
                            v.Lng = room.longitude;
                            v.VideoCategory = room.VideoCategory;
                        }
                        v.SessionId = item.SessionId;
                        v.ArchiveId = item.Id;
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(item.Duration);
                        v.VideoDuration = dateTimeOffset.TimeOfDay;
                        v.VideoURL = item.Url;
                        v.Status = item.Status.ToString();
                        finalList.Add(v);
                    }


                    return Json(new { finalList = finalList }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                }

                //var activeList = List.Where(x => x.Status == ArchiveStatus.AVAILABLE);

                return Json(new { msg = "No recent video found" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ResponseHandler.GetResponse("Exception: ", ex.Message, "404", null, null), System.Web.Mvc.JsonRequestBehavior.AllowGet);
            }

        }

        public OpenTok getOpenTokInstance()
        {
            var apiKey = int.Parse(ConfigurationManager.AppSettings["OTapiKey"]);
            var apiSecret = ConfigurationManager.AppSettings["OTapiSecret"];
            var opentok = new OpenTok(apiKey, apiSecret);
            return opentok;
        }

        public async Task<bool> NotifyAsync(List<string> deviceIds, tbl_Room room, string title, string body)
        {
            try
            {
                // Get the server key from FCM console
                //var serverKey = string.Format("key={0}", "AAAA2iBKcq0:APA91bGEqi4_KzhP_axTPgko2U-SqJ81qqSMimmkeuwG3FuZA9xD2eQMUugZLF6mjAoP1lAtTjjn5vO0eVteOEV7Oq7Qyz_O57rDUJvaxcj0de-3UM_Z-vgG5_ZUgSuugDeiHvRb-Fvk");

                // Get the sender id from FCM console
                //var senderId = string.Format("id={0}", "936844620461");

                string serverKey = string.Format("key={0}", ConfigurationManager.AppSettings["FCM_Server_Key"]);
                string fcm_Url = ConfigurationManager.AppSettings["FCM_URL"].ToString();
                // Get the sender id from FCM console
                string senderId = string.Format("id={0}", ConfigurationManager.AppSettings["FCM_SenderId"]);
            
                var data = new
                {
                    registration_ids = deviceIds, // Recipient device token
                    priority = "high",
                    content_available = true,
                    notification = new { title, body, badge = 1 },
                    data = new { sessionId = room.SessionId, token = room.Token, role = room.Role, videoCategory = room.VideoCategory , userId = room.UserId }
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
