using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

// https://github.com/jesec/flood/blob/7cdf1de10743f6f4bf5f0eb553450f8e2a60e268/shared/types/Torrent.ts#L60
public record TorrentListSummary {

    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("torrents")]
    public required Dictionary<string, TorrentProperties> Torrents { get; init; }
}