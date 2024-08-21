using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

// https://github.com/jesec/flood/blob/7cdf1de10743f6f4bf5f0eb553450f8e2a60e268/shared/schema/api/auth.ts#L18
public record Authenticate {

    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("username")]
    public required string Username { get; init; }

    [JsonPropertyName("level")]
    public AccessLevel Level { get; init; }
}