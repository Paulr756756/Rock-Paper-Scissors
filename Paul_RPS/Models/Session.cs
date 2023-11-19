namespace Paul_RPS.Models;

/// <summary>
/// Session for every user, stores sessionId,
/// match history, statistics and session last active time
/// </summary>
public class Session
{
    public readonly Guid Id = Guid.NewGuid();
    
    public List<Match> Matches = new();

    public Statistics Statistics = new ()
    {
        Wins = 0,
        Losses = 0,
        Draws = 0
    };

    public DateTime LastActivityTime = DateTime.UtcNow;
}