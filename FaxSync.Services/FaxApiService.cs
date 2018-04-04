using FaxSync.Models.FaxApi;
using FaxSync.Services.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using FaxSync.Domain;
using FaxSync.Models;

namespace FaxSync.Services
{
    public class FaxApiService : IFaxApiService
    {
        const string _apiURL = "https://portal.xmedius.com/enterprises/clarkhill/";
        const string _token = "535ffa49e0c63561bfd18a835d3b2245";
        public ApiResult AddUser(int faxNumberId, int faxUserId)
        {
            var client = BuildRestClient();
            var request = new RestRequest("fax_numbers/{faxId}/associate.json", Method.POST);
            request.AddUrlSegment("faxId", faxNumberId);
            request.AddJsonBody(new { user_id = faxUserId });

            IRestResponse<FaxApiRootObject<object>> response = client.Execute<FaxApiRootObject<object>>(request);
            return GenerateResult(response,client);
        }

        public ApiResult RemoveUser(int faxNumberId, int faxUserId)
        {
            var client = BuildRestClient();
            var request = new RestRequest("fax_numbers/{faxId}/disassociate.json", Method.POST);
            request.AddUrlSegment("faxId", faxNumberId);
            request.AddJsonBody(new { user_id = faxUserId });

            IRestResponse<FaxApiRootObject<object>> response = client.Execute<FaxApiRootObject<object>>(request);
            return GenerateResult(response,client);
        }

        public FaxApiRootObject<List<FaxUser>> GetAllUsers()
        {
            var client = BuildRestClient();
            var request = new RestRequest("users.json", Method.GET);

            IRestResponse <FaxApiRootObject<List<FaxUser>>> response = client.Execute<FaxApiRootObject<List<FaxUser>>>(request);
            return response.Data;
        }
        public FaxApiRootObject<List<FaxApiNumber>> GetAllFaxNumbers()
        {
            var client = BuildRestClient();
            var request = new RestRequest("fax_numbers.json", Method.GET);
            IRestResponse<FaxApiRootObject<string>> response = client.Execute<FaxApiRootObject<string>>(request);

            var resultObject = new FaxApiRootObject<List<FaxApiNumber>>();
            resultObject.errors = response.Data.errors;
            resultObject.result = response.Data.result;
            resultObject.data = new List<FaxApiNumber>();

            if (response.Data.result)
            {
                resultObject.data = ExtractFaxNumbersFromResult(response.Data.data);
            }

            return resultObject;
        }

        private List<FaxApiNumber> ExtractFaxNumbersFromResult(string result)
        {
            var root = JObject.Parse(result);
            var faxNumbers = JObject.Parse(root["fax_numbers"].ToString());
            var prop = faxNumbers.Properties().Select(p => p.Name);
            var lstFaxNumbers = new List<FaxApiNumber>();
            foreach (var item in prop)
            {
                var faxObj = new FaxApiNumber();
                faxObj.FaxNumber = faxNumbers[item]["number"].ToString();
                faxObj.FaxNumberId = item.toInt();
                faxObj.Shared = bool.Parse(faxNumbers[item]["shared"].ToString());
                lstFaxNumbers.Add(faxObj);
            }
            return lstFaxNumbers;
        }

        private RestClient BuildRestClient()
        {
            var client = new RestClient(_apiURL);
            client.AddDefaultHeader("Authorization-Token", _token);
            client.AddDefaultHeader("Content-type", "application/json");
            return client;
        }

        private ApiResult GenerateResult(IRestResponse result, RestClient restClient)
        {
            var apiRes = new ApiResult();
            apiRes.ApiCall = result.Request.Resource;
            apiRes.Result = false;
            if (result.IsSuccessful)
            {
                var rez = result as IRestResponse<FaxApiRootObject<object>>;
                if (rez.NotNull())
                {
                    apiRes.Result = rez.Data.result;
                    apiRes.Message = rez.Data.errors?.token;
                }
            }
            else
            {
                apiRes.Message = $"{result.ResponseStatus.ToString()} {result.StatusDescription}";
            }
            apiRes.HttpCallLog = LogRequest(result.Request, result, restClient);
            return apiRes;
            
        }

        private string LogRequest(IRestRequest request, IRestResponse response, RestClient restClient)
        {
            var message = "";
            try
            {
              
               var requestToLog = new
                {
                    resource = request.Resource,
                    // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                    // otherwise it will just show the enum value
                    parameters = request.Parameters.Select(parameter => new
                    {
                        name = parameter.Name,
                        value = parameter.Value,
                        type = parameter.Type.ToString()
                    }),
                    // ToString() here to have the method as a nice string otherwise it will just show the enum value
                    method = request.Method.ToString(),
                    // This will generate the actual Uri used in the request
                    uri = restClient.BuildUri(request),
                };

                var responseToLog = new
                {
                    statusCode = response.StatusCode,
                    content = response.Content,
                    headers = response.Headers,
                    // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                    responseUri = response.ResponseUri,
                    errorMessage = response.ErrorMessage,
                };

                 message = $"Request:{requestToLog.method} {requestToLog.uri.AbsoluteUri} Response:{responseToLog.statusCode} {responseToLog.content} {responseToLog.errorMessage}";
           
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }
       
    }
}
