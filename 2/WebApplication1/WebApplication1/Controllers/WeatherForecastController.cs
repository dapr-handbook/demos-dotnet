using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
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

        [HttpPost]
        public async Task<ActionResult> Save1State()
        {
            var value = Guid.NewGuid().ToString();
            await _daprClient.SaveStateAsync<string>("statestore", "keyPrefix", "zzz");
            return Ok(value);
        }

        [HttpGet]
        public async Task<ActionResult> Get1State()
        {
            var result = await _daprClient.GetStateAsync<string>("statestore", "guid");
            return string.IsNullOrEmpty(result) ? NoContent() : Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete1State()
        {
            await _daprClient.DeleteStateAsync("statestore", "guid");
            return Ok("done");
        }

        [HttpPost]
        public async Task<ActionResult> Save1StateWithTag()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>("statestore", "guid");
            value ??= Guid.NewGuid().ToString();
            await _daprClient.TrySaveStateAsync<string>("statestore", "guid", value + "1", etag);
            return Ok(value);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete1StateWithTag()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>("statestore", "guid");
            var result = await _daprClient.TryDeleteStateAsync("statestore", "guid", etag);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> SaveNState()
        {
            var metadata1 = new Dictionary<string, string>()
            {
                {"a", "b" }
            };
            var options1 = new StateOptions
            {
                Concurrency = ConcurrencyMode.LastWrite
            };
            var requests = new List<StateTransactionRequest>()
            {
                new StateTransactionRequest("value1", Guid.NewGuid().ToByteArray(), StateOperationType.Upsert),
                new StateTransactionRequest("value2", Guid.NewGuid().ToByteArray(), StateOperationType.Delete),
                new StateTransactionRequest("value3", Guid.NewGuid().ToByteArray(), StateOperationType.Upsert, "testEtag", metadata1, options1),
            };

            await _daprClient.ExecuteStateTransactionAsync("statestore", requests);

            return Ok("value1 Upsert, value2 Delete, value3 testEtag.");
        }

        [HttpGet]
        public async Task<ActionResult> GetNState()
        {
            var result = await _daprClient.GetBulkStateAsync("statestore", new List<string> { "value1", "value2", "value3" }, 0);
            return Ok(result);
        }


    }
}