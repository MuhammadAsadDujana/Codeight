using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.LogRepository
{
    public class LogRepository
    {
        private CodEightEntities _db;

        public LogRepository()
        {
            _db = new CodEightEntities();
        }


        public async Task<ResponseModel> InsertLog(tbl_Logs entity)
        {
            if (entity == null)
                return ResponseHandler.GetResponse("Failed", "Entity is null", "404", null, null);

            _db.Entry(entity).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return ResponseHandler.GetResponse("Success", "Log Inserted", "200", entity, null);
        }
        public ResponseModel Delete(tbl_Logs entity)
        {
            throw new NotImplementedException();
        }

        public tbl_Logs Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<tbl_Logs> GetAll()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public ResponseModel Update(tbl_Logs entity)
        {
            throw new NotImplementedException();
        }

    }
}
