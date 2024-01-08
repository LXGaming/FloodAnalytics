using System.Text.Json.Serialization;
using LXGaming.Common.Text.Json.Serialization.Converters;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

[JsonConverter(typeof(StringEnumConverter<TorrentStatus>))]
public enum TorrentStatus {

    [JsonPropertyName("downloading")]
    Downloading = 0,

    [JsonPropertyName("seeding")]
    Seeding = 1,

    [JsonPropertyName("checking")]
    Checking = 2,

    [JsonPropertyName("complete")]
    Complete = 3,

    [JsonPropertyName("stopped")]
    Stopped = 4,

    [JsonPropertyName("active")]
    Active = 5,

    [JsonPropertyName("inactive")]
    Inactive = 6,

    [JsonPropertyName("error")]
    Error = 7
}