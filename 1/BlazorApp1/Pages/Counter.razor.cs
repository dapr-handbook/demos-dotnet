using Dapr.Client;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp1.Pages
{
    public partial class Counter
    {
        private int _currentCount = 0;

        private IEnumerable<WeatherData> allFlights = new List<WeatherData>();

        [Inject]
        private DaprClient _daprClient { get; set; }

        private async void IncrementCount()
        {
            _currentCount++;

            allFlights = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherData>>(
                HttpMethod.Get,
                "webapi1",
                "weatherforecast");
        }

    }

    public class WeatherData
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }
}
