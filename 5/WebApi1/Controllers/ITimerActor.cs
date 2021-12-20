using Dapr.Actors;

public interface ITimerActor : IActor
{
    Task StartTimerAsync(string name, string text);
}
