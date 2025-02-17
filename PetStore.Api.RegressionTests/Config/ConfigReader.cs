using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace PetStore.Api.RegressionTests.Config
{
    public static class ConfigReader
    {
        private static IConfiguration configuration;

        public static string Get(string key)
        {
            SetConfiguration();

            return configuration.GetValue<string>(key);
        }

        private static void SetConfiguration()
        {
            if (configuration != null) return;

            //Read the configuration properties from the AppConfig.json file
            configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile(@"Config\AppConfig.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static string GetOrThrow(string key)
        {
            var value = Get(key);

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"No value has been found for the {key} in the config file");
            }

            return value;
        }
    }
}