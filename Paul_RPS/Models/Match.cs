namespace Paul_RPS.Models;

/// <summary>
/// 
/// </summary>
public class Match
{
    public Action UserAction { get; set; }
    public Action ComputerAction { get; set; }
    public bool UserWon { get; set; }
    public bool IsDraw { get; set; }
    //public string SessionId { get; set; }
}
