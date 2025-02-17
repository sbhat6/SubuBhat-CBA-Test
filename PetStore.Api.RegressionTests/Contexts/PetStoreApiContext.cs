using RestSharp;
using System;

namespace PetStore.Api.RegressionTests.Contexts
{
    public class PetStoreApiContext
    {
        public Uri BaseUrl { get; set; }
        public RestRequest Request { get; set; }
        public RestResponse Response { get; set; }
        public RestClient RestClient { get; set; }
        public string UrlPath { get; set; }
    }
}
