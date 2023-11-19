using Paul_RPS.Data;
using Paul_RPS.Models;
using Paul_RPS.Models.RequestModels;

namespace Paul_RPS.Pages;


partial class Index
{
    List<Match> Matches = new();
    string ButtonText = "Start";
    Guid? SessionId { get; set; }

    Statistics Stats = new ();
    
    void GameControl()
    {
        if (ButtonText == "Start")
        {
            ButtonText = "Terminate";
            var session = GameService.CreateSession();
            SessionId = session.Id;
        }
        else
        {
            ButtonText = "Start";
            if(SessionId is not null) GameService.TerminateSession(SessionId.Value);
            SessionId = null;
            Matches = new();
            Stats = new();
        }
    }

    void GetMatches()
    {
        if (SessionId is not null)
        {
            Matches = GameService.GetPreviousMatches(SessionId.Value);
        }
    }

    void SubmitUserAction(int action)
    {
        if (action is < 1 or > 3)
        {
            Console.WriteLine("Invalid Action");
        }
        else
        {
            if (SessionId is null) return;
            var request = new SendAction()
            {
                SessionId = this.SessionId.Value,
                Action = action
            };
            //After every action add match and update stats
            Matches.Add(GameService.ProcessMatch(request));
            GetStatistics();
        }
    }

    void GetStatistics()
    {
        if(SessionId is not null) Stats = GameService.GetCurrentSessionStats(SessionId.Value);
    }

    /// <summary>
    /// Would clear all the sessions from memory
    /// </summary>
    void ClearMemory()
    {
        GameService.ClearMemory();
    }
    

}
