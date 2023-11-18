namespace Paul_RPS.Models;

public class Session
{
    public readonly Guid Id = Guid.NewGuid();

    /*public Session(string sessionId)
    {
        _sessionId = sessionId;
        LastActivityTime = DateTime.UtcNow;
    }*/

    public List<Match> Matches = new();

    public Statistics Statistics = new Statistics()
    {
        Wins = 0,
        Losses = 0,
        Draws = 0
    };

    public DateTime LastActivityTime = DateTime.UtcNow;
}