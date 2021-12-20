using Dapr.Actors;

public interface IScoreActor : IActor
{
    Task<int> IncrementScoreAsync();

    Task<int> GetScoreAsync();
}