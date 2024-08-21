﻿using System.Text.Json.Serialization;
using LXGaming.Common.Text.Json.Serialization.Converters;

namespace LXGaming.FloodAnalytics.Services.Flood.Models;

// https://github.com/jesec/flood/blob/7cdf1de10743f6f4bf5f0eb553450f8e2a60e268/shared/constants/torrentStatusMap.ts
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