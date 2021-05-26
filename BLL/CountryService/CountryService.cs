using BOL.Common;
using BOL.dbContext;
using BOL.Helper;
using DAL.CountryRepository;
using DAL.LogRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.CountryService
{
    public class CountryService
    {
        private  CountryRepository _countryRepository;
        private  LogRepository _logRepository;

        public CountryService()
        {
            _countryRepository = new CountryRepository();
            _logRepository = new LogRepository();
        }

        public async Task<ResponseModel> GetAllCountriesServices()
        {
            try
            {
                var data = await _countryRepository.GetAllCountriesRepo();

                var list = data.Select(x => new
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName
                });

                if (list == null)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }
                return ResponseHandler.GetResponse("Success", "Success", "200", list, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "GetAllCountriesServices", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> GetAllStatesService(int CountryId)
        {
            try
            {
                var data = await _countryRepository.GetAllStatesRepo(CountryId);

                var list = data.Select(x => new
                {
                    StateId = x.StateId,
                    StateName = x.StateName,
                    CountryId = x.CountryId,
                });

                if (list.Count() <= 0)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }

                return ResponseHandler.GetResponse("Success", "Success", "200", list, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "GetAllStatesService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }

        public async Task<ResponseModel> GetAllCitiesService(int StateId)
        {
            try
            {
                var data = await _countryRepository.GetAllCitiesRepo(StateId);

                var list = data.Select(x => new
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                    StateId = x.StateId,
                });
                
                if (list.Count() <= 0)
                {
                    return ResponseHandler.GetResponse("Failed", "Record not found", "404", null, null);
                }

                return ResponseHandler.GetResponse("Success", "Success", "200", list, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                await _logRepository.InsertLog(new tbl_Logs() { UserId = null, Operation = "GetAllCitiesService", ErrorDescription = "Exception: " + ex.Message, EventType = EventType.HttpGet.ToString(), CreatedDate = DateTime.Now, CreadedBy = "" });
                return ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, "");
            }
        }
    }
}
