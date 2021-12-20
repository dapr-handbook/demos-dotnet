using Dapr.Actors.Runtime;
using System.Text;

[Actor(TypeName = "MyActor")]
public class ScoreActor : Actor, IScoreActor
{
    public ScoreActor(ActorHost host) : base(host)
    {

    }

    public Task<int> IncrementScoreAsync()
    {
        return StateManager.AddOrUpdateStateAsync("score", 1, (key, currentScore) => currentScore + 1);
    }

    public async Task<int> GetScoreAsync()
    {
        var scoreValue = await StateManager.TryGetStateAsync<int>("score");
        if (scoreValue.HasValue)
        {
            return scoreValue.Value;
        }

        return 0;
    }

}