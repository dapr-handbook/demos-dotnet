using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [HttpPost("/mybinding")]
        public ActionResult MsgIn([FromBody] WeatherForecast data)
        {
            Console.WriteLine($"message received at {data.Date.ToLongTimeString()}.");
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> MsgOut()
        {
            await _daprClient.InvokeBindingAsync("mybinding", "create", new WeatherForecast { Date = DateTime.Now });
            return Ok();
        }

    }
}