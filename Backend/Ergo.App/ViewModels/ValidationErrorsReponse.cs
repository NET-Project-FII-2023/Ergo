using System.Text.Json.Serialization;
using System.Collections.Generic;

public class ValidationErrorResponse
{
    [JsonPropertyName("errors")]
    public Dictionary<string, List<string>> Errors { get; set; }
}
