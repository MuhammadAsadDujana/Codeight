using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using BOL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SessionRepository
{
    public class SessionRepository
    {
        private CodEightEntities _db;

        public SessionRepository()
        {
            _db = new CodEightEntities();
        }

        public async Task<tbl_Room> getRoomByUserIdRepo(int userId)
        {
            var room = await _db.tbl_Room.Where(x => x.UserId == userId && x.IsActive == true).FirstOrDefaultAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return room;
        }

        public async Task<tbl_Room> getRoomBySessionIdRepo(string sessionId)
        {
            var room = await _db.tbl_Room.Where(x => x.SessionId == sessionId && x.IsActive == true).FirstOrDefaultAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return room;
        }

        public async Task<IEnumerable<tbl_NearByUser>> getRecentNearByVideoListRepo(int userId)
        {
            var nearByVideoList = await _db.tbl_NearByUser.Where(x => x.roomJoinedUserId == userId && x.IsActive == false).ToListAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return nearByVideoList;
        }

        public async Task<IEnumerable<tbl_Room>> getMySessionListRepo(int userId)
        {
            var baselineDate = DateTime.Now.AddDays(-2);
            var room = await _db.tbl_Room.Where(x => x.UserId == userId && x.IsActive == true && x.TokenIssueDate >= baselineDate).ToListAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return room;
        }

        public async Task<IEnumerable<tbl_NearByUser>> getActiveNearByVideoListRepo(int userId)
        {
            var nearByVideoList = await _db.tbl_NearByUser.Where(x => x.roomJoinedUserId == userId && x.IsActive == true).ToListAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return nearByVideoList;
        }

        public async Task<tbl_NearByUser> getNearByVideoBySessionIdRepo(string sessionId)
        {
            var nearByVideo = await _db.tbl_NearByUser.Where(x => x.PublisherSessionId == sessionId && x.IsActive == true).FirstOrDefaultAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return nearByVideo;
        }

        public async Task<ResponseModel> DisconnectNearByVideoRepo(int roomCreatedUserId)
        {

            if (roomCreatedUserId != null)
            {
                await _db.tbl_NearByUser.Where(x => x.roomCreatedUserId == roomCreatedUserId).ForEachAsync(a => a.IsActive = false);

                //user.IsActive = false;
                //_db.Entry(user).State = EntityState.Modified;
                var user = await _db.SaveChangesAsync();
                return ResponseHandler.GetResponse("Success", "Session disconnected successfully", "200", user, null);
            }

            return ResponseHandler.GetResponse("Failed", "roomCreatedUserId is null", "404", null, null);
        }

        public async Task<tbl_Room> DeleteRoomRepo(tbl_Room room)
        {

            if (room != null)
            {
                room.IsActive = false;
                _db.Entry(room).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }

            return room;
        }

        public async Task<ResponseModel> InsertRoomRepo(tbl_Room entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            // _db.Entry(entity).State = EntityState.Added;
            _db.tbl_Room.Add(entity);
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", "Room inserted successfully", "200", entity, null);
        }

        public async Task<ResponseModel> InsertMarkerRepo(tbl_Markers entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            _db.Entry(entity).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", ConstantMessages.RegistrationSuccess, "200", entity, null);
        }

        public async Task<ResponseModel> UpdatePreviousMarkerRepo(int userId, double? Latitude, double? Longitude)
        {
            if (userId.Equals(null))
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            var entity = await _db.tbl_Markers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if(entity != null)
            {
                entity.Latitude = Latitude;
                entity.Longitude = Longitude;
                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return ResponseHandler.GetResponse("Success", ConstantMessages.CoordinatesUpdated, "200", entity, null);
            }
            else
            {
                tbl_Markers marker = new tbl_Markers
                {
                    Latitude = Latitude,
                    Longitude = Longitude,
                    UserId = userId
                };

                _db.Entry(marker).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return ResponseHandler.GetResponse("Success", ConstantMessages.CoordinatesAdded, "200", entity, null);
            }

          //  return ResponseHandler.GetResponse("Success", ConstantMessages.CoordinatesDoesNotExist, "408", entity, null);
        }

        public async Task<tbl_Markers> VerifyCoordinatesRepo(double lat, double lng, int UserId)
        {
            var marker = await _db.tbl_Markers.Where(x => x.Latitude == lat && x.Longitude == lng && x.UserId == UserId).FirstOrDefaultAsync();
            return marker;
        }

        public async Task<IEnumerable<MarkerViewModel>> getNearByLocationsRepo(double lat, double lng)
        {
            try
            {
                var radiusValue = await _db.tbl_ManageRadius.Where(x => x.IsSelected == true).Select(z => z.RadiusValue).FirstOrDefaultAsync();
                if (radiusValue.HasValue)
                {
                    SqlParameter[] Parameters =
                        {
                            new SqlParameter("@latitude", lat),
                            new SqlParameter("@longitude", lng),
                            new SqlParameter("@radiusValue", radiusValue)
                        };

                    var List = await _db.Database.SqlQuery<MarkerViewModel>("getNearByLocation @latitude, @longitude, @radiusValue", Parameters).ToListAsync();

                    return List;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<MapNearByListViewModel>> getMapClickedSessionListRepo(double lat, double lng)
        {
            try
            {
                var radiusValue = await _db.tbl_ManageRadius.Where(x => x.IsSelected == true).Select(z => z.RadiusValue).FirstOrDefaultAsync();
                if (radiusValue.HasValue)
                {
                    SqlParameter[] Parameters =
                        {
                            new SqlParameter("@latitude", lat),
                            new SqlParameter("@longitude", lng),
                            new SqlParameter("@radiusValue", radiusValue)
                        };

                    var List = await _db.Database.SqlQuery<MapNearByListViewModel>("MapClickedNearBySessionList @latitude, @longitude, @radiusValue", Parameters).ToListAsync();

                    return List;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<tbl_Users> getUserByIdRepo(int Id)
        {
            var user = await _db.tbl_Users.Where(x => x.UserId.Equals(Id) && x.IsActive == true).FirstOrDefaultAsync();
            //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return user;
        }

        public async Task<ResponseModel> UpdateMarkerRepo(tbl_Markers entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", entity, null);

            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", "Updated", "200", entity, null);
        }

        public async Task<ResponseModel> InsertNearByUserRepo(tbl_NearByUser entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            _db.Entry(entity).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", "NearByUser inserted successfully", "200", entity, null);
        }

        public async Task<IEnumerable<tbl_ManageRadius>> GetRadiusListRepo()
        {
            var data = await _db.tbl_ManageRadius.ToListAsync();
            if (data.Count != 0)
                return data;
            else
                return null;
        }

        public async Task<tbl_ManageRadius> GetLastRadiusRepo()
        {
            var data = await _db.tbl_ManageRadius.Where(x => x.IsSelected == true).FirstOrDefaultAsync();
            if (data != null)
                return data;
            else
                return null;
        }

        public async Task<ResponseModel> editRadiusByIdRepo(double radiusValue,int PreviousId)
        {
            if (radiusValue.Equals(null))
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            var radius = await _db.tbl_ManageRadius.Where(x => x.RadiusId == PreviousId).FirstOrDefaultAsync();
            if(radius != null)
            {
                radius.IsSelected = true;
                radius.ModifiedDate = DateTime.Now;
                radius.RadiusValue = radiusValue;
                _db.Entry(radius).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return ResponseHandler.GetResponse("Success", "Updated", "200", null, null);
            }
            else
            {

                tbl_ManageRadius _radius = new tbl_ManageRadius
                {
                    RadiusValue = radiusValue,
                    IsSelected = true,
                    CreatedDate = DateTime.Now,
                };

                _db.Entry(_radius).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return ResponseHandler.GetResponse("Success", "Inserted", "200", null, null);
            }

          //  return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);
        }


    }
}
