using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Configuration.Categories; 

public class FloodCategory {

    [JsonPropertyName("address")]
    public string Address { get; init; } = "";

    [JsonPropertyName("username")]
    public string Username { get; init; } = "";

    [JsonPropertyName("password")]
    public string Password { get; init; } = "";

    [JsonPropertyName("schedule")]
    public string Schedule { get; init; } = "0 */10 * * * ?";
}