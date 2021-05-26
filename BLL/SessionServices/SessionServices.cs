using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using DAL.LogRepository;
using DAL.SessionRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SessionServices
{
    public class SessionServices
    {
        private SessionRepository _sessionRepository;
        private LogRepository _logRepository;
        public SessionServices()
        {
            _sessionRepository = new SessionRepository();
            _logRepository = new LogRepository();
        }

        public async Task<ResponseModel> getRoomByUserIdService(int Id)
        {
            try
            {
                var room = await _sessionRepository.getRoomByUserIdRepo(Id);
                if (room == null)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }

                return ResponseHandler.GetResponse("Success", "Get room", "200", room, room.SessionId);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "getRoomByUserIdService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }


        public async Task<ResponseModel> getRoomBySessionIdService(string sessionId)
        {
            try
            {
                var room = await _sessionRepository.getRoomBySessionIdRepo(sessionId);
                if (room == null)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }

                return ResponseHandler.GetResponse("Success", "Get room", "200", room, room.SessionId);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "getRoomBySessionIdService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<IEnumerable<tbl_NearByUser>> getRecentNearByVideoListService(int userId)
        {
            try
            {
                var model = await _sessionRepository.getRecentNearByVideoListRepo(userId);

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<tbl_Room>> getMySessionListService(int userId)
        {
            try
            {
                var model = await _sessionRepository.getMySessionListRepo(userId);

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }



        public async Task<IEnumerable<tbl_NearByUser>> getActiveNearByVideoListService(int userId)
        {
            try
            {
                var model = await _sessionRepository.getActiveNearByVideoListRepo(userId);

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<tbl_NearByUser> getNearByVideoBySessionIdService(string sessionId)
        {
            try
            {
                var model = await _sessionRepository.getNearByVideoBySessionIdRepo(sessionId);

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<ResponseModel> DisconnectNearByVideoService(int roomCreatedUserId)
        {
            try
            {
                var data = await _sessionRepository.DisconnectNearByVideoRepo(roomCreatedUserId);
                return ResponseHandler.GetResponse("Success", "Session disconnected", "200", data, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "DisconnectNearByVideoService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> DeleteRoomService(tbl_Room room)
        {
            try
            {
                var data = await _sessionRepository.DeleteRoomRepo(room);
                return ResponseHandler.GetResponse("Success", "Room deleted", "200", data, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "DeleteRoomRepoService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> InsertRoomService(tbl_Room room)
        {
            try
            {
                var response = await _sessionRepository.InsertRoomRepo(room);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", "Room inserted successfully", "200", room, null);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", "Insertion failed", "408", room, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "InsertRoomService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }
    

        public async Task<ResponseModel> InsertMarkerService(tbl_Markers marker)
        {
            try
            {
                var response = await _sessionRepository.InsertMarkerRepo(marker);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", ConstantMessages.RegistrationSuccess, "200", marker, null);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.RegistrationFailed, "408", marker, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "InsertMarkerService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> UpdatePreviousMarkerService(int userId, double? Latitude, double? Longitude)
        {
            try
            {
                var response = await _sessionRepository.UpdatePreviousMarkerRepo(userId, Latitude, Longitude);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", response.Message, "200", response, null);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.CoordinatesDoesNotExist, "408", response, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "DeletePreviousMarkerService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> VerifyCoordinatesService(double lat, double lng, int UserId)
        {
            try
            {
                var marker = await _sessionRepository.VerifyCoordinatesRepo(lat, lng, UserId);
                if (marker != null)
                    return ResponseHandler.GetResponse("Success", "Verified coordinates", "200", marker, null);

                return ResponseHandler.GetResponse("Exception", ConstantMessages.CoordinatesDoesNotExist, "404", null, ""); 
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "VerifyCoordinatesService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }


        public async Task<ResponseModel> getUserByIdService(int Id)
        {
            try
            {
                var user = await _sessionRepository.getUserByIdRepo(Id);
                if (user == null)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }

                return ResponseHandler.GetResponse("Success", "Get User Profile", "200", user, user.AccessToken);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "getUserByIdService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<IEnumerable<MarkerViewModel>> getNearByLocationsService(double lat, double lng)
        {
            try
            {
                var model = await _sessionRepository.getNearByLocationsRepo(lat, lng);

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<MapNearByListViewModel>> getMapClickedSessionListService(double lat, double lng)
        {
            try
            {
                var model = await _sessionRepository.getMapClickedSessionListRepo(lat, lng);

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        //public async Task<ResponseModel> getNearByLocationsService(double lat, double lng)
        //{
        //    try
        //    {
        //        var model = await _sessionRepository.getNearByLocationsRepo(lat, lng);

        //        if (model == null)
        //            return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);

        //        return ResponseHandler.GetResponse("Success", "Get near by users list", "200", model, null);

        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.TraceError(ex.Message);
        //        return null;
        //    }
        //}


        public async Task<ResponseModel> UpdateMarkerService(tbl_Markers markers)
        {
            try
            {
                var response = await _sessionRepository.UpdateMarkerRepo(markers);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", "Marker updated", "200", response, response.AccessToken);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", ConstantMessages.RegistrationFailed, "408", response, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "UpdateMarkerService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> InsertNearByUserService(tbl_NearByUser room)
        {
            try
            {
                var response = await _sessionRepository.InsertNearByUserRepo(room);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", "NearByUser inserted successfully", "200", room, null);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", "Insertion failed", "408", room, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "InsertNearByUserService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<IEnumerable<tbl_ManageRadius>> GetRadiusListService()
        {
            try
            {
                var model = await _sessionRepository.GetRadiusListRepo();

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<tbl_ManageRadius> GetLastRadiusService()
        {
            try
            {
                var model = await _sessionRepository.GetLastRadiusRepo();

                if (model != null)
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        public async Task<ResponseModel> editRadiusByIdService(double radiusValue,int PreviousId)
        {

            try
            {
                var response = await _sessionRepository.editRadiusByIdRepo(radiusValue, PreviousId);
                if (response.Code == "200")
                {
                    return ResponseHandler.GetResponse("Success", "Edited", "200", response, null);
                }
                else
                {
                    return ResponseHandler.GetResponse("Failed", "Entity is null", "404", response, null);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "editRadiusByIdService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpPost.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }


        }

    }
}
