using Dapr.Actors;

public interface IDoNotForgetActor: IActor
{
    Task SetReminderAsync(string text);
}
