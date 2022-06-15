using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

public class TorrentProperties {

    [JsonPropertyName("bytesDone")]
    public long BytesDone { get; init; }

    [JsonPropertyName("dateActive")]
    public long DateActive { get; init; }

    [JsonPropertyName("dateAdded")]
    public long DateAdded { get; init; }

    [JsonPropertyName("dateCreated")]
    public long DateCreated { get; init; }

    [JsonPropertyName("dateFinished")]
    public long DateFinished { get; init; }

    [JsonPropertyName("directory")]
    public string? Directory { get; init; }

    [JsonPropertyName("downRate")]
    public long DownRate { get; init; }

    [JsonPropertyName("downTotal")]
    public long DownTotal { get; init; }

    [JsonPropertyName("eta")]
    public decimal Eta { get; init; }

    [JsonPropertyName("hash")]
    public string? Hash { get; init; }

    [JsonPropertyName("isInitialSeeding")]
    public bool IsInitialSeeding { get; init; }

    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; init; }

    [JsonPropertyName("isSequential")]
    public bool IsSequential { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("peersConnected")]
    public int PeersConnected { get; init; }

    [JsonPropertyName("peersTotal")]
    public int PeersTotal { get; init; }

    [JsonPropertyName("percentComplete")]
    public decimal PercentComplete { get; init; }

    [JsonPropertyName("priority")]
    public TorrentPriority Priority { get; init; }

    [JsonPropertyName("ratio")]
    public decimal Ratio { get; init; }

    [JsonPropertyName("seedsConnected")]
    public int SeedsConnected { get; init; }

    [JsonPropertyName("seedsTotal")]
    public int SeedsTotal { get; init; }

    [JsonPropertyName("sizeBytes")]
    public long SizeBytes { get; init; }

    [JsonPropertyName("status")]
    public List<TorrentStatus>? Status { get; init; }

    [JsonPropertyName("tags")]
    public List<string>? Tags { get; init; }

    [JsonPropertyName("trackerURIs")]
    public List<string>? TrackerUris { get; init; }

    [JsonPropertyName("upRate")]
    public long UpRate { get; init; }

    [JsonPropertyName("upTotal")]
    public long UpTotal { get; init; }
}