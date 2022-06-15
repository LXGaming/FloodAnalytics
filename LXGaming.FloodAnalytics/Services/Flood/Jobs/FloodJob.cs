using LXGaming.FloodAnalytics.Models;
using LXGaming.FloodAnalytics.Services.Flood.Models;
using LXGaming.FloodAnalytics.Storage;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace LXGaming.FloodAnalytics.Services.Flood.Jobs; 

[DisallowConcurrentExecution, PersistJobDataAfterExecution]
public class FloodJob : IJob {
    
    public static readonly JobKey JobKey = JobKey.Create(nameof(FloodJob));
    private readonly FloodService _floodService;
    private readonly StorageContext _storageContext;

    public FloodJob(FloodService floodService, StorageContext storageContext) {
        _floodService = floodService;
        _storageContext = storageContext;
    }

    public async Task Execute(IJobExecutionContext context) {
        var torrentListSummary = await _floodService.EnsureAuthenticatedAsync(_floodService.GetTorrentsAsync());
        if (torrentListSummary?.Torrents == null || torrentListSummary.Torrents.Count == 0) {
            return;
        }
        
        foreach (var (_, value) in torrentListSummary.Torrents) {
            var torrent = await GetOrCreateTorrent(value);
            if (value.DateActive == 0 || await TrafficExists(value)) {
                continue;
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
    
    private async Task<bool> TrafficExists(TorrentProperties properties) {
        var previousTraffic = await _storageContext.Traffic
            .Where(model => string.Equals(model.TorrentId, properties.Hash))
            .OrderByDescending(model => model.Id)
            .FirstOrDefaultAsync();
        return previousTraffic != null
               && previousTraffic.BytesDone == properties.BytesDone
               && previousTraffic.SizeBytes == properties.SizeBytes
               && previousTraffic.PercentComplete == properties.PercentComplete
               && previousTraffic.Ratio == properties.Ratio
               && previousTraffic.DownRate == properties.DownRate
               && previousTraffic.DownTotal == properties.DownTotal
               && previousTraffic.UpRate == properties.UpRate
               && previousTraffic.UpTotal == properties.UpTotal
               && previousTraffic.PeersConnected == properties.PeersConnected
               && previousTraffic.PeersTotal == properties.PeersTotal
               && previousTraffic.SeedsConnected == properties.SeedsConnected
               && previousTraffic.SeedsTotal == properties.SeedsTotal;
    }
}