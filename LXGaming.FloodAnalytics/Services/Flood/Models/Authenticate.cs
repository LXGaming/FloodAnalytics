using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

public record Authenticate {

    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("username")]
    public required string Username { get; init; }

    [JsonPropertyName("level")]
    public AccessLevel Level { get; init; }
}