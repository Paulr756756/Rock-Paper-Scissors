using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Paul_RPS.Data;
using Paul_RPS.Models.RequestModels;

namespace Paul_RPS.Controllers;

[ApiController, Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    /// <summary>
    /// Creates a new session
    /// </summary>
    /// <returns>A session ID.</returns>
    [HttpPost("post/session")]
    public IActionResult CreateSession()
    {
        var session = _gameService.CreateSession();
        if (session is null) return BadRequest("Couldn't create session");
        return Ok(session.Id);
    }
    
    /// <summary>
    /// Registers user action and would generate a match.
    /// Store the results of that match in memory and return it as well.
    /// </summary>
    /// <param name="request">Send Action Request model</param>
    /// <returns>Match object</returns>
    [HttpPost("post/action")]
    public IActionResult SendAction([FromBody] SendAction request)
    {
        var match = _gameService.ProcessMatch(request);
        if (match is null) return BadRequest();
        return Ok(match);
    }

    /// <summary>
    /// Returns the statistics of the current session. 
    /// </summary>
    /// <param name="sessionId">The sessionId query parameter.</param>
    /// <returns>Statistics</returns>
    [HttpGet("get/stats")]
    public IActionResult GetStats([FromQuery] Guid sessionId)
    {
        var stats = _gameService.GetCurrentSessionStats(sessionId);
        if (stats is null) return BadRequest();
        return Ok(stats);
    }

    /// <summary>
    /// Returns the matches of this session.
    /// </summary>
    /// <param name="sessionId">Session Id query parameter.</param>
    /// <returns>List of matches</returns>
    [HttpGet("get/matches")]
    public IActionResult GetMatches([FromQuery] Guid sessionId)
    {
        var list = _gameService.GetPreviousMatches(sessionId);
        if (list is null) return BadRequest();
        return Ok(list);
    }

    /// <summary>
    /// Deletes the game session.
    /// </summary>
    /// <param name="request">The sessionID json object</param>
    /// <returns></returns>
    [HttpDelete("delete/terminate")]
    public IActionResult TerminateSession([FromBody] TerminateAction request)
    {
        _gameService.TerminateSession(request.SessionId);
        return Ok("Session Terminated");
    }
}
