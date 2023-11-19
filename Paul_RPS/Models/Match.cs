namespace Paul_RPS.Models;

/// <summary>
/// Describes the results of a match b/w human and computer.
/// </summary>
public class Match
{
    public Action UserAction { get; set; }
    public Action ComputerAction { get; set; }
    public bool UserWon { get; set; }
    public bool IsDraw { get; set; }
    //public string SessionId { get; set; }
}
