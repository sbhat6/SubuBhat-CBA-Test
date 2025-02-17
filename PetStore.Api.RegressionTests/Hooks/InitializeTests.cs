using Newtonsoft.Json;
using Reqnroll;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using PetStore.Api.RegressionTests.Config;
using PetStore.Api.RegressionTests.Helpers;
using PetStore.Api.RegressionTests.Models;
using System.Diagnostics;

namespace PetStore.Api.RegressionTests.Hooks
{
    [Binding]
    public class InitializeTests
    {
        [BeforeTestRun()]
        public static void SetTestDataForGetEndpoints()
        {
            // Get test data file path
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestData\SetTestData\", "PetTestData.json");

            // Read the test data file content
            string jsonFile = File.ReadAllText(filePath);

            // Deserialize JSON array in the test data file to a list of objects
            List<PetDetailsFromApiModel> petTestData = JsonConvert.DeserializeObject<List<PetDetailsFromApiModel>>(jsonFile);

            // POST test data to be use for GET tests
            foreach (var pet in petTestData)
            {
                RestClient testDataClient = new RestClient(ConfigReader.GetOrThrow("BaseUrl"));
                RestRequest testDataRequest = new RestRequest("/pet/");
                testDataRequest.Method = Method.Post;
                testDataRequest.AddJsonBody(pet);

                var tesDataResponse = testDataClient.ExecuteAsync(testDataRequest).GetAwaiter().GetResult();

                if (tesDataResponse.ErrorMessage != null)
                {
                    LoggingHelper.LogError(string.Format("Error {0} while executing the request: {1}", tesDataResponse.ErrorMessage, testDataClient.BuildUri(testDataRequest)));
                }
            }
        }

        [AfterTestRun()]
        public static void DeleteTestDataForGetEndpoints()
        {
            //List of the pet IDs used in all the tests
            List<string> petTestIds = new List<string> { "2241", "2242", "2243", "2244", "2245" };

            foreach (var pet in petTestIds)
            {
                RestClient testDataClient = new RestClient(ConfigReader.GetOrThrow("BaseUrl"));
                RestRequest testDataRequest = new RestRequest("/pet/{petTestId}");
                testDataRequest.AddUrlSegment("petTestId", pet);
                testDataRequest.Method = Method.Delete;

                var tesDataResponse = testDataClient.ExecuteAsync(testDataRequest).GetAwaiter().GetResult();

                if (tesDataResponse.ErrorMessage != null)
                {
                    LoggingHelper.LogError(string.Format("Error {0} while executing the request: {1}", tesDataResponse.ErrorMessage, testDataClient.BuildUri(testDataRequest)));
                }
            }
        }

        [AfterTestRun]
        public static void GenerateAllureReport()
        {
            // Generate Allure Report Automatically After Tests

            string allureResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-results");
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string reportsBasePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
            string reportsDirectory = Path.Combine(reportsBasePath, "Reports");

            //Creates each test report with current timestamp folder under the 'Reports' folder.
            string reportDir = Path.Combine(reportsDirectory, timestamp);
            Directory.CreateDirectory(reportDir);

            // Run Allure generate command with the updated paths
            Process.Start("cmd.exe", $"/C allure generate \"{allureResultsPath}\" --clean -o \"{reportDir}");
        }
    }
}