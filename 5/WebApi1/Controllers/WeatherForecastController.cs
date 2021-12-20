using Dapr.Actors;
using Dapr.Actors.Client;
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
        private readonly IActorProxyFactory _actorProxyFactory;
        private static string? _key;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DaprClient daprClient, IActorProxyFactory actorProxyFactory)
        {
            _logger = logger;
            _daprClient = daprClient;
            _actorProxyFactory = actorProxyFactory;
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

        [HttpPut("{scoreId}")]
        public Task<int> IncrementAsync(string scoreId)
        {
            _key = scoreId;
            var scoreActor = _actorProxyFactory.CreateActorProxy<IScoreActor>(new ActorId(_key), "MyActor");
            return scoreActor.IncrementScoreAsync();
        }

        [HttpGet]
        public Task<int> GetScoreAsync()
        {
            var scoreActor = _actorProxyFactory.CreateActorProxy<IScoreActor>(new ActorId(_key), "MyActor");
            return scoreActor.GetScoreAsync();
        }

        [HttpGet]
        public Task StartTimerAsync()
        {
            var timerActor = _actorProxyFactory.CreateActorProxy<ITimerActor>(ActorId.CreateRandom(), nameof(TimerActor));
            return timerActor.StartTimerAsync("mytimer", "hello timer");
        }



        [HttpGet]
        public Task DoNotForgetMeAsync()
        {
            var donotActor = _actorProxyFactory.CreateActorProxy<IDoNotForgetActor>(ActorId.CreateRandom(), nameof(DoNotForgetActor));
            return donotActor.SetReminderAsync("me");
        }


    }
}