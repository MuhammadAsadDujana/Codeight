using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UserRepository
{
    public class UserRepository
    {
        private CodEightEntities _db;

        public UserRepository()
        {
            _db = new CodEightEntities();
        }

        public async Task<ResponseModel> InsertUserRepo(tbl_Users entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            _db.Entry(entity).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", ConstantMessages.RegistrationSuccess, "200", entity, null);
        }

        public async Task<IEnumerable<tbl_Users>> getAllUsersRepo()
        {
            try
            {
                var data = await _db.tbl_Users.Where(x => x.IsActive == true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<ResponseModel> UpdateUserRepo(tbl_Users entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", entity, null);

            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", "Updated", "200", entity, null);
        }

        public async Task<tbl_Users> getUserByIdRepo(int Id)
        {
            var user = await _db.tbl_Users.Where(x => x.UserId.Equals(Id) && x.IsActive == true).FirstOrDefaultAsync();
         //   var country = user.tbl_Cities.tbl_States.tbl_Countries.CountryName;
            return user;
        }

        public async Task<tbl_Users> VerifyEmailRepo(string EmailAddress)
        {
            var user = await _db.tbl_Users.Where(x => x.IsActive == true && x.Email == EmailAddress).FirstOrDefaultAsync();
            return user;
        }

        public async Task<ResponseModel> VerifyForgotPasswordLinkRepo(string Link)
        {
            var data = await _db.tbl_ForgotPasswordLinks.Where(x => x != null && x.Link.Equals(Link) && x.IsActive == true).FirstOrDefaultAsync();
            return ResponseHandler.GetResponse("Success", "verify", "200", data, null);
        }

        public async Task<ResponseModel> InsertForgotLinkRepo(tbl_ForgotPasswordLinks entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            _db.Entry(entity).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", "Inserted", "200", entity, null);
        }

        public async Task<ResponseModel> ResetPasswordRepo(string Link, string NewPass)
        {
            var AssociatedLink = await _db.tbl_ForgotPasswordLinks.Where(x => x != null && x.Link.Equals(Link) && x.IsActive == true).FirstOrDefaultAsync();
            if (AssociatedLink != null)
            {
                if (AssociatedLink.ExpiryDate > DateTime.Now)
                {
                    var AssociatedUser = await getUserByIdRepo((int)AssociatedLink.UserId);
                    AssociatedUser.Password = Encryption.EncodePasswordToBase64(NewPass);
                    _db.Entry(AssociatedUser).State = EntityState.Modified;

                    AssociatedLink.IsActive = false;
                    _db.Entry(AssociatedLink).State = System.Data.Entity.EntityState.Modified;
                    await _db.SaveChangesAsync();

                    return new ResponseModel { Code = "200", Status = "Success", Message = "Your password has been reset successfully." };
                }
                else
                {
                    return new ResponseModel { Code = "400", Status = "Failed", Message = "Your password reset link has expired." };
                }
            }
            else
            {
                return new ResponseModel { Code = "400", Status = "Failed", Message = "Reset password link is invalid." };
            }
        }

        public string GetUserByNameRepo(string email)
        {
            string role = _db.tbl_Users.Where(x => x.Email == email && x.IsActive == true).FirstOrDefault().UserType.ToString();
            return role;
        }

        public async Task<tbl_Users> DeleteUserByIdRepo(int Id)
        {
            var user = await _db.tbl_Users.Where(x => x.UserId == Id && x.IsActive == true).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsActive = false;
                _db.Entry(user).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }

            return user;
        }

    }
}
