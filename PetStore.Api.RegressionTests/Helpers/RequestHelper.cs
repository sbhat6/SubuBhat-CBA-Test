using RestSharp;
using System.Collections.Generic;
using PetStore.Api.RegressionTests.Models;

namespace PetStore.Api.RegressionTests.Helpers
{
    public class RequestHelper
    {
        // OOP concept method overloading is used here for 3 ExecuteRequestAsync methods
        public RestResponse ExecuteRequestAsync(Method httpMethod, string baseUrl, string requestPath, Dictionary<string, string> queryParameters = null, Dictionary<string, string> headers = null)
        {
            var restCLient = BuildRestClient(baseUrl);
            var restRequest = BuildRestRequest(httpMethod, baseUrl, requestPath, headers, queryParameters);

            return SendRequest(restCLient, restRequest, httpMethod);
        }

        public RestResponse ExecuteRequestAsync(Method httpMethod, string baseUrl, string requestPath, PetDetailsFromApiModel requestBody, Dictionary<string, string> queryParameters = null, Dictionary<string, string> headers = null)
        {
            var restCLient = BuildRestClient(baseUrl);
            var restRequest = BuildRestRequest(httpMethod, baseUrl, requestPath, headers, queryParameters);
            restRequest.AddJsonBody(requestBody);

            return SendRequest(restCLient, restRequest, httpMethod);
        }

        public RestResponse ExecuteRequestAsync(Method httpMethod, string baseUrl, string requestPath, string filePath, Dictionary<string, string> queryParameters = null, Dictionary<string, string> headers = null)
        {
            var restCLient = BuildRestClient(baseUrl);
            var restRequest = BuildRestRequest(httpMethod, baseUrl, requestPath, headers, queryParameters);
            restRequest.AddFile("file", filePath);

            return SendRequest(restCLient, restRequest, httpMethod);
        }

        private RestClient BuildRestClient(string baseUrl)
        {
            var restClient = new RestClient(baseUrl);

            return restClient;
        }

        private RestRequest BuildRestRequest(Method httpMethod, string baseUrl, string requestPath,
            Dictionary<string, string> headers, Dictionary<string, string> queryParameters)
        {
            RestRequest restRequest = new RestRequest(requestPath);

            restRequest.Method = httpMethod;

            if (queryParameters == null)
            {
                queryParameters = new Dictionary<string, string>();
            }

            else
            {
                foreach (var parameter in queryParameters)
                {
                    restRequest.AddParameter(parameter.Key, parameter.Value);
                }
            }

            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            else
            {
                foreach (var header in headers)
                {
                    restRequest.AddParameter(header.Key, header.Value);
                }
            }

            return restRequest;
        }

        private RestResponse SendRequest(RestClient restCLient, RestRequest restRequest, Method httpMethod)
        {
            var response = restCLient.ExecuteAsync(restRequest).GetAwaiter().GetResult();

            if (response.ErrorMessage != null)
            {
                LoggingHelper.LogError(string.Format("Error {0} while executing the request: {1}", response.ErrorMessage, restCLient.BuildUri(restRequest)));
            }

            return response;
        }
    }
}