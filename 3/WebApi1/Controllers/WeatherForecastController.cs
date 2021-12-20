using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace WebApi1.Controllers
{
    [Route("[controller]/[action]")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DaprClient _daprClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }



        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Topic("pubsub", "massage1")]
        [HttpPost]
        public async Task<ActionResult> Sub(WeatherForecast massage)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("reback: " + massage.Date.ToLongTimeString());
            });
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Pub()
        {
            await _daprClient.PublishEventAsync<WeatherForecast>("pubsub", "massage1",
                new WeatherForecast() { Date = DateTime.Now });
            return Ok();
        }
    }
}