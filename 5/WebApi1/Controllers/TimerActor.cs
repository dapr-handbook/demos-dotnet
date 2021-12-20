using Dapr.Actors.Runtime;
using System.Text;

public class TimerActor : Actor, ITimerActor
{
    public TimerActor(ActorHost host) : base(host)
    {
    }

    public Task StartTimerAsync(string name, string text)
    {
        return RegisterTimerAsync(
            name,
            nameof(Timer2LoggerAsync),
            Encoding.UTF8.GetBytes(text),
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));
    }

    public Task Timer2LoggerAsync(byte[] state)
    {
        var text = Encoding.UTF8.GetString(state);

        Logger.LogInformation($"Timer fired: {text}");

        return Task.CompletedTask;
    }

    public Task Stop2LoggerAsync(string name)
    {
        return UnregisterTimerAsync(name);
    }
}
