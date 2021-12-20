using Dapr.Actors;
using Dapr.Actors.Client;

Console.WriteLine("Hello, World!");

var actorId = new ActorId("scoreActor1");

var proxy = ActorProxy.Create<IScoreActor>(actorId, "ScoreActor");

var score = await proxy.IncrementScoreAsync();

Console.WriteLine($"Current score: {score}");
