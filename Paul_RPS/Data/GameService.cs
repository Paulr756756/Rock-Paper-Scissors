using System.Diagnostics;
using Paul_RPS.Models;
using Paul_RPS.Models.RequestModels;

namespace Paul_RPS.Data;

public interface IGameService
{
    public Session CreateSession();
    Match ProcessMatch(SendActionRM request);
    List<Match> GetPreviousMatches(Guid sessionId);
    Statistics GetCurrentSessionStats(Guid sessionId);
    void TerminateSession(Guid sessionId);
    void ClearMemory();
}

public class GameService : IGameService
{
    private readonly IDataAccess _dataAccess;
    private readonly ILogger<GameService> _logger;
    private Random _randomizer = new Random();

    public GameService(IDataAccess dataAccess, ILogger<GameService> logger)
    {
        _dataAccess = dataAccess;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new session
    /// </summary>
    /// <returns>Session</returns>
    public Session CreateSession()
    {
        var session = new Session();
        _dataAccess.SaveSession(session);
        return session;
    }
    
    /// <summary>
    /// Processes users action request and creates new match.
    /// </summary>
    /// <param name="request">The SendAction Response Model</param>
    /// <returns>Match</returns>
    public Match ProcessMatch(SendActionRM request)
    {
        var userAction = (Action)request.Action;
        var computerAction = GenerateComputerAction(request.SessionId);
        var didUserWin = false;
        
        switch (userAction)
        {
            case Action.Rock:
                didUserWin = computerAction == Action.Scissor;
                break;
            case Action.Paper:
                didUserWin = computerAction == Action.Rock;
                break;
            case Action.Scissor:
                didUserWin = computerAction == Action.Paper;
                break;
        }
        
        var match = new Match()
        {
            UserAction = userAction,
            ComputerAction = computerAction,
            UserWon = didUserWin,
            IsDraw = computerAction == userAction,
            //SessionId = request.SessionId
        };
        
        _dataAccess.AddMatchData(match, request.SessionId);
        return match;
    }
    
    /// <summary>
    /// Would generate a computer action in response of user's action
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Action GenerateComputerAction(Guid sessionId)
    {
        var randomNumber = _randomizer.Next(1, 4);

        /*Get the most frequent action and if not null,
         * choose randomly between that action or the one generated randomly*/
        var matches = GetPreviousMatches(sessionId);
        var mostFrequentAction = GetMostFrequentAction(matches);
        if (mostFrequentAction is not null)
        {
            randomNumber = (_randomizer.Next(2)==0)?(int)mostFrequentAction.Value: randomNumber;
        }
        
        var action = randomNumber switch
        {
            1 => Action.Rock,
            2 => Action.Paper,
            3 => Action.Scissor,
            _ => throw new NotImplementedException("Handle Default switch cases")
        };
        return action;
    }

    /// <summary>
    /// Returns all the previous matches
    /// </summary>
    /// <param name="sessionId">The session Id</param>
    /// <returns>List of Matches</returns>
    public List<Match> GetPreviousMatches(Guid sessionId)
    {
        return _dataAccess.GetAllMatches(sessionId);
    }

    /// <summary>
    /// Get Statistics
    /// </summary>
    /// <param name="sessionId">Current session ID</param>
    /// <returns>Statistics</returns>
    public Statistics GetCurrentSessionStats(Guid sessionId)
    {
        return _dataAccess.GetStats(sessionId);
    }
    
    /// <summary>
    /// Terminates the session
    /// </summary>
    /// <param name="sessionId">The session ID</param>
    public void TerminateSession(Guid sessionId)
    {
        _dataAccess.DeleteSession(sessionId);
    }

    /// <summary>
    /// Deletes all the sessions
    /// </summary>
    public void ClearMemory()
    {
        _dataAccess.ClearMemory();
    }

    /// <summary>
    /// Returns the most frequent UserAction in the session.
    /// </summary>
    /// <param name="matches">List of matches in the session</param>
    /// <returns>Action</returns>
    private Action? GetMostFrequentAction(List<Match> matches)
    {
        /*If total matches are less than 4, return null
         Insufficient data*/
        if (matches.Count < 4 ) return null;

        var rockActionCount = 0;
        var paperActionCount = 0;
        var scissorActionCount = 0;
        
        foreach (var match in matches)
        {
            switch (match.UserAction)
            {
                case Action.Rock:
                    rockActionCount++;
                    break;
                case Action.Paper:
                    paperActionCount++;
                    break;
                case Action.Scissor:
                    scissorActionCount++;
                    break;
            }
        }

        if(rockActionCount>paperActionCount && rockActionCount>scissorActionCount) return Action.Rock;
        else if(scissorActionCount>paperActionCount && scissorActionCount>rockActionCount) return Action.Scissor;
        else if(paperActionCount>rockActionCount && paperActionCount>scissorActionCount) return Action.Paper;
        return null;
    }
}
