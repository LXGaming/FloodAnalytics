using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

public class TorrentListSummary {

    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("torrents")]
    public Dictionary<string, TorrentProperties>? Torrents { get; init; }
}