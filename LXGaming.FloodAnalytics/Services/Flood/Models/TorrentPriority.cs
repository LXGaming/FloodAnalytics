namespace LXGaming.FloodAnalytics.Services.Flood.Models;

// https://github.com/jesec/flood/blob/7cdf1de10743f6f4bf5f0eb553450f8e2a60e268/shared/types/Torrent.ts#L12
public enum TorrentPriority {

    DoNotDownload = 0,
    Low = 1,
    Normal = 2,
    High = 3
}