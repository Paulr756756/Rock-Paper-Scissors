using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paul_RPS.Models.RequestModels;

/// <summary>
/// RequestModel for accepting user action requests.
/// </summary>
public class SendAction
{
    [Required, JsonPropertyName("sessionId")]
    public Guid SessionId { get; set; }
    
    [Required, Range(minimum:1, maximum:3), JsonPropertyName("action")]
    public int Action { get; set; }
}