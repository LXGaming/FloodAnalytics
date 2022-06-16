using System.Text.Json.Serialization;
using LXGaming.FloodAnalytics.Configuration.Categories;

namespace LXGaming.FloodAnalytics.Configuration; 

public class Config {
    
    [JsonPropertyName("flood")]
    public FloodCategory FloodCategory { get; init; } = new();
    
    [JsonPropertyName("influxDb")]
    public InfluxDbCategory InfluxDbCategory { get; init; } = new();
    
    [JsonPropertyName("quartz")]
    public QuartzCategory QuartzCategory { get; init; } = new();
}