using Reqnroll;
using RestSharp;
using System;
using System.Collections.Generic;
using FluentAssertions;
using PetStore.Api.RegressionTests.Models;
using Newtonsoft.Json;
using System.Linq;
using PetStore.Api.RegressionTests.Helpers;
using PetStore.Api.RegressionTests.Contexts;
using PetStore.Api.RegressionTests.Config;
using log4net.Repository.Hierarchy;

namespace PetStore.Api.RegressionTests.StepDefinitions
{
    [Binding]
    public class GetRequestSteps
    {
        private static Dictionary<string, string> petStoreApiHeaders = new();
        private static Dictionary<string, string> petStoreApiQueryParameters = new();
        private readonly PetStoreApiContext _petStoreApiContext;
        private string urlPathSegment1;
        private string urlPathSegment2;

        //OOP concept constructor is used here to initilize the PetStoreApiContext first
        public GetRequestSteps(PetStoreApiContext petStoreApiContext)
        {
            _petStoreApiContext = petStoreApiContext;
        }

        [Given("I am pointed to the PetStore GET Pet By (.*) endpoint")]
        public void GivenIAmPointedToThePetStoreGETPetByIDEndpoint(string getType)
        {
            LoggingHelper.LogInfo($"Building API request for the GET by {getType} endpoint");

            _petStoreApiContext.Request = new RestRequest();
            _petStoreApiContext.Request.Method = Method.Get;

            //Using api-key for authentication is optional. Hence the line that reads it is commented. Uncomment when needed.
            //petStoreApiHeaders.Add("api_key", ConfigReader.GetOrThrow("AuthenticationKey"));

            if (getType.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                urlPathSegment1 = "/pet/";
            }
            else if (getType.Equals("status", StringComparison.OrdinalIgnoreCase))
            {
                urlPathSegment1 = "/pet/findByStatus/";
            }

            _petStoreApiContext.UrlPath = urlPathSegment1;
        }

        [Given("I am provided with a Pet ID (.*) to use")]
        public void GivenIAmProvidedWithAPetIDToUse(string petId)
        {
            urlPathSegment2 = petId;
            _petStoreApiContext.UrlPath = urlPathSegment1 + urlPathSegment2;
        }

        [Given("I am provided with a Pet status (.*)")]
        public void GivenIAmProvidedWithAPetStatusAvailable(string petStatus)
        {
            petStoreApiQueryParameters.Add("status", petStatus);
        }

        [When("I make a GET request to the endpoint")]
        public void WhenIMakeAGETRequestToTheEndpoint()
        {
            LoggingHelper.LogInfo($"Sending the GET request");

            _petStoreApiContext.Response = new RequestHelper().ExecuteRequestAsync(_petStoreApiContext.Request.Method, ConfigReader.GetOrThrow("BaseUrl"), _petStoreApiContext.UrlPath,
                petStoreApiQueryParameters, petStoreApiHeaders);

            LoggingHelper.LogDebug($"Response received." +
                $"Status code: {_petStoreApiContext.Response.StatusCode}. Response: {_petStoreApiContext.Response.Content}");
        }

        [Then("I should receive a successful status code 200 for the GET request")]
        public void ThenIShouldReceiveASuccessfulStatusCodeForTheGETRequest()
        {
            //Assert that Status Code should be OK 
            _petStoreApiContext.Response.StatusCode.ToString().Should().Be("OK");
        }

        [Then("I should receive a error status code 404 for the GET request")]
        public void ThenIShouldReceiveAErrorStatusCodeForTheGETRequest()
        {
            //Assert that Status Code should be NotFound 
            _petStoreApiContext.Response.StatusCode.ToString().Should().Be("NotFound");
        }

        [Then("I should see a valid response for one pet as below")]
        public void ThenIShouldSeeAValidResponseForOnePetAsBelow(DataTable dataTable)
        {
            if (!_petStoreApiContext.Response.IsSuccessful)
            {
                throw new Exception("API call failed: " + _petStoreApiContext.Response.ErrorMessage);
            }

            //Using Reqnroll DataTable helpers to easily map feature file table to its model class
            var expectedPetDetails = dataTable.CreateSet<PetDetailsFromFeatureModel>().First();
            LoggingHelper.LogDebug($"Expected pet info is set for pet ID: {expectedPetDetails.Id}");

            //Deserializing the response content to its model class
            var actualResponseData = JsonConvert.DeserializeObject<PetDetailsFromApiModel>(_petStoreApiContext.Response.Content);
            LoggingHelper.LogDebug($"Received pet info set for pet ID: {expectedPetDetails.Id}");

            AssertPetDetails(expectedPetDetails, actualResponseData);
        }

        [Then("I should see a valid response for multiple pets below")]
        public void ThenIShouldSeeAValidResponseAsForMultiplePetsBelow(DataTable dataTable)
        {
            if (!_petStoreApiContext.Response.IsSuccessful)
            {
                throw new Exception("API call failed: " + _petStoreApiContext.Response.ErrorMessage);
            }

            //Reading all the rows of the feature table into a list of its model class type
            var expectedPetDetails = dataTable.CreateSet<PetDetailsFromFeatureModel>().ToList();

            //Similarly, storing each element of the JSON array of the response into a list of its model class type
            var actualResponseData = JsonConvert.DeserializeObject<List<PetDetailsFromApiModel>>(_petStoreApiContext.Response.Content);

            foreach (var expected in expectedPetDetails)
            {
                //Endpoint may return additional elements for which we do not know the expected data. Hence finding only required elements.
                var actual = actualResponseData.Find(u => u.Id == expected.Id);

                actual.Should().NotBeNull($"Pet '{expected.Id}' should be present in the API response.");

                AssertPetDetails(expected, actual);
            }
        }

        private void AssertPetDetails(PetDetailsFromFeatureModel expected, PetDetailsFromApiModel actual)
        {
            // Assert that Pet Id matches
            actual.Id.Should().Be(expected.Id, "because the Id should match between the table and the API response");

            // Assert that Category Id and Name match
            actual.Category.Id.Should().Be(expected.CategoryId, "because the Category Id should match");
            actual.Category.Name.Should().Be(expected.CategoryName, "because the Category Name should match");

            // Assert that Name matches
            actual.Name.Should().Be(expected.Name, "because the Name should match");

            // Assert that PhotoUrls match
            actual.PhotoUrls[0].Should().BeEquivalentTo(expected.PhotoUrls, "because the PhotoUrls should match");

            // actual that Tags Id and Name match
            actual.Tags[0].Id.Should().Be(expected.TagsId, $"because the Tag Id at index {0} should match");
            actual.Tags[0].Name.Should().Be(expected.TagsName, $"because the Tag Name at index {0} should match");

            // Assert that Status matches
            actual.Status.Should().Be(expected.Status, "because the Status should match");
        }

        [Then("I should see the error message as below")]
        public void ThenIShouldSeeTheErrorMessageAsBelow(DataTable dataTable)
        {
            var expectedErrorResponse = dataTable.CreateSet<PetValidationResponseModel>().First();

            var errorResponse = JsonConvert.DeserializeObject<PetValidationResponseModel>(_petStoreApiContext.Response.Content);

            expectedErrorResponse.Should().BeEquivalentTo(errorResponse);
        }
    }
}