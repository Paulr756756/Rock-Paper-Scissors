using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paul_RPS.Models.RequestModels;

public class TerminateAction
{
    [Required, JsonPropertyName("sessionId")]
    public Guid SessionId { get; set; }
}