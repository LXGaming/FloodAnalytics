using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

public class Authenticate {

    [JsonPropertyName("level")]
    public AccessLevel Level { get; init; }

    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }
}