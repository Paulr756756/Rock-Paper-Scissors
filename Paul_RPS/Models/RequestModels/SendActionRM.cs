using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paul_RPS.Models.RequestModels;

public class SendActionRM
{
    [Required, JsonPropertyName("sessionId")]
    public Guid SessionId { get; set; }
    
    [Required, Range(minimum:1, maximum:3), JsonPropertyName("action")]
    public int Action { get; set; }
}