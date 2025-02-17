using Reqnroll;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PetStore.Api.RegressionTests.Helpers;
using Newtonsoft.Json;
using FluentAssertions;
using PetStore.Api.RegressionTests.Config;
using PetStore.Api.RegressionTests.Contexts;
using PetStore.Api.RegressionTests.Models;

namespace PetStore.Api.RegressionTests.StepDefinitions
{
    [Binding]
    public class PostRequestSteps
    {
        private static Dictionary<string, string> petStoreApiHeaders = new();
        private static Dictionary<string, string> petStoreApiQueryParameters = new();
        private readonly PetStoreApiContext _petStoreApiContext;
        private PetDetailsFromApiModel _postPetRequestBody;
        private string requestType;
        private string petImageFilePath;
        private string petImageFileName;
        private string petImageFileSize;
        private string urlPathSegment1;
        private string urlPathSegment2;
        private string urlPathSegment3;

        //OOP concept constructor is used here to initilize the PetStoreApiContext first
        public PostRequestSteps(PetStoreApiContext petStoreApiContext)
        {
            _petStoreApiContext = petStoreApiContext;
        }

        [Given("I am pointed to the PetStore POST add new (.*) endpoint")]
        public void GivenIAmPointedToThePetStorePOSTAddNewImageEndpoint(string postType)
        {
            LoggingHelper.LogInfo($"Building API request for the POST by {postType} endpoint");

            requestType = postType;

            _petStoreApiContext.Request = new RestRequest();
            _petStoreApiContext.Request.Method = Method.Post;

            if (requestType.Equals("pet", StringComparison.OrdinalIgnoreCase))
            {
                urlPathSegment1 = "/pet/";
                _petStoreApiContext.UrlPath = urlPathSegment1;

            }
            else if (requestType.Equals("image", StringComparison.OrdinalIgnoreCase))
            {
                urlPathSegment1 = "/pet/";
                urlPathSegment3 = "/uploadImage/";
            }
        }

        [Given("I am provided with a Pet ID (.*) and the image filename (.*) and image file size (.*)")]
        public void GivenIAmProvidedWithAPetIDAndTheImageFilenamePet__Dog_JpgAndImageFileSize(string petId, string fileName, string fileSize)
        {
            petImageFileName = fileName;
            petImageFileSize = fileSize;

            petImageFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestData\PetImages\", fileName);
            _petStoreApiContext.Request.AddFile("file", petImageFilePath);

            urlPathSegment2 = petId;

            _petStoreApiContext.UrlPath = urlPathSegment1 + urlPathSegment2 + urlPathSegment3;
        }


        [Given("I am provided with the pet data as below")]
        public void GivenIAmProvidedWithThePetDataAsBelow(DataTable dataTable)
        {

            var petDetails = dataTable.CreateSet<PetDetailsFromFeatureModel>().First();

            //Forming the request body from the test data
            _postPetRequestBody = new PetDetailsFromApiModel
            {
                Id = petDetails.Id,
                Category = new Category
                {
                    Id = petDetails.CategoryId,
                    Name = petDetails.CategoryName
                },
                Name = petDetails.Name,
                PhotoUrls = new List<string> { petDetails.PhotoUrls },
                Tags = new List<Tag>
                {
                    new Tag { Id = petDetails.TagsId, Name = petDetails.TagsName }
                },
                Status = petDetails.Status
            };

            LoggingHelper.LogDebug($"Request body is formed as {JsonConvert.SerializeObject(_postPetRequestBody, Formatting.Indented)}");
        }

        [When("I make a POST request to the endpoint")]
        public void WhenIMakeAPOSTRequestToTheEndpoint()
        {
            LoggingHelper.LogInfo($"Sending the GET request");

            if (requestType.Equals("pet", StringComparison.OrdinalIgnoreCase))
            {
                _petStoreApiContext.Response = new RequestHelper().ExecuteRequestAsync(_petStoreApiContext.Request.Method, ConfigReader.GetOrThrow("BaseUrl"), _petStoreApiContext.UrlPath,
                _postPetRequestBody, petStoreApiQueryParameters, petStoreApiHeaders);
            }
            else if (requestType.Equals("image", StringComparison.OrdinalIgnoreCase))
            {
                _petStoreApiContext.Response = new RequestHelper().ExecuteRequestAsync(_petStoreApiContext.Request.Method, ConfigReader.GetOrThrow("BaseUrl"), _petStoreApiContext.UrlPath,
                petImageFilePath, petStoreApiQueryParameters, petStoreApiHeaders);
            }

            LoggingHelper.LogDebug($"Response received." + 
                $"Status code: {_petStoreApiContext.Response.StatusCode}. Response: {_petStoreApiContext.Response.Content}");

            if (!_petStoreApiContext.Response.IsSuccessful)
            {
                throw new Exception("API call failed: " + _petStoreApiContext.Response.ErrorMessage);
            }
        }

        [Then("I should receive a successful status code 200 for the POST request")]
        public void ThenIShouldReceiveASuccessfulStatusCodeForThePOSTRequest()
        {
            //Assert that Status Code should be OK 
            _petStoreApiContext.Response.StatusCode.ToString().Should().Be("OK");
        }

        [Then("I should see a valid response for file upload as below")]
        public void ThenIShouldSeeAValidResponseForFileUploadAsBelow(DataTable dataTable)
        {
            var row = dataTable.Rows.First();

            PetValidationResponseModel expectedPetImageUploadResponse = new PetValidationResponseModel();

            expectedPetImageUploadResponse.Code = int.Parse(row["Code"]);
            expectedPetImageUploadResponse.Type = row["Type"];
            string rawMessage = row["Message"];

            //Modifying expected response based on test data
            expectedPetImageUploadResponse.Message = IncludeFileDetailsIntheMessage(rawMessage);

            //Deserializing the response content to its model class
            var actualResponseData = JsonConvert.DeserializeObject<PetValidationResponseModel>(_petStoreApiContext.Response.Content);

            //Assert that every fields match between expected response and actual response
            expectedPetImageUploadResponse.Should().BeEquivalentTo(actualResponseData);
        }

        //This method will dynamically place the filename and file size in the expected response message
        private string IncludeFileDetailsIntheMessage(string message)
        {
            // Split the string at the comma, then take the last word before the comma
            string updatedMessageWithFileName = message.Replace(message.Split(',')[0].Trim().Split(' ')[^1], "./" + petImageFileName);


            // Split the string at the comma, then take the last word after the comma
            string updatedMessageWithFileSize = updatedMessageWithFileName.Replace(updatedMessageWithFileName.Split(',')[1].Trim().Split(' ')[0], petImageFileSize);

            return updatedMessageWithFileSize;
        }
    }
}