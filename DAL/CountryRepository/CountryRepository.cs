using BOL.dbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CountryRepository
{
    public class CountryRepository
    {
        private CodEightEntities _db;

        public CountryRepository()
        {
            _db = new CodEightEntities();
        }


        public async Task<IEnumerable<tbl_Countries>> GetAllCountriesRepo()
        {
            var data = await _db.tbl_Countries.Where(x => x.IsActive == true).ToListAsync();
          
            return data;
        }

        public async Task<IEnumerable<tbl_States>> GetAllStatesRepo(int CountryId)
        {
            var data = await _db.tbl_States.Where(x => x.IsActive == true && x.CountryId == CountryId).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<tbl_Cities>> GetAllCitiesRepo(int StateId)
        {
            var data = await _db.tbl_Cities.Where(x => x.IsActive == true && x.StateId == StateId).ToListAsync();
            return data;
        }
    }
}
