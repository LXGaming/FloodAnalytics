using LXGaming.FloodAnalytics.Models;
using LXGaming.FloodAnalytics.Services.Flood.Models;
using LXGaming.FloodAnalytics.Services.Quartz;
using LXGaming.FloodAnalytics.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LXGaming.FloodAnalytics.Services.Flood.Jobs; 

[DisallowConcurrentExecution, PersistJobDataAfterExecution]
public class FloodJob : IJob {
    
    public const string TorrentsKey = "torrents";
    public static readonly JobKey JobKey = JobKey.Create(nameof(FloodJob));
    private readonly FloodService _floodService;
    private readonly ILogger<FloodJob> _logger;
    private readonly StorageContext _storageContext;

    public FloodJob(FloodService floodService, ILogger<FloodJob> logger, StorageContext storageContext) {
        _floodService = floodService;
        _logger = logger;
        _storageContext = storageContext;
    }

    public async Task Execute(IJobExecutionContext context) {
        var torrentListSummary = await _floodService.EnsureAuthenticatedAsync(_floodService.GetTorrentsAsync());
        if (torrentListSummary?.Torrents == null || torrentListSummary.Torrents.Count == 0) {
            return;
        }
        
        var activeTorrents = context.TryGetOrCreateValue<HashSet<string>>(TorrentsKey);
        
        var previousFireTime = context.PreviousFireTimeUtc?.ToUnixTimeSeconds();
        foreach (var (key, value) in torrentListSummary.Torrents) {
            var torrent = await GetOrCreateTorrent(value);
            
            if (value.DateActive == 0) { // Value not available
                continue;
            }
            
            if (previousFireTime != null && value.DateActive < previousFireTime) {
                if (value.DateActive == -1) { // Active now
                    activeTorrents.Add(key);
                } else if (!activeTorrents.Contains(key)) {
                    continue;
                }
            }
            
            _storageContext.Traffic.Add(new Traffic {
                BytesDone = value.BytesDone,
                SizeBytes = value.SizeBytes,
                PercentComplete = value.PercentComplete,
                Ratio = value.Ratio,
                DownRate = value.DownRate,
                DownTotal = value.DownTotal,
                UpRate = value.UpRate,
                UpTotal = value.UpTotal,
                PeersConnected = value.PeersConnected,
                PeersTotal = value.PeersTotal,
                SeedsConnected = value.SeedsConnected,
                SeedsTotal = value.SeedsTotal,
                CreatedAt = context.ScheduledFireTimeUtc?.LocalDateTime ?? context.FireTimeUtc.LocalDateTime,
                Torrent = torrent
            });
        }
        
        await _storageContext.SaveChangesAsync();
    }

    private async Task<Torrent> GetOrCreateTorrent(TorrentProperties properties) {
        var existingTorrent = await _storageContext.Torrents
            .SingleOrDefaultAsync(model => string.Equals(model.Id, properties.Hash));
        if (existingTorrent != null) {
            existingTorrent.Name = properties.Name!;
            existingTorrent.Trackers = properties.TrackerUris!.ToArray();
            return existingTorrent;
        }

        var torrent = new Torrent {
            Id = properties.Hash!,
            Name = properties.Name!,
            Trackers = properties.TrackerUris!.ToArray(),
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(properties.DateAdded).LocalDateTime
        };

        _storageContext.Add(torrent);
        return torrent;
    }
}