using Paul_RPS.Models;

namespace Paul_RPS.Data;

public interface IDataAccess
{
    void SaveSession(Session session);
    void DeleteSession(Guid sessionId);
    List<Match> GetAllMatches(Guid sessionId);
    Statistics GetStats(Guid sessionId);
    void AddMatchData(Match match, Guid sessionId);
    void ClearMemory();
}

public class DataAccess : IDataAccess
{
    private Dictionary<Guid, Session> _sessionsInMemory = new ();
    private readonly Timer sessionCleanupTimer;
    private ILogger<DataAccess> _logger;

    public DataAccess(ILogger<DataAccess> logger)
    {
        _logger = logger;
        
        //Timer for session cleanup
        sessionCleanupTimer = new Timer(PerformCleanupOperations, null, TimeSpan.Zero, TimeSpan.FromHours(1));
    }

    /// <summary>
    /// Saves session in the dictionary
    /// </summary>
    /// <param name="session">Session</param>
    public void SaveSession(Session session)
    {
        if(!_sessionsInMemory.TryAdd(session.Id, session)) 
            _logger.LogError($"Error: Session with {session.Id} already present in memory!");
        else _logger.LogInformation($"New session created with id: {session.Id}.");
    }

    /// <summary>
    /// Removes the session from the dictionary
    /// </summary>
    /// <param name="sessionId">The session id</param>
    /// <exception cref="NotImplementedException">When session is not found</exception>
    public void DeleteSession(Guid sessionId)
    {
        if (!_sessionsInMemory.ContainsKey(sessionId))
        {
            _logger.LogError($"No such session in memory with id:{sessionId}");
            throw new NotImplementedException("Handle Session not found");
        }
        _sessionsInMemory.Remove(sessionId);
    }

    /// <summary>
    /// Gets the list of matches in the session
    /// </summary>
    /// <param name="sessionId">The session id</param>
    /// <returns>List of matches</returns>
    public List<Match> GetAllMatches(Guid sessionId)
    {
        var session = _sessionsInMemory.GetValueOrDefault(sessionId);
        if(session is null) _logger.LogError("No such session present");
        UpdateLastActivityTime(sessionId);
        return session!.Matches;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Statistics GetStats(Guid sessionId)
    {
        var session = _sessionsInMemory.GetValueOrDefault(sessionId);
        if (session is null)
        {
            _logger.LogError($"Session not found with ID: {sessionId}");
            throw new NotImplementedException("Handle SessionNotFound");
        }
        UpdateLastActivityTime(sessionId);
        return session.Statistics;
    }
    
    /// <summary>
    /// Add a new match to the session
    /// </summary>
    /// <param name="match">Match</param>
    /// <param name="sessionId">The session id</param>
    /// <exception cref="NotImplementedException">When session is not found</exception>
    public void AddMatchData(Match match, Guid sessionId)
    {
        var session = _sessionsInMemory.GetValueOrDefault(sessionId);
        if (session is null)
        {
            _logger.LogError($"Session not found with ID: {sessionId}");
            throw new NotImplementedException("Handle SessionNotFound");
        }        
        session.Matches.Add(match);
        if (match.UserWon) session.Statistics.Wins++;
        else if (match.IsDraw) session.Statistics.Draws++;
        else session.Statistics.Losses++;
        UpdateLastActivityTime(sessionId);
    }

    /// <summary>
    /// Clears the dictionary
    /// </summary>
    public void ClearMemory()
    {
        _sessionsInMemory = new();
    }
    
    /// <summary>
    /// Updates the last activity time
    /// </summary>
    /// <param name="sessionId">The id of the session</param>
    /// <exception cref="NotImplementedException">When session is not found</exception>
    public void UpdateLastActivityTime(Guid sessionId)
    {
        var session = _sessionsInMemory.GetValueOrDefault(sessionId);
        if (session is null)
        {
            _logger.LogInformation($"Couldn't get Session with id {sessionId}");
            throw new NotImplementedException("Handle session not found");
        }

        session.LastActivityTime = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Remove sessions that are inactive for more than a day
    /// </summary>
    /// <param name="state">Object</param>
    private void PerformCleanupOperations(object? state)
    {
        var current = DateTime.UtcNow;
        var expiredSessions = _sessionsInMemory.Where(pair =>
            current > pair.Value.LastActivityTime.AddDays(1));
        foreach (var session in expiredSessions)
        {
            _sessionsInMemory.Remove(session.Key);
        }
    }
}
