using Dapr.Actors.Runtime;
using System.Text;

public class DoNotForgetActor : Actor, IDoNotForgetActor, IRemindable
{
    public DoNotForgetActor(ActorHost host) : base(host)
    {
    }

    public Task SetReminderAsync(string text)
    {
        return RegisterReminderAsync(
            "DoNotForget",
            Encoding.UTF8.GetBytes(text),
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));
    }

    public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        if (reminderName == "DoNotForget")
        {
            var text = Encoding.UTF8.GetString(state);

            Logger.LogInformation($"Don't forget: {text}");
        }

        return Task.CompletedTask;
    }

}