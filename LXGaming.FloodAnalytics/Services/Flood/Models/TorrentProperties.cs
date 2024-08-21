﻿using System.Text.Json.Serialization;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

// https://github.com/jesec/flood/blob/7cdf1de10743f6f4bf5f0eb553450f8e2a60e268/shared/types/Torrent.ts#L19
public record TorrentProperties {

    [JsonPropertyName("bytesDone")]
    public long BytesDone { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonPropertyName("dateActive")]
    public long DateActive { get; init; }

    [JsonPropertyName("dateAdded")]
    public long DateAdded { get; init; }

    [JsonPropertyName("dateCreated")]
    public long DateCreated { get; init; }

    [JsonPropertyName("dateFinished")]
    public long DateFinished { get; init; }

    [JsonPropertyName("directory")]
    public required string Directory { get; init; }

    [JsonPropertyName("downRate")]
    public long DownRate { get; init; }

    [JsonPropertyName("downTotal")]
    public long DownTotal { get; init; }

    [JsonPropertyName("eta")]
    public double Eta { get; init; }

    [JsonPropertyName("hash")]
    public required string Hash { get; init; }

    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; init; }

    [JsonPropertyName("isInitialSeeding")]
    public bool IsInitialSeeding { get; init; }

    [JsonPropertyName("isSequential")]
    public bool IsSequential { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("peersConnected")]
    public int PeersConnected { get; init; }

    [JsonPropertyName("peersTotal")]
    public int PeersTotal { get; init; }

    [JsonPropertyName("percentComplete")]
    public double PercentComplete { get; init; }

    [JsonPropertyName("priority")]
    public TorrentPriority Priority { get; init; }

    [JsonPropertyName("ratio")]
    public double Ratio { get; init; }

    [JsonPropertyName("seedsConnected")]
    public int SeedsConnected { get; init; }

    [JsonPropertyName("seedsTotal")]
    public int SeedsTotal { get; init; }

    [JsonPropertyName("sizeBytes")]
    public long SizeBytes { get; init; }

    [JsonPropertyName("status")]
    public required List<TorrentStatus> Status { get; init; }

    [JsonPropertyName("tags")]
    public required List<string> Tags { get; init; }

    [JsonPropertyName("trackerURIs")]
    public required List<string> TrackerUris { get; init; }

    [JsonPropertyName("upRate")]
    public long UpRate { get; init; }

    [JsonPropertyName("upTotal")]
    public long UpTotal { get; init; }
}