using Data;
using Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Utilities
{
    internal static class DbHelper
    {
        public static void InitializeDbForTests(ApplicationContext db)
        {
            db.Forecasts.AddRange(GetTestForecasts());
            db.SaveChanges();
        }
        public static IEnumerable<WeatherForecast> GetTestForecasts()
        {
            return new WeatherForecast[]
            {
                new WeatherForecast()
                {
                    Id = Guid.Parse("cefa6ffb-ce6e-4197-9c30-81459d072e5a"),
                    TemperatureC = 0
                },

                new WeatherForecast
                {
                    Id = Guid.Parse("adc71289-a52b-4d06-9ffe-ed8d36604f13"),
                    TemperatureC = 1
                },

                new WeatherForecast
                {
                    Id = Guid.Parse("a81e2adf-d628-45a2-ba9d-e30e1a337432"),
                    TemperatureC = 2
                },

                new WeatherForecast
                {
                    Id = Guid.Parse("1fda063f-c6a1-4022-b2e3-6e4d43db4d33"),
                    TemperatureC = 3
                }
            };
        }
    }
}
