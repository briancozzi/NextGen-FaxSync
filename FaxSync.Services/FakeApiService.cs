using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaxSync.Models;
using FaxSync.Models.FaxApi;

namespace FaxSync.Services
{
    public class FakeApiService : IFaxApiService
    {
        public ApiResult AddUser(int faxNumberId, int faxUserId)
        {
            return FakeApiResult(faxNumberId, faxUserId);
        }

        public ApiResult RemoveUser(int faxNumberId, int faxUserId)
        {
            return FakeApiResult(faxNumberId, faxUserId);
        }

        public FaxApiRootObject<List<FaxApiUser>> GetAllUsers()
        {
            return new FaxApiRootObject<List<FaxApiUser>>()
            {
                data = new List<FaxApiUser>() { new FaxApiUser { username = "pkuchnicki", id = 123123 } },
                result = true,
                errors = null
            };
        }
        public FaxApiRootObject<List<FaxApiNumber>> GetAllFaxNumbers()
        {
            return new FaxApiRootObject<List<FaxApiNumber>>() {
                data = new List<FaxApiNumber>() { new FaxApiNumber { FaxNumber = "+14123942555", FaxNumberId = 15232 } },
                result = true,
                errors = null
            };

        }
        private ApiResult FakeApiResult(int faxNumberId, int faxUserId)
        {
            return new ApiResult() {
                ApiCall =$"https://fakapicall.test/fax/{faxNumberId}/userid/{faxUserId}",
                HttpCallLog = "FakeCallLog",
                Result = true,
                Message = "Fake Massage"
            };

        }
    }
}
