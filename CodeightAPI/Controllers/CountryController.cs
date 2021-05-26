using BLL.CountryService;
using BOL.dbContext;
using BOL.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeightAPI.Controllers
{
    public class CountryController : BaseController
    {
        private CountryService _countryService;

        public CountryController()
        {
            _countryService = new CountryService();
        }

        [HttpGet]
        public async Task<string> GetAllCountries()
        {
            try
            {
                var Result = await _countryService.GetAllCountriesServices();
             //   var country = (tbl_Countries) Result.Body;
               
                //return JsonConvert.SerializeObject(Result);
                return JsonConvert.SerializeObject(Result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        [HttpGet]
        public async Task<string> GetAllStates(int CountryId)
        {
            try
            {
                if (CountryId == 0 || CountryId.Equals(null))
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "CountryId Not Found", "404", null, ""));

                var Result = await _countryService.GetAllStatesService(CountryId);
              //  return JsonConvert.SerializeObject(Result);
                return JsonConvert.SerializeObject(Result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }

        [HttpGet]
        public async Task<string> GetAllCities(int StateId)
        {
            try
            {
                if (StateId == 0 || StateId.Equals(null))
                    return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Failed", "StateId Not Found", "404", null, ""));

                var Result = await _countryService.GetAllCitiesService(StateId);
                //  return JsonConvert.SerializeObject(Result);
                return JsonConvert.SerializeObject(Result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(ResponseHandler.GetResponse("Exception", ConstantMessages.SomethingWentWrong, "404", null, ""));
            }
        }


    }
}
