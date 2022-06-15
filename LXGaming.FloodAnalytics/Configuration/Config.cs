using System.Text.Json.Serialization;
using LXGaming.FloodAnalytics.Configuration.Categories;

namespace LXGaming.FloodAnalytics.Configuration; 

public class Config {
    
    [JsonPropertyName("connectionStrings")]
    public Dictionary<string, string> ConnectionStrings { get; init; } = new();
    
    [JsonPropertyName("flood")]
    public FloodCategory FloodCategory { get; init; } = new();
    
    [JsonPropertyName("quartz")]
    public QuartzCategory QuartzCategory { get; init; } = new();

    [JsonPropertyName("web")]
    public WebCategory WebCategory { get; init; } = new();
}