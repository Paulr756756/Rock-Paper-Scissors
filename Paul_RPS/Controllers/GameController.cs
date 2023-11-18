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
    
    [HttpPost("post/session")]
    public IActionResult CreateSession()
    {
        var session = _gameService.CreateSession();
        if (session is null) return BadRequest("Couldn't create session");
        return Ok(session.Id);
    }
    
    [HttpPost("post/action")]
    public IActionResult SendAction([FromBody] SendActionRM request)
    {
        var match = _gameService.ProcessMatch(request);
        if (match is null) return BadRequest();
        return Ok(match);
    }

    [HttpGet("get/stats")]
    public IActionResult GetStats([FromQuery] Guid sessionId)
    {
        var stats = _gameService.GetCurrentSessionStats(sessionId);
        if (stats is null) return BadRequest();
        return Ok(stats);
    }

    [HttpGet("get/matches")]
    public IActionResult GetMatches([FromQuery] Guid sessionId)
    {
        var list = _gameService.GetPreviousMatches(sessionId);
        if (list is null) return BadRequest();
        return Ok(list);
    }

    [HttpDelete("delete/terminate")]
    public IActionResult TerminateSession([FromBody] Guid sessionId)
    {
        _gameService.TerminateSession(sessionId);
        return Ok("Session Terminated");
    }
}
