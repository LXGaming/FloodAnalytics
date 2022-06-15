using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TorrentStatus {

    Downloading = 0,
    Seeding = 1,
    Checking = 2,
    Complete = 3,
    Stopped = 4,
    Active = 5,
    Inactive = 6,
    Error = 7
}